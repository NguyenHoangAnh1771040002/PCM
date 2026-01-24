<template>
  <div>
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <v-icon icon="mdi-history" class="mr-2" />
        Lịch sử trận đấu
        <v-spacer />
        <v-btn v-if="authStore.canManageMatches" color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Ghi nhận trận đấu
        </v-btn>
      </v-card-title>
    </v-card>

    <v-skeleton-loader v-if="loading" type="table" />

    <v-card v-else>
      <v-card-text>
        <v-data-table
          :headers="headers"
          :items="matches"
          :items-per-page="10"
          :sort-by="[{ key: 'date', order: 'desc' }]"
        >
          <template v-slot:item.date="{ item }">
            {{ formatDateTime(item.date) }}
          </template>
          <template v-slot:item.format="{ item }">
            <v-chip :color="item.matchFormat === 'Singles' ? 'primary' : 'secondary'" size="small">
              {{ item.matchFormat === 'Singles' ? 'Đơn' : 'Đôi' }}
            </v-chip>
          </template>
          <template v-slot:item.team1="{ item }">
            <div :class="{ 'text-success font-weight-bold': item.winningSide === 'Team1' }">
              {{ getTeamPlayers(item, 1) }}
              <v-icon v-if="item.winningSide === 'Team1'" icon="mdi-trophy" color="amber" size="small" />
            </div>
          </template>
          <template v-slot:item.team2="{ item }">
            <div :class="{ 'text-success font-weight-bold': item.winningSide === 'Team2' }">
              {{ getTeamPlayers(item, 2) }}
              <v-icon v-if="item.winningSide === 'Team2'" icon="mdi-trophy" color="amber" size="small" />
            </div>
          </template>
          <template v-slot:item.isRanked="{ item }">
            <v-chip :color="item.isRanked ? 'success' : 'grey'" size="small">
              {{ item.isRanked ? 'Ranked' : 'Unranked' }}
            </v-chip>
          </template>
          <template v-slot:item.challenge="{ item }">
            <router-link v-if="item.challengeId" :to="{ name: 'ChallengeDetail', params: { id: item.challengeId } }">
              {{ item.challenge?.title || `Kèo #${item.challengeId}` }}
            </router-link>
            <span v-else class="text-grey">Giao hữu</span>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>

    <!-- Dialog Create Match -->
    <v-dialog v-model="dialog" max-width="600">
      <v-card>
        <v-card-title>Ghi nhận trận đấu mới</v-card-title>
        <v-card-text>
          <v-form ref="formRef" v-model="valid">
            <v-row>
              <v-col cols="6">
                <v-select
                  v-model="form.matchFormat"
                  :items="formatOptions"
                  item-title="text"
                  item-value="value"
                  label="Thể thức"
                />
              </v-col>
              <v-col cols="6">
                <v-text-field v-model="form.date" label="Ngày giờ" type="datetime-local" />
              </v-col>
            </v-row>

            <v-divider class="my-3" />
            <div class="text-subtitle-2 mb-2">Team 1</div>
            <v-row>
              <v-col :cols="form.matchFormat === 'Doubles' ? 6 : 12">
                <v-select
                  v-model="form.team1_Player1Id"
                  :items="members"
                  item-title="fullName"
                  item-value="id"
                  label="Người chơi 1"
                  :rules="[(v) => !!v || 'Bắt buộc']"
                />
              </v-col>
              <v-col v-if="form.matchFormat === 'Doubles'" cols="6">
                <v-select
                  v-model="form.team1_Player2Id"
                  :items="members"
                  item-title="fullName"
                  item-value="id"
                  label="Người chơi 2"
                />
              </v-col>
            </v-row>

            <v-divider class="my-3" />
            <div class="text-subtitle-2 mb-2">Team 2</div>
            <v-row>
              <v-col :cols="form.matchFormat === 'Doubles' ? 6 : 12">
                <v-select
                  v-model="form.team2_Player1Id"
                  :items="members"
                  item-title="fullName"
                  item-value="id"
                  label="Người chơi 1"
                  :rules="[(v) => !!v || 'Bắt buộc']"
                />
              </v-col>
              <v-col v-if="form.matchFormat === 'Doubles'" cols="6">
                <v-select
                  v-model="form.team2_Player2Id"
                  :items="members"
                  item-title="fullName"
                  item-value="id"
                  label="Người chơi 2"
                />
              </v-col>
            </v-row>

            <v-divider class="my-3" />
            <v-select
              v-model="winningSide"
              :items="winningSideOptions"
              item-title="text"
              item-value="value"
              label="Bên thắng"
            />

            <v-switch v-model="form.isRanked" label="Tính điểm Rank" color="primary" />

            <v-select
              v-model="form.challengeId"
              :items="challenges"
              item-title="title"
              item-value="id"
              label="Thuộc kèo (tùy chọn)"
              clearable
            />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="createMatch">
            Lưu trận đấu
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor">{{ snackbarText }}</v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { matchService, memberService, challengeService } from '@/services'
import type { Match, MatchDto, Member, Challenge } from '@/types'
import { MatchFormat, WinningSide } from '@/types'

const authStore = useAuthStore()

const matches = ref<Match[]>([])
const members = ref<Member[]>([])
const challenges = ref<Challenge[]>([])
const loading = ref(false)
const dialog = ref(false)
const saving = ref(false)
const valid = ref(false)
const winningSide = ref<WinningSide>(WinningSide.Team1)

const form = ref<MatchDto>({
  date: '',
  isRanked: true,
  challengeId: undefined,
  matchFormat: MatchFormat.Singles,
  team1_Player1Id: 0,
  team1_Player2Id: undefined,
  team2_Player1Id: 0,
  team2_Player2Id: undefined
})

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const headers = [
  { title: 'Ngày', key: 'date' },
  { title: 'Thể thức', key: 'format' },
  { title: 'Team 1', key: 'team1' },
  { title: 'Team 2', key: 'team2' },
  { title: 'Ranked', key: 'isRanked' },
  { title: 'Kèo', key: 'challenge' }
]

const formatOptions = [
  { text: 'Đơn (Singles)', value: 'Singles' },
  { text: 'Đôi (Doubles)', value: 'Doubles' }
]

const winningSideOptions = [
  { text: 'Team 1 thắng', value: 'Team1' },
  { text: 'Team 2 thắng', value: 'Team2' }
]

const formatDateTime = (dateStr: string) => new Date(dateStr).toLocaleString('vi-VN')

const getTeamPlayers = (match: Match, team: 1 | 2) => {
  if (team === 1) {
    const p1 = (match as any).team1_Player1Name || 'N/A'
    const p2 = (match as any).team1_Player2Name
    return p2 ? `${p1} & ${p2}` : p1
  } else {
    const p1 = (match as any).team2_Player1Name || 'N/A'
    const p2 = (match as any).team2_Player2Name
    return p2 ? `${p1} & ${p2}` : p1
  }
}

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

const loadData = async () => {
  loading.value = true
  try {
    const [matchRes, memberRes, challengeRes] = await Promise.all([
      matchService.getAll(),
      memberService.getAll(),
      challengeService.getAll()
    ])
    matches.value = matchRes.data
    members.value = memberRes.data
    challenges.value = challengeRes.data.filter(c => c.status === 'Ongoing')
  } catch (e) {
    showSnackbar('Không thể tải dữ liệu', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = () => {
  form.value = {
    date: new Date().toISOString().slice(0, 16),
    isRanked: true,
    challengeId: undefined,
    matchFormat: MatchFormat.Singles,
    team1_Player1Id: 0,
    team1_Player2Id: undefined,
    team2_Player1Id: 0,
    team2_Player2Id: undefined
  }
  winningSide.value = WinningSide.Team1
  dialog.value = true
}

const createMatch = async () => {
  if (!valid.value) return
  saving.value = true
  try {
    // Create match
    const matchRes = await matchService.create(form.value)
    
    // Update result
    await matchService.updateResult(matchRes.data.id, { winningSide: winningSide.value })
    
    showSnackbar('Ghi nhận trận đấu thành công!')
    dialog.value = false
    loadData()
  } catch (e) {
    showSnackbar('Lỗi khi lưu trận đấu', 'error')
  } finally {
    saving.value = false
  }
}

onMounted(loadData)
</script>

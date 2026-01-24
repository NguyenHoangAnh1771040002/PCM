<template>
  <div>
    <v-btn icon variant="text" @click="$router.back()" class="mb-4">
      <v-icon>mdi-arrow-left</v-icon>
    </v-btn>

    <v-skeleton-loader v-if="loading" type="card" />

    <template v-else-if="challenge">
      <v-card class="mb-4">
        <v-card-title class="d-flex align-center">
          <v-icon :icon="getTypeIcon(challenge.type)" :color="getTypeColor(challenge.type)" size="32" class="mr-2" />
          {{ challenge.title }}
          <v-spacer />
          <v-chip :color="getStatusColor(challenge.status)" size="small">
            {{ getStatusText(challenge.status) }}
          </v-chip>
        </v-card-title>
        <v-card-subtitle>
          Tạo bởi: {{ (challenge as any).createdByName || 'N/A' }}
        </v-card-subtitle>
        <v-card-text>
          <p v-if="challenge.description" class="mb-4">{{ challenge.description }}</p>

          <!-- Score Board for Team Battle -->
          <v-sheet v-if="challenge.gameMode === 'TeamBattle'" color="grey-lighten-4" class="pa-4 rounded mb-4">
            <div class="text-center">
              <div class="text-overline">TEAM BATTLE</div>
              <div class="d-flex justify-center align-center">
                <div class="text-center">
                  <div class="text-h3 text-primary font-weight-bold">{{ challenge.currentScore_TeamA }}</div>
                  <div class="text-caption">Team A</div>
                </div>
                <div class="mx-6 text-h5 text-grey">VS</div>
                <div class="text-center">
                  <div class="text-h3 text-error font-weight-bold">{{ challenge.currentScore_TeamB }}</div>
                  <div class="text-caption">Team B</div>
                </div>
              </div>
              <v-progress-linear
                :model-value="getProgressA"
                :buffer-value="100"
                height="20"
                class="mt-3"
                color="primary"
                bg-color="error"
                rounded
              />
              <div class="text-caption mt-1">Mục tiêu: {{ challenge.config_TargetWins }} trận thắng</div>
            </div>
          </v-sheet>

          <v-row>
            <v-col cols="6" sm="3">
              <v-card variant="tonal" color="primary">
                <v-card-text class="text-center">
                  <v-icon icon="mdi-currency-usd" />
                  <div class="text-h6">{{ formatMoney(challenge.entryFee) }}</div>
                  <div class="text-caption">Phí tham gia</div>
                </v-card-text>
              </v-card>
            </v-col>
            <v-col cols="6" sm="3">
              <v-card variant="tonal" color="amber">
                <v-card-text class="text-center">
                  <v-icon icon="mdi-trophy" />
                  <div class="text-h6">{{ formatMoney(challenge.prizePool) }}</div>
                  <div class="text-caption">Giải thưởng</div>
                </v-card-text>
              </v-card>
            </v-col>
            <v-col cols="6" sm="3">
              <v-card variant="tonal">
                <v-card-text class="text-center">
                  <v-icon icon="mdi-calendar-start" />
                  <div class="text-h6">{{ challenge.startDate ? formatDate(challenge.startDate) : '-' }}</div>
                  <div class="text-caption">Bắt đầu</div>
                </v-card-text>
              </v-card>
            </v-col>
            <v-col cols="6" sm="3">
              <v-card variant="tonal">
                <v-card-text class="text-center">
                  <v-icon icon="mdi-calendar-end" />
                  <div class="text-h6">{{ challenge.endDate ? formatDate(challenge.endDate) : '-' }}</div>
                  <div class="text-caption">Kết thúc</div>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>

      <!-- Participants -->
      <v-row>
        <v-col cols="12" md="6">
          <v-card>
            <v-card-title class="bg-primary text-white">
              <v-icon icon="mdi-account-group" class="mr-2" />
              Team A ({{ teamAParticipants.length }})
            </v-card-title>
            <v-list>
              <v-list-item v-for="p in teamAParticipants" :key="p.id">
                <template v-slot:prepend>
                  <v-avatar color="primary" size="32">
                    <span class="text-caption">{{ getInitials((p as any).memberName || '') }}</span>
                  </v-avatar>
                </template>
                <v-list-item-title>{{ (p as any).memberName || 'N/A' }}</v-list-item-title>
                <v-list-item-subtitle>
                  DUPR: {{ (p as any).memberRank?.toFixed(1) || 'N/A' }}
                </v-list-item-subtitle>
                <template v-slot:append>
                  <v-chip :color="p.entryFeePaid ? 'success' : 'warning'" size="x-small">
                    {{ p.entryFeePaid ? 'Đã nộp phí' : 'Chưa nộp' }}
                  </v-chip>
                </template>
              </v-list-item>
              <v-list-item v-if="!teamAParticipants.length">
                <v-list-item-title class="text-grey">Chưa có thành viên</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card>
        </v-col>

        <v-col cols="12" md="6">
          <v-card>
            <v-card-title class="bg-error text-white">
              <v-icon icon="mdi-account-group" class="mr-2" />
              Team B ({{ teamBParticipants.length }})
            </v-card-title>
            <v-list>
              <v-list-item v-for="p in teamBParticipants" :key="p.id">
                <template v-slot:prepend>
                  <v-avatar color="error" size="32">
                    <span class="text-caption">{{ getInitials((p as any).memberName || '') }}</span>
                  </v-avatar>
                </template>
                <v-list-item-title>{{ (p as any).memberName || 'N/A' }}</v-list-item-title>
                <v-list-item-subtitle>
                  DUPR: {{ (p as any).memberRank?.toFixed(1) || 'N/A' }}
                </v-list-item-subtitle>
                <template v-slot:append>
                  <v-chip :color="p.entryFeePaid ? 'success' : 'warning'" size="x-small">
                    {{ p.entryFeePaid ? 'Đã nộp phí' : 'Chưa nộp' }}
                  </v-chip>
                </template>
              </v-list-item>
              <v-list-item v-if="!teamBParticipants.length">
                <v-list-item-title class="text-grey">Chưa có thành viên</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card>
        </v-col>
      </v-row>

      <!-- Matches -->
      <v-card class="mt-4" v-if="challenge.matches && challenge.matches.length">
        <v-card-title>
          <v-icon icon="mdi-sword" class="mr-2" />
          Các trận đấu
        </v-card-title>
        <v-card-text>
          <v-timeline density="compact" align="start">
            <v-timeline-item v-for="match in challenge.matches" :key="match.id" dot-color="primary" size="small">
              <div class="d-flex justify-space-between align-center">
                <div>
                  <strong>{{ getMatchPlayers(match, 1) }}</strong>
                  vs
                  <strong>{{ getMatchPlayers(match, 2) }}</strong>
                </div>
                <v-chip :color="match.winningSide === 'Team1' ? 'primary' : 'error'" size="small">
                  {{ match.winningSide === 'Team1' ? 'Team 1 thắng' : 'Team 2 thắng' }}
                </v-chip>
              </div>
              <div class="text-caption text-grey">{{ formatDateTime(match.date) }}</div>
            </v-timeline-item>
          </v-timeline>
        </v-card-text>
      </v-card>
    </template>

    <v-alert v-else type="error" variant="tonal">
      Không tìm thấy kèo đấu này
    </v-alert>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { challengeService } from '@/services'
import type { Challenge, ChallengeType, ChallengeStatus, Match } from '@/types'

const route = useRoute()

const challenge = ref<Challenge | null>(null)
const loading = ref(false)

const teamAParticipants = computed(() => 
  challenge.value?.participants?.filter(p => p.team === 'TeamA') || []
)

const teamBParticipants = computed(() => 
  challenge.value?.participants?.filter(p => p.team === 'TeamB') || []
)

const getProgressA = computed(() => {
  if (!challenge.value?.config_TargetWins) return 50
  const total = challenge.value.config_TargetWins * 2
  return (challenge.value.currentScore_TeamA / total) * 100
})

const getTypeIcon = (type: ChallengeType) => type === 'Duel' ? 'mdi-sword-cross' : 'mdi-gamepad-variant'
const getTypeColor = (type: ChallengeType) => type === 'Duel' ? 'warning' : 'primary'

const getStatusColor = (status: ChallengeStatus) => {
  switch (status) {
    case 'Open': return 'success'
    case 'Ongoing': return 'primary'
    case 'Completed': return 'info'
    case 'Cancelled': return 'error'
    default: return 'grey'
  }
}

const getStatusText = (status: ChallengeStatus) => {
  switch (status) {
    case 'Open': return 'Mở đăng ký'
    case 'Ongoing': return 'Đang diễn ra'
    case 'Completed': return 'Hoàn thành'
    case 'Cancelled': return 'Đã hủy'
    default: return status
  }
}

const formatMoney = (amount: number) => new Intl.NumberFormat('vi-VN').format(amount) + 'đ'
const formatDate = (dateStr: string) => new Date(dateStr).toLocaleDateString('vi-VN')
const formatDateTime = (dateStr: string) => new Date(dateStr).toLocaleString('vi-VN')

const getInitials = (name: string) => {
  return name.split(' ').map(n => n[0]).slice(0, 2).join('').toUpperCase()
}

const getMatchPlayers = (match: Match, team: 1 | 2) => {
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

onMounted(async () => {
  const id = Number(route.params.id)
  if (!id) return

  loading.value = true
  try {
    const res = await challengeService.getById(id)
    challenge.value = res.data
  } catch (e) {
    console.error('Failed to load challenge:', e)
  } finally {
    loading.value = false
  }
})
</script>

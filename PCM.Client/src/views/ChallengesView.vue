<template>
  <div>
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <v-icon icon="mdi-sword-cross" class="mr-2" />
        Thách đấu & Mini-game
        <v-spacer />
        <v-btn color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Tạo kèo mới
        </v-btn>
      </v-card-title>
    </v-card>

    <v-tabs v-model="tab" color="primary">
      <v-tab value="ongoing">Đang diễn ra</v-tab>
      <v-tab value="open">Mở đăng ký</v-tab>
      <v-tab value="completed">Đã kết thúc</v-tab>
    </v-tabs>

    <v-tabs-window v-model="tab">
      <v-tabs-window-item value="ongoing">
        <v-row class="mt-2">
          <v-col v-for="challenge in ongoingChallenges" :key="challenge.id" cols="12" md="6">
            <ChallengeCard :challenge="challenge" @view="viewChallenge" />
          </v-col>
          <v-col v-if="!ongoingChallenges.length" cols="12">
            <v-alert type="info" variant="tonal">Không có kèo nào đang diễn ra</v-alert>
          </v-col>
        </v-row>
      </v-tabs-window-item>

      <v-tabs-window-item value="open">
        <v-row class="mt-2">
          <v-col v-for="challenge in openChallenges" :key="challenge.id" cols="12" md="6">
            <ChallengeCard :challenge="challenge" @view="viewChallenge" @join="joinChallenge" />
          </v-col>
          <v-col v-if="!openChallenges.length" cols="12">
            <v-alert type="info" variant="tonal">Không có kèo nào đang mở</v-alert>
          </v-col>
        </v-row>
      </v-tabs-window-item>

      <v-tabs-window-item value="completed">
        <v-row class="mt-2">
          <v-col v-for="challenge in completedChallenges" :key="challenge.id" cols="12" md="6">
            <ChallengeCard :challenge="challenge" @view="viewChallenge" />
          </v-col>
          <v-col v-if="!completedChallenges.length" cols="12">
            <v-alert type="info" variant="tonal">Không có kèo nào đã kết thúc</v-alert>
          </v-col>
        </v-row>
      </v-tabs-window-item>
    </v-tabs-window>

    <!-- Dialog Create Challenge -->
    <v-dialog v-model="dialog" max-width="600">
      <v-card>
        <v-card-title>Tạo kèo mới</v-card-title>
        <v-card-text>
          <v-form ref="formRef" v-model="valid">
            <v-text-field
              v-model="form.title"
              label="Tiêu đề"
              :rules="[(v) => !!v || 'Tiêu đề là bắt buộc']"
            />
            <v-select
              v-model="form.type"
              :items="typeOptions"
              item-title="text"
              item-value="value"
              label="Loại kèo"
            />
            <v-select
              v-if="form.type === 'MiniGame'"
              v-model="form.gameMode"
              :items="gameModeOptions"
              item-title="text"
              item-value="value"
              label="Chế độ chơi"
            />
            <v-text-field
              v-if="form.gameMode === 'TeamBattle'"
              v-model.number="form.config_TargetWins"
              label="Số trận thắng mục tiêu"
              type="number"
              min="1"
            />
            <v-text-field
              v-model.number="form.entryFee"
              label="Phí tham gia (VNĐ)"
              type="number"
              min="0"
            />
            <v-text-field
              v-model.number="form.prizePool"
              label="Giải thưởng (VNĐ)"
              type="number"
              min="0"
            />
            <v-row>
              <v-col cols="6">
                <v-text-field v-model="form.startDate" label="Ngày bắt đầu" type="date" />
              </v-col>
              <v-col cols="6">
                <v-text-field v-model="form.endDate" label="Ngày kết thúc" type="date" />
              </v-col>
            </v-row>
            <v-textarea v-model="form.description" label="Mô tả" rows="3" />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="createChallenge">
            Tạo kèo
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor">{{ snackbarText }}</v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { challengeService } from '@/services'
import type { Challenge, ChallengeDto } from '@/types'
import { ChallengeType, GameMode } from '@/types'
import ChallengeCard from '@/components/ChallengeCard.vue'

const router = useRouter()

const challenges = ref<Challenge[]>([])
const loading = ref(false)
const dialog = ref(false)
const saving = ref(false)
const valid = ref(false)
const tab = ref('ongoing')

const form = ref<ChallengeDto>({
  title: '',
  type: ChallengeType.Duel,
  gameMode: GameMode.None,
  config_TargetWins: 5,
  entryFee: 0,
  prizePool: 0,
  description: '',
  startDate: '',
  endDate: ''
})

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const typeOptions = [
  { text: 'Thách đấu 1v1', value: 'Duel' },
  { text: 'Mini-game', value: 'MiniGame' }
]

const gameModeOptions = [
  { text: 'Team Battle (Chia phe)', value: 'TeamBattle' },
  { text: 'Tournament (Giải đấu)', value: 'Tournament' }
]

const openChallenges = computed(() => challenges.value.filter(c => c.status === 'Open'))
const ongoingChallenges = computed(() => challenges.value.filter(c => c.status === 'Ongoing'))
const completedChallenges = computed(() => challenges.value.filter(c => c.status === 'Completed' || c.status === 'Cancelled'))

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

const loadChallenges = async () => {
  loading.value = true
  try {
    const res = await challengeService.getAll()
    challenges.value = res.data
  } catch (e) {
    showSnackbar('Không thể tải danh sách kèo', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = () => {
  form.value = {
    title: '',
    type: ChallengeType.Duel,
    gameMode: GameMode.None,
    config_TargetWins: 5,
    entryFee: 0,
    prizePool: 0,
    description: '',
    startDate: '',
    endDate: ''
  }
  dialog.value = true
}

const createChallenge = async () => {
  if (!valid.value) return
  saving.value = true
  try {
    await challengeService.create(form.value)
    showSnackbar('Tạo kèo thành công!')
    dialog.value = false
    loadChallenges()
  } catch (e) {
    showSnackbar('Lỗi khi tạo kèo', 'error')
  } finally {
    saving.value = false
  }
}

const viewChallenge = (challenge: Challenge) => {
  router.push({ name: 'ChallengeDetail', params: { id: challenge.id } })
}

const joinChallenge = async (challenge: Challenge) => {
  try {
    await challengeService.join(challenge.id, { challengeId: challenge.id, entryFeeAmount: challenge.entryFee })
    showSnackbar('Đăng ký tham gia thành công!')
    loadChallenges()
  } catch (e) {
    showSnackbar('Lỗi khi đăng ký', 'error')
  }
}

onMounted(loadChallenges)
</script>

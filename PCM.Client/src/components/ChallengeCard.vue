<template>
  <v-card :class="{ 'border-primary': challenge.status === 'Ongoing' }">
    <v-card-title class="d-flex align-center">
      <v-icon :icon="getTypeIcon(challenge.type)" :color="getTypeColor(challenge.type)" class="mr-2" />
      {{ challenge.title }}
      <v-spacer />
      <v-chip :color="getStatusColor(challenge.status)" size="small">
        {{ getStatusText(challenge.status) }}
      </v-chip>
    </v-card-title>

    <v-card-text>
      <!-- Score Board for Team Battle -->
      <v-sheet v-if="challenge.gameMode === 'TeamBattle'" color="grey-lighten-4" class="pa-3 rounded mb-3">
        <div class="text-center">
          <span class="text-h5 text-primary font-weight-bold">{{ challenge.currentScore_TeamA }}</span>
          <span class="mx-3 text-grey">vs</span>
          <span class="text-h5 text-error font-weight-bold">{{ challenge.currentScore_TeamB }}</span>
        </div>
        <div class="text-center text-caption">
          Mục tiêu: {{ challenge.config_TargetWins }} trận thắng
        </div>
      </v-sheet>

      <p v-if="challenge.description" class="mb-2">{{ challenge.description }}</p>

      <div class="d-flex flex-wrap gap-2 mb-2">
        <v-chip size="small" prepend-icon="mdi-currency-usd">
          Phí: {{ formatMoney(challenge.entryFee) }}
        </v-chip>
        <v-chip size="small" prepend-icon="mdi-trophy" color="amber">
          Giải: {{ formatMoney(challenge.prizePool) }}
        </v-chip>
        <v-chip size="small" prepend-icon="mdi-account-group">
          {{ challenge.participants?.length || 0 }} người
        </v-chip>
      </div>

      <div v-if="challenge.startDate || challenge.endDate" class="text-caption text-grey">
        📅 {{ challenge.startDate ? formatDate(challenge.startDate) : '?' }} 
        - {{ challenge.endDate ? formatDate(challenge.endDate) : '?' }}
      </div>
    </v-card-text>

    <v-card-actions>
      <v-btn color="primary" variant="text" @click="$emit('view', challenge)">
        <v-icon left>mdi-eye</v-icon> Chi tiết
      </v-btn>
      <v-btn
        v-if="challenge.status === 'Open'"
        color="success"
        variant="tonal"
        @click="$emit('join', challenge)"
      >
        <v-icon left>mdi-account-plus</v-icon> Tham gia
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import type { Challenge, ChallengeType, ChallengeStatus } from '@/types'

defineProps<{
  challenge: Challenge
}>()

defineEmits<{
  (e: 'view', challenge: Challenge): void
  (e: 'join', challenge: Challenge): void
}>()

const getTypeIcon = (type: ChallengeType) => {
  return type === 'Duel' ? 'mdi-sword-cross' : 'mdi-gamepad-variant'
}

const getTypeColor = (type: ChallengeType) => {
  return type === 'Duel' ? 'warning' : 'primary'
}

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

const formatMoney = (amount: number) => {
  return new Intl.NumberFormat('vi-VN').format(amount) + 'đ'
}

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('vi-VN')
}
</script>

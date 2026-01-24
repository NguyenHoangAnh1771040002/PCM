<template>
  <div>
    <v-card>
      <v-card-title class="d-flex align-center">
        <v-icon icon="mdi-trophy" class="mr-2" />
        Xếp hạng DUPR - CLB Pickleball
      </v-card-title>
      <v-card-text>
        <v-skeleton-loader v-if="loading" type="table" />
        <v-data-table
          v-else
          :headers="headers"
          :items="members"
          :items-per-page="15"
          class="elevation-0"
        >
          <template v-slot:item.rank="{ index }">
            <v-chip :color="getMedalColor(index)" size="small">
              {{ index + 1 }}
            </v-chip>
          </template>
          <template v-slot:item.fullName="{ item }">
            <div class="d-flex align-center">
              <v-avatar :color="getMedalColor(members.indexOf(item))" size="32" class="mr-2">
                <span class="text-white text-caption">{{ getInitials(item.fullName) }}</span>
              </v-avatar>
              {{ item.fullName }}
            </div>
          </template>
          <template v-slot:item.rankLevel="{ item }">
            <v-chip color="primary" variant="elevated">
              {{ item.rankLevel.toFixed(2) }}
            </v-chip>
          </template>
          <template v-slot:item.winRate="{ item }">
            <v-progress-linear
              :model-value="getWinRate(item)"
              :color="getWinRateColor(item)"
              height="20"
              rounded
            >
              <template v-slot:default>
                <span class="text-caption">{{ getWinRate(item).toFixed(1) }}%</span>
              </template>
            </v-progress-linear>
          </template>
          <template v-slot:item.stats="{ item }">
            <span class="text-success">{{ item.winMatches }}W</span>
            /
            <span class="text-error">{{ item.totalMatches - item.winMatches }}L</span>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { memberService } from '@/services'
import type { Member } from '@/types'

const loading = ref(false)
const members = ref<Member[]>([])

const headers = [
  { title: '#', key: 'rank', sortable: false, width: '60px' },
  { title: 'Hội viên', key: 'fullName' },
  { title: 'DUPR', key: 'rankLevel' },
  { title: 'Tỉ lệ thắng', key: 'winRate', sortable: false },
  { title: 'Thống kê', key: 'stats', sortable: false }
]

const getMedalColor = (index: number) => {
  if (index === 0) return 'amber-darken-1'
  if (index === 1) return 'grey-lighten-1'
  if (index === 2) return 'brown-lighten-1'
  return 'grey'
}

const getInitials = (name: string) => {
  return name
    .split(' ')
    .map(n => n[0])
    .slice(0, 2)
    .join('')
    .toUpperCase()
}

const getWinRate = (member: Member) => {
  if (member.totalMatches === 0) return 0
  return (member.winMatches / member.totalMatches) * 100
}

const getWinRateColor = (member: Member) => {
  const rate = getWinRate(member)
  if (rate >= 70) return 'success'
  if (rate >= 50) return 'primary'
  if (rate >= 30) return 'warning'
  return 'error'
}

onMounted(async () => {
  loading.value = true
  try {
    const res = await memberService.getRanking()
    members.value = res.data
  } catch (e) {
    console.error('Failed to load ranking:', e)
  } finally {
    loading.value = false
  }
})
</script>

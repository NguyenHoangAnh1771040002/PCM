<template>
  <div>
    <!-- Cảnh báo quỹ âm cho Admin/Treasurer -->
    <v-alert
      v-if="showBalanceWarning && currentBalance < 0"
      type="error"
      variant="tonal"
      class="mb-4"
      prominent
      closable
    >
      <v-alert-title>⚠️ CẢNH BÁO: QUỸ CLB ĐANG ÂM!</v-alert-title>
      <div>
        Số dư hiện tại: <strong class="text-error">{{ formatCurrency(currentBalance) }}</strong>.
        Vui lòng kiểm tra và xử lý ngay!
      </div>
      <template v-slot:append>
        <v-btn color="error" variant="elevated" :to="{ name: 'Finance' }">
          Xem tài chính
        </v-btn>
      </template>
    </v-alert>

    <!-- Hero Section -->
    <v-card color="primary" variant="flat" class="mb-6">
      <v-card-text class="text-center py-10">
        <v-icon icon="mdi-badminton" size="80" class="mb-4" />
        <h1 class="text-h3 font-weight-bold mb-2">CLB PICKLEBALL</h1>
        <h2 class="text-h5 mb-4">VỢT THỦ PHỐ NÚI</h2>
        <p class="text-subtitle-1 mb-4">
          Nơi kết nối đam mê Pickleball - Thể thao số 1 thế giới đang phát triển!
        </p>
        <v-btn v-if="!authStore.isAuthenticated" color="secondary" size="large" :to="{ name: 'Register' }">
          🎾 Tham gia ngay
        </v-btn>
      </v-card-text>
    </v-card>

    <v-row>
      <!-- News Section -->
      <v-col cols="12" md="8">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-newspaper" class="mr-2" />
            Tin tức mới nhất
          </v-card-title>
          <v-card-text>
            <v-skeleton-loader v-if="loadingNews" type="article" />
            <template v-else>
              <v-alert v-if="!newsList.length" type="info" variant="tonal">
                Chưa có tin tức nào.
              </v-alert>
              <v-list v-else>
                <v-list-item
                  v-for="news in newsList.slice(0, 5)"
                  :key="news.id"
                  :prepend-icon="news.isPinned ? 'mdi-pin' : 'mdi-newspaper-variant-outline'"
                  class="mb-2"
                >
                  <v-list-item-title class="font-weight-medium">
                    {{ news.title }}
                  </v-list-item-title>
                  <v-list-item-subtitle>
                    {{ news.content.substring(0, 100) }}...
                  </v-list-item-subtitle>
                  <template v-slot:append>
                    <v-chip size="small" variant="outlined">
                      {{ formatDate(news.createdDate) }}
                    </v-chip>
                  </template>
                </v-list-item>
              </v-list>
            </template>
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" :to="{ name: 'News' }">Xem tất cả tin tức</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <!-- Quick Stats -->
      <v-col cols="12" md="4">
        <v-card class="mb-4">
          <v-card-title>
            <v-icon icon="mdi-trophy" class="mr-2" />
            Top 5 DUPR
          </v-card-title>
          <v-card-text>
            <v-skeleton-loader v-if="loadingRanking" type="list-item-avatar-three-line" />
            <template v-else>
              <v-list density="compact">
                <v-list-item
                  v-for="(member, index) in topMembers"
                  :key="member.id"
                >
                  <template v-slot:prepend>
                    <v-avatar :color="getMedalColor(index)" size="32">
                      <span class="text-white font-weight-bold">{{ index + 1 }}</span>
                    </v-avatar>
                  </template>
                  <v-list-item-title>{{ member.fullName }}</v-list-item-title>
                  <template v-slot:append>
                    <v-chip color="primary" size="small">{{ member.rankLevel.toFixed(1) }}</v-chip>
                  </template>
                </v-list-item>
              </v-list>
            </template>
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" :to="{ name: 'Ranking' }">Xem bảng xếp hạng</v-btn>
          </v-card-actions>
        </v-card>

        <!-- Quick Actions -->
        <v-card v-if="authStore.isAuthenticated">
          <v-card-title>
            <v-icon icon="mdi-lightning-bolt" class="mr-2" />
            Hành động nhanh
          </v-card-title>
          <v-card-text>
            <v-btn block color="primary" class="mb-2" :to="{ name: 'Bookings' }" prepend-icon="mdi-calendar-plus">
              Đặt sân
            </v-btn>
            <v-btn block color="secondary" class="mb-2" :to="{ name: 'Challenges' }" prepend-icon="mdi-sword-cross">
              Thách đấu
            </v-btn>
            <v-btn block variant="outlined" :to="{ name: 'Matches' }" prepend-icon="mdi-history">
              Lịch sử trận đấu
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { newsService, memberService, transactionService } from '@/services'
import type { News, Member } from '@/types'

const authStore = useAuthStore()

const newsList = ref<News[]>([])
const topMembers = ref<Member[]>([])
const loadingNews = ref(false)
const loadingRanking = ref(false)
const currentBalance = ref(0)

// Chỉ hiện cảnh báo cho Admin và Treasurer
const showBalanceWarning = computed(() => {
  return authStore.isAuthenticated && 
    (authStore.user?.roles?.includes('Admin') || authStore.user?.roles?.includes('Treasurer'))
})

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('vi-VN')
}

const formatCurrency = (value: number) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value)
}

const getMedalColor = (index: number) => {
  if (index === 0) return 'amber-darken-1'
  if (index === 1) return 'grey-lighten-1'
  if (index === 2) return 'brown-lighten-1'
  return 'grey'
}

onMounted(async () => {
  // Load news
  loadingNews.value = true
  try {
    const res = await newsService.getAll()
    newsList.value = res.data
  } catch (e) {
    console.error('Failed to load news:', e)
  } finally {
    loadingNews.value = false
  }

  // Load ranking
  loadingRanking.value = true
  try {
    const res = await memberService.getRanking()
    topMembers.value = res.data.slice(0, 5)
  } catch (e) {
    console.error('Failed to load ranking:', e)
  } finally {
    loadingRanking.value = false
  }

  // Load balance for Admin/Treasurer
  if (showBalanceWarning.value) {
    try {
      const res = await transactionService.getSummary()
      currentBalance.value = res.data.balance
    } catch (e) {
      console.error('Failed to load balance:', e)
    }
  }
})
</script>

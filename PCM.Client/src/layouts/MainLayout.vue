<template>
  <div>
    <!-- App Bar -->
    <v-app-bar color="primary" density="comfortable">
      <v-app-bar-nav-icon @click="drawer = !drawer" />
    
    <v-app-bar-title class="d-flex align-center">
      <v-icon icon="mdi-badminton" class="mr-2" />
      <span class="font-weight-bold">PCM</span>
      <span class="ml-2 text-caption d-none d-sm-inline">Vợt Thủ Phố Núi</span>
    </v-app-bar-title>

    <v-spacer />

      <!-- Theme Toggle -->
      <v-btn icon @click="toggleTheme">
        <v-icon>{{ isDark ? 'mdi-weather-sunny' : 'mdi-weather-night' }}</v-icon>
      </v-btn>

      <!-- User Menu -->
      <template v-if="authStore.isAuthenticated">
        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn v-bind="props" variant="text">
              <v-avatar color="secondary" size="32" class="mr-2">
                <span class="text-caption">{{ userInitials }}</span>
              </v-avatar>
              <span class="d-none d-sm-inline">{{ authStore.user?.fullName }}</span>
              <v-icon right>mdi-chevron-down</v-icon>
            </v-btn>
          </template>
          <v-list>
            <v-list-item>
              <v-list-item-title>{{ authStore.user?.email }}</v-list-item-title>
              <v-list-item-subtitle>
                <v-chip v-for="role in authStore.user?.roles" :key="role" size="x-small" class="mr-1">
                  {{ role }}
                </v-chip>
              </v-list-item-subtitle>
            </v-list-item>
            <v-divider />
            <v-list-item @click="logout" prepend-icon="mdi-logout">
              <v-list-item-title>Đăng xuất</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </template>
      <template v-else>
        <v-btn variant="outlined" :to="{ name: 'Login' }">Đăng nhập</v-btn>
      </template>
    </v-app-bar>

    <!-- Navigation Drawer -->
    <v-navigation-drawer v-model="drawer" temporary>
      <v-list density="compact" nav>
        <v-list-item
          v-for="item in menuItems"
          :key="item.title"
          :to="item.to"
          :prepend-icon="item.icon"
          :title="item.title"
          link
        />
      </v-list>
    </v-navigation-drawer>

    <!-- Main Content -->
    <v-main>
      <v-container fluid>
        <router-view />
      </v-container>
    </v-main>

    <!-- Footer -->
    <v-footer app class="bg-surface text-center">
      <v-col class="text-center text-caption" cols="12">
        © 2026 CLB Pickleball Vợt Thủ Phố Núi - PCM
      </v-col>
    </v-footer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useTheme } from 'vuetify'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const theme = useTheme()
const authStore = useAuthStore()
const drawer = ref(false)

const isDark = computed(() => theme.global.name.value === 'dark')

const toggleTheme = () => {
  theme.global.name.value = isDark.value ? 'light' : 'dark'
}

const userInitials = computed(() => {
  if (!authStore.user?.fullName) return '?'
  return authStore.user.fullName
    .split(' ')
    .map(n => n[0])
    .slice(0, 2)
    .join('')
    .toUpperCase()
})

const menuItems = computed(() => {
  const items = [
    { title: 'Trang chủ', icon: 'mdi-home', to: { name: 'Home' } },
    { title: 'Tin tức', icon: 'mdi-newspaper', to: { name: 'News' } },
    { title: 'Xếp hạng DUPR', icon: 'mdi-trophy', to: { name: 'Ranking' } }
  ]

  if (authStore.isAuthenticated) {
    // Menu cho tất cả member
    items.push(
      { title: 'Đặt sân', icon: 'mdi-calendar-clock', to: { name: 'Bookings' } },
      { title: 'Thách đấu', icon: 'mdi-sword-cross', to: { name: 'Challenges' } },
      { title: 'Lịch sử trận đấu', icon: 'mdi-history', to: { name: 'Matches' } }
    )

    // Menu chỉ cho Admin
    if (authStore.isAdmin) {
      items.push(
        { title: 'Quản lý hội viên', icon: 'mdi-account-group', to: { name: 'Members' } },
        { title: 'Quản lý sân', icon: 'mdi-tennis-ball', to: { name: 'Courts' } }
      )
    }

    // Menu cho Admin/Treasurer
    if (authStore.canManageFinance) {
      items.push({ title: 'Quản lý thu chi', icon: 'mdi-cash-multiple', to: { name: 'Finance' } })
    }
  }

  return items
})

const logout = () => {
  authStore.logout()
  router.push({ name: 'Login' })
}
</script>

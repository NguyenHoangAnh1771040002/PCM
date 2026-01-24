import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes = [
  {
    path: '/',
    component: () => import('@/layouts/MainLayout.vue'),
    children: [
      {
        path: '',
        name: 'Home',
        component: () => import('@/views/HomeView.vue'),
        meta: { title: 'Trang chủ' }
      },
      {
        path: 'news',
        name: 'News',
        component: () => import('@/views/NewsView.vue'),
        meta: { title: 'Tin tức' }
      },
      {
        path: 'members',
        name: 'Members',
        component: () => import('@/views/MembersView.vue'),
        meta: { title: 'Hội viên', requiresAuth: true, roles: ['Admin'] }
      },
      {
        path: 'ranking',
        name: 'Ranking',
        component: () => import('@/views/RankingView.vue'),
        meta: { title: 'Xếp hạng DUPR' }
      },
      {
        path: 'courts',
        name: 'Courts',
        component: () => import('@/views/CourtsView.vue'),
        meta: { title: 'Quản lý sân', requiresAuth: true, roles: ['Admin'] }
      },
      {
        path: 'bookings',
        name: 'Bookings',
        component: () => import('@/views/BookingsView.vue'),
        meta: { title: 'Đặt sân', requiresAuth: true }
      },
      {
        path: 'finance',
        name: 'Finance',
        component: () => import('@/views/FinanceView.vue'),
        meta: { title: 'Thu chi', requiresAuth: true, roles: ['Admin', 'Treasurer'] }
      },
      {
        path: 'challenges',
        name: 'Challenges',
        component: () => import('@/views/ChallengesView.vue'),
        meta: { title: 'Thách đấu & Mini-game', requiresAuth: true }
      },
      {
        path: 'challenges/:id',
        name: 'ChallengeDetail',
        component: () => import('@/views/ChallengeDetailView.vue'),
        meta: { title: 'Chi tiết kèo', requiresAuth: true }
      },
      {
        path: 'matches',
        name: 'Matches',
        component: () => import('@/views/MatchesView.vue'),
        meta: { title: 'Lịch sử trận đấu', requiresAuth: true }
      }
    ]
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/LoginView.vue'),
    meta: { title: 'Đăng nhập', guest: true }
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('@/views/RegisterView.vue'),
    meta: { title: 'Đăng ký', guest: true }
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('@/views/NotFoundView.vue'),
    meta: { title: '404' }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Navigation Guard
router.beforeEach(async (to, _from, next) => {
  const authStore = useAuthStore()
  
  // Set page title
  document.title = `${to.meta.title || 'PCM'} - CLB Pickleball Vợt Thủ Phố Núi`

  // Guest routes (login, register)
  if (to.meta.guest && authStore.isAuthenticated) {
    return next({ name: 'Home' })
  }

  // Protected routes
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return next({ name: 'Login', query: { redirect: to.fullPath } })
  }

  // Role-based routes
  const requiredRoles = to.meta.roles as string[] | undefined
  if (requiredRoles && requiredRoles.length > 0) {
    const userRoles = authStore.user?.roles || []
    const hasRole = requiredRoles.some(role => userRoles.includes(role))
    if (!hasRole) {
      return next({ name: 'Home' })
    }
  }

  next()
})

export default router

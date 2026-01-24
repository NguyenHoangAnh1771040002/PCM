import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { UserInfo, AuthResponse } from '@/types'
import { authService } from '@/services'

export const useAuthStore = defineStore('auth', () => {
  // State
  const token = ref<string | null>(localStorage.getItem('token'))
  const user = ref<UserInfo | null>(
    localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')!) : null
  )
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Getters
  const isAuthenticated = computed(() => !!token.value)
  const isAdmin = computed(() => user.value?.roles?.includes('Admin') ?? false)
  const isTreasurer = computed(() => user.value?.roles?.includes('Treasurer') ?? false)
  const isReferee = computed(() => user.value?.roles?.includes('Referee') ?? false)
  const isMember = computed(() => user.value?.roles?.includes('Member') ?? false)
  const canManageFinance = computed(() => isAdmin.value || isTreasurer.value)
  const canManageMatches = computed(() => isAdmin.value || isReferee.value)

  // Actions
  async function login(email: string, password: string) {
    loading.value = true
    error.value = null
    try {
      const response = await authService.login({ email, password })
      setAuth(response.data)
      return true
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Đăng nhập thất bại'
      return false
    } finally {
      loading.value = false
    }
  }

  async function register(email: string, password: string, fullName: string, phoneNumber?: string) {
    loading.value = true
    error.value = null
    try {
      const response = await authService.register({ email, password, fullName, phoneNumber })
      setAuth(response.data)
      return true
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Đăng ký thất bại'
      return false
    } finally {
      loading.value = false
    }
  }

  function setAuth(data: AuthResponse) {
    token.value = data.token
    user.value = {
      email: data.email,
      fullName: data.fullName,
      roles: data.roles,
      memberId: data.memberId
    }
    localStorage.setItem('token', data.token)
    localStorage.setItem('user', JSON.stringify(user.value))
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  async function checkAuth() {
    if (!token.value) return false
    try {
      const response = await authService.me()
      user.value = {
        email: response.data.email,
        fullName: response.data.fullName,
        roles: response.data.roles,
        memberId: response.data.memberId
      }
      return true
    } catch {
      logout()
      return false
    }
  }

  return {
    // State
    token,
    user,
    loading,
    error,
    // Getters
    isAuthenticated,
    isAdmin,
    isTreasurer,
    isReferee,
    isMember,
    canManageFinance,
    canManageMatches,
    // Actions
    login,
    register,
    logout,
    checkAuth
  }
})

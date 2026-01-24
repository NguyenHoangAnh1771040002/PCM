<template>
  <v-container>
    <v-row justify="center" class="mt-10">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="pa-4">
          <v-card-title class="text-center">
            <v-icon icon="mdi-badminton" size="48" color="primary" class="mb-2" />
            <div class="text-h5">Đăng ký tài khoản</div>
            <div class="text-subtitle-2 text-grey">CLB Pickleball Vợt Thủ Phố Núi</div>
          </v-card-title>

          <v-card-text>
            <v-alert v-if="authStore.error" type="error" variant="tonal" class="mb-4" closable>
              {{ authStore.error }}
            </v-alert>

            <v-form ref="formRef" v-model="valid" @submit.prevent="handleRegister">
              <v-text-field
                v-model="fullName"
                label="Họ và tên"
                prepend-inner-icon="mdi-account"
                :rules="nameRules"
                required
              />

              <v-text-field
                v-model="email"
                label="Email"
                type="email"
                prepend-inner-icon="mdi-email"
                :rules="emailRules"
                required
              />

              <v-text-field
                v-model="phoneNumber"
                label="Số điện thoại"
                prepend-inner-icon="mdi-phone"
                :rules="phoneRules"
              />

              <v-text-field
                v-model="password"
                label="Mật khẩu"
                :type="showPassword ? 'text' : 'password'"
                prepend-inner-icon="mdi-lock"
                :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append-inner="showPassword = !showPassword"
                :rules="passwordRules"
                required
              />

              <v-text-field
                v-model="confirmPassword"
                label="Xác nhận mật khẩu"
                :type="showPassword ? 'text' : 'password'"
                prepend-inner-icon="mdi-lock-check"
                :rules="confirmPasswordRules"
                required
              />

              <v-btn
                type="submit"
                color="primary"
                block
                size="large"
                :loading="authStore.loading"
                :disabled="!valid"
                class="mt-4"
              >
                Đăng ký
              </v-btn>
            </v-form>
          </v-card-text>

          <v-divider class="my-4" />

          <v-card-text class="text-center">
            <span class="text-grey">Đã có tài khoản?</span>
            <router-link :to="{ name: 'Login' }" class="text-primary ml-1">
              Đăng nhập
            </router-link>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const formRef = ref()
const valid = ref(false)
const fullName = ref('')
const email = ref('')
const phoneNumber = ref('')
const password = ref('')
const confirmPassword = ref('')
const showPassword = ref(false)

const nameRules = [
  (v: string) => !!v || 'Họ tên là bắt buộc',
  (v: string) => v.length >= 2 || 'Họ tên phải có ít nhất 2 ký tự'
]

const emailRules = [
  (v: string) => !!v || 'Email là bắt buộc',
  (v: string) => /.+@.+\..+/.test(v) || 'Email không hợp lệ'
]

const phoneRules = [
  (v: string) => !v || /^0\d{9}$/.test(v) || 'Số điện thoại không hợp lệ (VD: 0901234567)'
]

const passwordRules = [
  (v: string) => !!v || 'Mật khẩu là bắt buộc',
  (v: string) => v.length >= 6 || 'Mật khẩu phải có ít nhất 6 ký tự'
]

const confirmPasswordRules = [
  (v: string) => !!v || 'Xác nhận mật khẩu là bắt buộc',
  (v: string) => v === password.value || 'Mật khẩu không khớp'
]

const handleRegister = async () => {
  if (!valid.value) return
  
  const success = await authStore.register(
    email.value, 
    password.value, 
    fullName.value,
    phoneNumber.value || undefined
  )
  if (success) {
    router.push({ name: 'Home' })
  }
}
</script>

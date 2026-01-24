<template>
  <v-container>
    <v-row justify="center" class="mt-10">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="pa-4">
          <v-card-title class="text-center">
            <v-icon icon="mdi-badminton" size="48" color="primary" class="mb-2" />
            <div class="text-h5">Đăng nhập</div>
            <div class="text-subtitle-2 text-grey">CLB Pickleball Vợt Thủ Phố Núi</div>
          </v-card-title>

          <v-card-text>
            <v-alert v-if="authStore.error" type="error" variant="tonal" class="mb-4" closable>
              {{ authStore.error }}
            </v-alert>

            <v-form ref="formRef" v-model="valid" @submit.prevent="handleLogin">
              <v-text-field
                v-model="email"
                label="Email"
                type="email"
                prepend-inner-icon="mdi-email"
                :rules="emailRules"
                required
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

              <v-btn
                type="submit"
                color="primary"
                block
                size="large"
                :loading="authStore.loading"
                :disabled="!valid"
                class="mt-4"
              >
                Đăng nhập
              </v-btn>
            </v-form>
          </v-card-text>

          <v-divider class="my-4" />

          <v-card-text class="text-center">
            <span class="text-grey">Chưa có tài khoản?</span>
            <router-link :to="{ name: 'Register' }" class="text-primary ml-1">
              Đăng ký ngay
            </router-link>
          </v-card-text>

          <!-- Demo Accounts -->
          <v-expansion-panels variant="accordion">
            <v-expansion-panel title="📋 Tài khoản demo">
              <v-expansion-panel-text>
                <v-list density="compact">
                  <v-list-item
                    v-for="acc in demoAccounts"
                    :key="acc.email"
                    @click="fillDemo(acc)"
                    class="cursor-pointer"
                  >
                    <v-list-item-title>{{ acc.email }}</v-list-item-title>
                    <v-list-item-subtitle>
                      <v-chip size="x-small" :color="acc.color">{{ acc.role }}</v-chip>
                    </v-list-item-subtitle>
                  </v-list-item>
                </v-list>
              </v-expansion-panel-text>
            </v-expansion-panel>
          </v-expansion-panels>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const formRef = ref()
const valid = ref(false)
const email = ref('')
const password = ref('')
const showPassword = ref(false)

const emailRules = [
  (v: string) => !!v || 'Email là bắt buộc',
  (v: string) => /.+@.+\..+/.test(v) || 'Email không hợp lệ'
]

const passwordRules = [
  (v: string) => !!v || 'Mật khẩu là bắt buộc',
  (v: string) => v.length >= 6 || 'Mật khẩu phải có ít nhất 6 ký tự'
]

const demoAccounts = [
  { email: 'admin@pcm.com', password: 'Admin@123', role: 'Admin', color: 'error' },
  { email: 'treasurer@pcm.com', password: 'Treasurer@123', role: 'Treasurer', color: 'warning' },
  { email: 'referee@pcm.com', password: 'Referee@123', role: 'Referee', color: 'info' },
  { email: 'member1@pcm.com', password: 'Member@123', role: 'Member', color: 'success' }
]

const fillDemo = (acc: typeof demoAccounts[0]) => {
  email.value = acc.email
  password.value = acc.password
}

const handleLogin = async () => {
  if (!valid.value) return
  
  const success = await authStore.login(email.value, password.value)
  if (success) {
    const redirect = (route.query.redirect as string) || '/'
    router.push(redirect)
  }
}
</script>

<style scoped>
.cursor-pointer {
  cursor: pointer;
}
</style>

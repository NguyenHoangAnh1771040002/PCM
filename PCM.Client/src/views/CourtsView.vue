<template>
  <div>
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <v-icon icon="mdi-tennis-ball" class="mr-2" />
        Quản lý Sân
        <v-spacer />
        <v-btn v-if="authStore.isAdmin" color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Thêm sân
        </v-btn>
      </v-card-title>
    </v-card>

    <v-row>
      <v-col v-for="court in courts" :key="court.id" cols="12" sm="6" md="4">
        <v-card :class="{ 'border-error': !court.isActive }">
          <v-card-title class="d-flex align-center">
            <v-icon :icon="court.isActive ? 'mdi-tennis-ball' : 'mdi-alert-circle'" 
                    :color="court.isActive ? 'primary' : 'error'" class="mr-2" />
            {{ court.name }}
            <v-spacer />
            <v-chip :color="court.isActive ? 'success' : 'error'" size="small">
              {{ court.isActive ? 'Hoạt động' : 'Bảo trì' }}
            </v-chip>
          </v-card-title>
          <v-card-text>
            <p>{{ court.description || 'Không có mô tả' }}</p>
            <p class="text-caption text-grey">Tạo ngày: {{ formatDate(court.createdDate) }}</p>
          </v-card-text>
          <v-card-actions v-if="authStore.isAdmin">
            <v-btn color="primary" variant="text" @click="openDialog(court)">
              <v-icon left>mdi-pencil</v-icon> Sửa
            </v-btn>
            <v-btn color="error" variant="text" @click="confirmDelete(court)">
              <v-icon left>mdi-delete</v-icon> Xóa
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-skeleton-loader v-if="loading" type="card@3" />

    <!-- Dialog -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>{{ editingCourt ? 'Sửa sân' : 'Thêm sân mới' }}</v-card-title>
        <v-card-text>
          <v-form ref="formRef" v-model="valid">
            <v-text-field
              v-model="form.name"
              label="Tên sân"
              :rules="[(v) => !!v || 'Tên sân là bắt buộc']"
              required
            />
            <v-textarea v-model="form.description" label="Mô tả" rows="3" />
            <v-switch v-model="form.isActive" label="Đang hoạt động" color="primary" />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="saveCourt">
            {{ editingCourt ? 'Cập nhật' : 'Tạo mới' }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Confirm Delete -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title>Xác nhận xóa</v-card-title>
        <v-card-text>Bạn có chắc muốn xóa "{{ deletingCourt?.name }}"?</v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="deleteDialog = false">Hủy</v-btn>
          <v-btn color="error" :loading="deleting" @click="deleteCourt">Xóa</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor">{{ snackbarText }}</v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { courtService } from '@/services'
import type { Court, CourtDto } from '@/types'

const authStore = useAuthStore()

const courts = ref<Court[]>([])
const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const saving = ref(false)
const deleting = ref(false)
const valid = ref(false)

const editingCourt = ref<Court | null>(null)
const deletingCourt = ref<Court | null>(null)

const form = ref<CourtDto>({ name: '', description: '', isActive: true })

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const formatDate = (dateStr: string) => {
  if (!dateStr) return 'N/A'
  const d = new Date(dateStr)
  return isNaN(d.getTime()) ? 'N/A' : d.toLocaleDateString('vi-VN')
}

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

const loadCourts = async () => {
  loading.value = true
  try {
    const res = await courtService.getAll()
    courts.value = res.data
  } catch (e) {
    showSnackbar('Không thể tải danh sách sân', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = (court?: Court) => {
  if (court) {
    editingCourt.value = court
    form.value = { name: court.name, description: court.description || '', isActive: court.isActive }
  } else {
    editingCourt.value = null
    form.value = { name: '', description: '', isActive: true }
  }
  dialog.value = true
}

const saveCourt = async () => {
  if (!valid.value) return
  saving.value = true
  try {
    if (editingCourt.value) {
      await courtService.update(editingCourt.value.id, form.value)
      showSnackbar('Cập nhật sân thành công')
    } else {
      await courtService.create(form.value)
      showSnackbar('Thêm sân thành công')
    }
    dialog.value = false
    loadCourts()
  } catch (e) {
    showSnackbar('Lỗi khi lưu', 'error')
  } finally {
    saving.value = false
  }
}

const confirmDelete = (court: Court) => {
  deletingCourt.value = court
  deleteDialog.value = true
}

const deleteCourt = async () => {
  if (!deletingCourt.value) return
  deleting.value = true
  try {
    await courtService.delete(deletingCourt.value.id)
    showSnackbar('Xóa sân thành công')
    deleteDialog.value = false
    loadCourts()
  } catch (e) {
    showSnackbar('Lỗi khi xóa', 'error')
  } finally {
    deleting.value = false
  }
}

onMounted(loadCourts)
</script>

<template>
  <div>
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <v-icon icon="mdi-account-group" class="mr-2" />
        Quản lý Hội viên
        <v-spacer />
        <v-btn v-if="authStore.isAdmin" color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Thêm hội viên
        </v-btn>
      </v-card-title>
    </v-card>

    <v-card>
      <v-card-text>
        <v-skeleton-loader v-if="loading" type="table" />
        <v-data-table
          v-else
          :headers="headers"
          :items="members"
          :search="search"
          :items-per-page="10"
        >
          <template v-slot:top>
            <v-text-field
              v-model="search"
              label="Tìm kiếm"
              prepend-inner-icon="mdi-magnify"
              clearable
              class="mx-4"
            />
          </template>
          <template v-slot:item.isActive="{ item }">
            <v-chip :color="item.isActive ? 'success' : 'error'" size="small">
              {{ item.isActive ? 'Hoạt động' : 'Ngừng' }}
            </v-chip>
          </template>
          <template v-slot:item.rankLevel="{ item }">
            <v-chip color="primary" size="small">{{ item.rankLevel.toFixed(1) }}</v-chip>
          </template>
          <template v-slot:item.joinDate="{ item }">
            {{ formatDate(item.joinDate) }}
          </template>
          <template v-slot:item.actions="{ item }">
            <v-btn 
              v-if="authStore.isAdmin" 
              icon 
              size="small" 
              variant="text" 
              @click="openDialog(item)"
            >
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn 
              v-if="authStore.isAdmin" 
              icon 
              size="small" 
              variant="text" 
              color="error" 
              @click="confirmDelete(item)"
            >
              <v-icon>mdi-delete</v-icon>
            </v-btn>
            <span v-if="!authStore.isAdmin" class="text-grey">—</span>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>

    <!-- Dialog Create/Edit -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>{{ editingMember ? 'Sửa hội viên' : 'Thêm hội viên' }}</v-card-title>
        <v-card-text>
          <v-form ref="formRef" v-model="valid">
            <v-text-field
              v-model="form.fullName"
              label="Họ tên"
              :rules="[(v) => !!v || 'Họ tên là bắt buộc']"
              required
            />
            <v-text-field v-model="form.email" label="Email" type="email" />
            <v-text-field v-model="form.phoneNumber" label="Số điện thoại" />
            <v-text-field
              v-model="form.dateOfBirth"
              label="Ngày sinh"
              type="date"
            />
            <v-text-field
              v-model.number="form.rankLevel"
              label="Điểm DUPR"
              type="number"
              step="0.1"
              min="0"
              max="7"
            />
            <v-switch v-model="form.isActive" label="Hoạt động" color="primary" />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="saveMember">
            {{ editingMember ? 'Cập nhật' : 'Tạo mới' }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Confirm Delete -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title>Xác nhận xóa</v-card-title>
        <v-card-text>
          Bạn có chắc muốn xóa hội viên "{{ deletingMember?.fullName }}"?
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="deleteDialog = false">Hủy</v-btn>
          <v-btn color="error" :loading="deleting" @click="deleteMember">Xóa</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor">{{ snackbarText }}</v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { memberService } from '@/services'
import type { Member, MemberDto } from '@/types'

const authStore = useAuthStore()

const members = ref<Member[]>([])
const loading = ref(false)
const search = ref('')
const dialog = ref(false)
const deleteDialog = ref(false)
const saving = ref(false)
const deleting = ref(false)
const valid = ref(false)

const editingMember = ref<Member | null>(null)
const deletingMember = ref<Member | null>(null)

const form = ref<MemberDto>({
  fullName: '',
  email: '',
  phoneNumber: '',
  dateOfBirth: '',
  rankLevel: 2.5,
  isActive: true
})

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const headers = [
  { title: 'Họ tên', key: 'fullName' },
  { title: 'Email', key: 'email' },
  { title: 'SĐT', key: 'phoneNumber' },
  { title: 'DUPR', key: 'rankLevel' },
  { title: 'Ngày tham gia', key: 'joinDate' },
  { title: 'Trạng thái', key: 'isActive' },
  { title: 'Hành động', key: 'actions', sortable: false }
]

const formatDate = (dateStr: string) => new Date(dateStr).toLocaleDateString('vi-VN')

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

const loadMembers = async () => {
  loading.value = true
  try {
    const res = await memberService.getAll()
    members.value = res.data
  } catch (e) {
    showSnackbar('Không thể tải danh sách', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = (member?: Member) => {
  if (member) {
    editingMember.value = member
    form.value = {
      fullName: member.fullName,
      email: member.email || '',
      phoneNumber: member.phoneNumber || '',
      dateOfBirth: member.dateOfBirth?.split('T')[0] || '',
      rankLevel: member.rankLevel,
      isActive: member.isActive
    }
  } else {
    editingMember.value = null
    form.value = { fullName: '', email: '', phoneNumber: '', dateOfBirth: '', rankLevel: 2.5, isActive: true }
  }
  dialog.value = true
}

const saveMember = async () => {
  if (!valid.value) return
  saving.value = true
  try {
    if (editingMember.value) {
      await memberService.update(editingMember.value.id, form.value)
      showSnackbar('Cập nhật thành công')
    } else {
      await memberService.create(form.value)
      showSnackbar('Thêm hội viên thành công')
    }
    dialog.value = false
    loadMembers()
  } catch (e) {
    showSnackbar('Lỗi khi lưu', 'error')
  } finally {
    saving.value = false
  }
}

const confirmDelete = (member: Member) => {
  deletingMember.value = member
  deleteDialog.value = true
}

const deleteMember = async () => {
  if (!deletingMember.value) return
  deleting.value = true
  try {
    await memberService.delete(deletingMember.value.id)
    showSnackbar('Xóa thành công')
    deleteDialog.value = false
    loadMembers()
  } catch (e) {
    showSnackbar('Lỗi khi xóa', 'error')
  } finally {
    deleting.value = false
  }
}

onMounted(loadMembers)
</script>

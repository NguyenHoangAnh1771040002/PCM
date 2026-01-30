<template>
  <div>
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <v-icon icon="mdi-calendar-clock" class="mr-2" />
        Đặt sân
        <v-spacer />
        <v-btn color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Đặt sân mới
        </v-btn>
      </v-card-title>
    </v-card>

    <!-- My Bookings -->
    <v-card class="mb-4">
      <v-card-title>Lịch đặt sân của tôi</v-card-title>
      <v-card-text>
        <v-skeleton-loader v-if="loading" type="table" />
        <v-data-table
          v-else
          :headers="headers"
          :items="myBookings"
          :items-per-page="5"
        >
          <template v-slot:item.court="{ item }">
            {{ item.court?.name || `Sân #${item.courtId}` }}
          </template>
          <template v-slot:item.time="{ item }">
            {{ formatDateTime(item.startTime) }} - {{ formatTime(item.endTime) }}
          </template>
          <template v-slot:item.status="{ item }">
            <v-chip :color="getStatusColor(item.status)" size="small">
              {{ getStatusText(item.status) }}
            </v-chip>
          </template>
          <template v-slot:item.actions="{ item }">
            <v-btn
              v-if="item.status === 'Pending'"
              icon
              size="small"
              variant="text"
              color="error"
              @click="cancelBooking(item)"
            >
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>

    <!-- All Bookings (Admin only) -->
    <v-card v-if="authStore.isAdmin">
      <v-card-title>Tất cả đặt sân (Admin)</v-card-title>
      <v-card-text>
        <v-data-table
          :headers="adminHeaders"
          :items="allBookings"
          :items-per-page="10"
        >
          <template v-slot:item.member="{ item }">
            {{ item.member?.fullName || 'N/A' }}
          </template>
          <template v-slot:item.court="{ item }">
            {{ item.court?.name || `Sân #${item.courtId}` }}
          </template>
          <template v-slot:item.time="{ item }">
            {{ formatDateTime(item.startTime) }} - {{ formatTime(item.endTime) }}
          </template>
          <template v-slot:item.status="{ item }">
            <v-select
              :model-value="item.status"
              :items="statusOptions"
              density="compact"
              variant="outlined"
              hide-details
              style="max-width: 150px"
              @update:model-value="updateStatus(item, $event)"
            />
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>

    <!-- Dialog Create Booking -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>Đặt sân mới</v-card-title>
        <v-card-text>
          <v-form ref="formRef" v-model="valid">
            <v-select
              v-model="form.courtId"
              :items="activeCourts"
              item-title="name"
              item-value="id"
              label="Chọn sân"
              :rules="[(v) => !!v || 'Vui lòng chọn sân']"
              required
            />
            <v-text-field
              v-model="form.startTime"
              label="Thời gian bắt đầu"
              type="datetime-local"
              :rules="[(v) => !!v || 'Vui lòng chọn thời gian']"
              required
            />
            <v-text-field
              v-model="form.endTime"
              label="Thời gian kết thúc"
              type="datetime-local"
              :rules="[(v) => !!v || 'Vui lòng chọn thời gian']"
              required
            />
            <v-textarea v-model="form.notes" label="Ghi chú" rows="2" />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="createBooking">
            Đặt sân
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor">{{ snackbarText }}</v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { bookingService, courtService } from '@/services'
import type { Booking, BookingDto, Court } from '@/types'
import { BookingStatus } from '@/types'

const authStore = useAuthStore()

const myBookings = ref<Booking[]>([])
const allBookings = ref<Booking[]>([])
const courts = ref<Court[]>([])
const loading = ref(false)
const dialog = ref(false)
const saving = ref(false)
const valid = ref(false)

const form = ref<BookingDto>({
  courtId: 0,
  startTime: '',
  endTime: '',
  notes: ''
})

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const headers = [
  { title: 'Sân', key: 'court' },
  { title: 'Thời gian', key: 'time' },
  { title: 'Ghi chú', key: 'notes' },
  { title: 'Trạng thái', key: 'status' },
  { title: '', key: 'actions', sortable: false }
]

const adminHeaders = [
  { title: 'Người đặt', key: 'member' },
  { title: 'Sân', key: 'court' },
  { title: 'Thời gian', key: 'time' },
  { title: 'Trạng thái', key: 'status', width: '170px' }
]

const statusOptions = [
  { title: 'Chờ duyệt', value: 'Pending' },
  { title: 'Đã duyệt', value: 'Confirmed' },
  { title: 'Đã hủy', value: 'Cancelled' },
  { title: 'Hoàn thành', value: 'Completed' }
]

const activeCourts = computed(() => courts.value.filter(c => c.isActive))

const getStatusColor = (status: BookingStatus) => {
  switch (status) {
    case 'Confirmed': return 'success'
    case 'Pending': return 'warning'
    case 'Cancelled': return 'error'
    case 'Completed': return 'info'
    default: return 'grey'
  }
}

const getStatusText = (status: BookingStatus) => {
  switch (status) {
    case 'Confirmed': return 'Đã duyệt'
    case 'Pending': return 'Chờ duyệt'
    case 'Cancelled': return 'Đã hủy'
    case 'Completed': return 'Hoàn thành'
    default: return status
  }
}

const formatDateTime = (dateStr: string) => {
  return new Date(dateStr).toLocaleString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const formatTime = (dateStr: string) => {
  return new Date(dateStr).toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })
}

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

const loadData = async () => {
  loading.value = true
  try {
    const [courtsRes, myRes] = await Promise.all([
      courtService.getAll(),
      bookingService.getMy()
    ])
    courts.value = courtsRes.data
    myBookings.value = myRes.data

    if (authStore.isAdmin) {
      const allRes = await bookingService.getAll()
      allBookings.value = allRes.data
    }
  } catch (e) {
    showSnackbar('Không thể tải dữ liệu', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = () => {
  form.value = { courtId: 0, startTime: '', endTime: '', notes: '' }
  dialog.value = true
}

const createBooking = async () => {
  if (!valid.value) return
  saving.value = true
  try {
    await bookingService.create(form.value)
    showSnackbar('Đặt sân thành công!')
    dialog.value = false
    loadData()
  } catch (e: any) {
    const errorMsg = e.response?.data?.message || e.response?.data?.title || 'Lỗi khi đặt sân'
    showSnackbar(errorMsg, 'error')
    console.error('Booking error:', e.response?.data)
  } finally {
    saving.value = false
  }
}

const cancelBooking = async (booking: Booking) => {
  try {
    await bookingService.delete(booking.id)
    showSnackbar('Đã hủy đặt sân')
    loadData()
  } catch (e: any) {
    const msg = e.response?.data?.message || 'Lỗi khi hủy'
    showSnackbar(msg, 'error')
  }
}

const updateStatus = async (booking: Booking, status: BookingStatus) => {
  try {
    await bookingService.updateStatus(booking.id, status)
    showSnackbar('Cập nhật trạng thái thành công')
    loadData()
  } catch (e) {
    showSnackbar('Lỗi khi cập nhật', 'error')
  }
}

onMounted(loadData)
</script>

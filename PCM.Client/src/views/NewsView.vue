<template>
  <div>
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <v-icon icon="mdi-newspaper" class="mr-2" />
        Tin tức CLB
        <v-spacer />
        <v-btn v-if="authStore.isAdmin" color="primary" @click="openDialog()">
          <v-icon left>mdi-plus</v-icon>
          Thêm tin
        </v-btn>
      </v-card-title>
    </v-card>

    <v-skeleton-loader v-if="loading" type="card@3" />

    <template v-else>
      <v-alert v-if="!newsList.length" type="info" variant="tonal">
        Chưa có tin tức nào.
      </v-alert>

      <v-card v-for="news in sortedNews" :key="news.id" class="mb-4">
        <v-card-title>
          <v-icon v-if="news.isPinned" icon="mdi-pin" color="primary" class="mr-2" />
          {{ news.title }}
        </v-card-title>
        <v-card-subtitle>
          📅 {{ formatDate(news.createdDate) }}
          <span v-if="news.modifiedDate" class="ml-2">(Sửa: {{ formatDate(news.modifiedDate) }})</span>
        </v-card-subtitle>
        <v-card-text>
          <div style="white-space: pre-line">{{ news.content }}</div>
        </v-card-text>
        <v-card-actions v-if="authStore.isAdmin">
          <v-btn color="primary" variant="text" @click="openDialog(news)">
            <v-icon left>mdi-pencil</v-icon> Sửa
          </v-btn>
          <v-btn color="error" variant="text" @click="confirmDelete(news)">
            <v-icon left>mdi-delete</v-icon> Xóa
          </v-btn>
        </v-card-actions>
      </v-card>
    </template>

    <!-- Dialog Create/Edit -->
    <v-dialog v-model="dialog" max-width="600">
      <v-card>
        <v-card-title>{{ editingNews ? 'Sửa tin tức' : 'Thêm tin tức mới' }}</v-card-title>
        <v-card-text>
          <v-form ref="formRef" v-model="valid">
            <v-text-field
              v-model="form.title"
              label="Tiêu đề"
              :rules="[(v) => !!v || 'Tiêu đề là bắt buộc']"
              required
            />
            <v-textarea
              v-model="form.content"
              label="Nội dung"
              rows="5"
              :rules="[(v) => !!v || 'Nội dung là bắt buộc']"
              required
            />
            <v-switch v-model="form.isPinned" label="Ghim tin" color="primary" />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="saveNews">
            {{ editingNews ? 'Cập nhật' : 'Tạo mới' }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Confirm Delete Dialog -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title>Xác nhận xóa</v-card-title>
        <v-card-text>
          Bạn có chắc muốn xóa tin tức "{{ deletingNews?.title }}"?
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="deleteDialog = false">Hủy</v-btn>
          <v-btn color="error" :loading="deleting" @click="deleteNews">Xóa</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar -->
    <v-snackbar v-model="snackbar" :color="snackbarColor" timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { newsService } from '@/services'
import type { News, NewsDto } from '@/types'

const authStore = useAuthStore()

const newsList = ref<News[]>([])
const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const saving = ref(false)
const deleting = ref(false)
const valid = ref(false)
const formRef = ref()

const editingNews = ref<News | null>(null)
const deletingNews = ref<News | null>(null)

const form = ref<NewsDto>({
  title: '',
  content: '',
  isPinned: false
})

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const sortedNews = computed(() => {
  return [...newsList.value].sort((a, b) => {
    // Pinned first
    if (a.isPinned && !b.isPinned) return -1
    if (!a.isPinned && b.isPinned) return 1
    // Then by date
    return new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
  })
})

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

const loadNews = async () => {
  loading.value = true
  try {
    const res = await newsService.getAll()
    newsList.value = res.data
  } catch (e) {
    showSnackbar('Không thể tải tin tức', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = (news?: News) => {
  if (news) {
    editingNews.value = news
    form.value = {
      title: news.title,
      content: news.content,
      isPinned: news.isPinned
    }
  } else {
    editingNews.value = null
    form.value = { title: '', content: '', isPinned: false }
  }
  dialog.value = true
}

const saveNews = async () => {
  if (!valid.value) return
  saving.value = true
  try {
    if (editingNews.value) {
      await newsService.update(editingNews.value.id, form.value)
      showSnackbar('Cập nhật tin tức thành công')
    } else {
      await newsService.create(form.value)
      showSnackbar('Thêm tin tức thành công')
    }
    dialog.value = false
    loadNews()
  } catch (e) {
    showSnackbar('Lỗi khi lưu tin tức', 'error')
  } finally {
    saving.value = false
  }
}

const confirmDelete = (news: News) => {
  deletingNews.value = news
  deleteDialog.value = true
}

const deleteNews = async () => {
  if (!deletingNews.value) return
  deleting.value = true
  try {
    await newsService.delete(deletingNews.value.id)
    showSnackbar('Xóa tin tức thành công')
    deleteDialog.value = false
    loadNews()
  } catch (e) {
    showSnackbar('Lỗi khi xóa tin tức', 'error')
  } finally {
    deleting.value = false
  }
}

onMounted(loadNews)
</script>

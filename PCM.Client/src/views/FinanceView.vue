<template>
  <div>
    <!-- Summary Cards -->
    <v-row class="mb-4">
      <v-col cols="12" sm="4">
        <v-card color="success" variant="elevated">
          <v-card-text class="text-center">
            <v-icon icon="mdi-arrow-up" size="40" />
            <div class="text-h4 font-weight-bold">{{ formatMoney(summary.totalIncome) }}</div>
            <div class="text-subtitle-1">Tổng Thu</div>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" sm="4">
        <v-card color="error" variant="elevated">
          <v-card-text class="text-center">
            <v-icon icon="mdi-arrow-down" size="40" />
            <div class="text-h4 font-weight-bold">{{ formatMoney(summary.totalExpense) }}</div>
            <div class="text-subtitle-1">Tổng Chi</div>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" sm="4">
        <v-card :color="summary.balance >= 0 ? 'primary' : 'warning'" variant="elevated">
          <v-card-text class="text-center">
            <v-icon icon="mdi-wallet" size="40" />
            <div class="text-h4 font-weight-bold">{{ formatMoney(summary.balance) }}</div>
            <div class="text-subtitle-1">Số dư Quỹ</div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Main Content -->
    <v-row>
      <!-- Transactions List -->
      <v-col cols="12" md="8">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-cash-multiple" class="mr-2" />
            Lịch sử Thu Chi
            <v-spacer />
            <v-btn color="primary" @click="openDialog()">
              <v-icon left>mdi-plus</v-icon>
              Thêm giao dịch
            </v-btn>
          </v-card-title>
          <v-card-text>
            <v-skeleton-loader v-if="loading" type="table" />
            <v-data-table
              v-else
              :headers="headers"
              :items="transactions"
              :items-per-page="10"
              :sort-by="[{ key: 'date', order: 'desc' }]"
            >
              <template v-slot:item.type="{ item }">
                <v-chip
                  :color="(item as any).categoryType === 'Income' ? 'success' : 'error'"
                  size="small"
                >
                  {{ (item as any).categoryType === 'Income' ? 'Thu' : 'Chi' }}
                </v-chip>
              </template>
              <template v-slot:item.amount="{ item }">
                <span :class="(item as any).categoryType === 'Income' ? 'text-success' : 'text-error'">
                  {{ (item as any).categoryType === 'Income' ? '+' : '-' }}{{ formatMoney(item.amount) }}
                </span>
              </template>
              <template v-slot:item.date="{ item }">
                {{ formatDate(item.date) }}
              </template>
              <template v-slot:item.category="{ item }">
                {{ (item as any).categoryName || 'N/A' }}
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Categories -->
      <v-col cols="12" md="4">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-tag-multiple" class="mr-2" />
            Danh mục
            <v-spacer />
            <v-btn v-if="authStore.isAdmin" icon size="small" @click="openCategoryDialog()">
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-text>
            <v-list density="compact">
              <v-list-subheader>Thu</v-list-subheader>
              <v-list-item
                v-for="cat in incomeCategories"
                :key="cat.id"
                :title="cat.name"
              >
                <template v-slot:prepend>
                  <v-icon color="success">mdi-plus-circle</v-icon>
                </template>
              </v-list-item>
              
              <v-divider />
              
              <v-list-subheader>Chi</v-list-subheader>
              <v-list-item
                v-for="cat in expenseCategories"
                :key="cat.id"
                :title="cat.name"
              >
                <template v-slot:prepend>
                  <v-icon color="error">mdi-minus-circle</v-icon>
                </template>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Dialog Create/Edit Transaction -->
    <v-dialog v-model="dialog" max-width="500">
      <v-card>
        <v-card-title>{{ editingTransaction ? 'Sửa giao dịch' : 'Thêm giao dịch' }}</v-card-title>
        <v-card-text>
          <v-form ref="formRef" v-model="valid">
            <v-select
              v-model="form.categoryId"
              :items="categories"
              item-title="name"
              item-value="id"
              label="Danh mục"
              :rules="[(v) => !!v || 'Vui lòng chọn danh mục']"
              required
            >
              <template v-slot:item="{ props, item }">
                <v-list-item
                  v-bind="props"
                  :prepend-icon="item.raw.type === 'Income' ? 'mdi-plus-circle' : 'mdi-minus-circle'"
                  :prepend-icon-color="item.raw.type === 'Income' ? 'success' : 'error'"
                />
              </template>
            </v-select>
            <v-text-field
              v-model.number="form.amount"
              label="Số tiền"
              type="number"
              prefix="₫"
              :rules="[(v) => v > 0 || 'Số tiền phải lớn hơn 0']"
              required
            />
            <v-text-field
              v-model="form.date"
              label="Ngày"
              type="date"
              :rules="[(v) => !!v || 'Vui lòng chọn ngày']"
              required
            />
            <v-textarea v-model="form.description" label="Mô tả" rows="2" />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="saveTransaction">
            {{ editingTransaction ? 'Cập nhật' : 'Tạo mới' }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Dialog Category -->
    <v-dialog v-model="categoryDialog" max-width="400">
      <v-card>
        <v-card-title>Thêm danh mục</v-card-title>
        <v-card-text>
          <v-form ref="catFormRef" v-model="catValid">
            <v-text-field
              v-model="catForm.name"
              label="Tên danh mục"
              :rules="[(v) => !!v || 'Tên là bắt buộc']"
            />
            <v-radio-group v-model="catForm.type" inline>
              <v-radio label="Thu" value="Income" color="success" />
              <v-radio label="Chi" value="Expense" color="error" />
            </v-radio-group>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="categoryDialog = false">Hủy</v-btn>
          <v-btn color="primary" :loading="savingCat" :disabled="!catValid" @click="saveCategory">
            Tạo mới
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Confirm Delete -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title>Xác nhận xóa</v-card-title>
        <v-card-text>Bạn có chắc muốn xóa giao dịch này?</v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="deleteDialog = false">Hủy</v-btn>
          <v-btn color="error" :loading="deleting" @click="deleteTransaction">Xóa</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor">{{ snackbarText }}</v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { transactionService, transactionCategoryService } from '@/services'
import type { Transaction, TransactionDto, TransactionCategory, TransactionCategoryDto, FinanceSummary } from '@/types'
import { TransactionType } from '@/types'

const authStore = useAuthStore()

const transactions = ref<Transaction[]>([])
const categories = ref<TransactionCategory[]>([])
const summary = ref<FinanceSummary>({ totalIncome: 0, totalExpense: 0, balance: 0, transactions: [] })
const loading = ref(false)
const dialog = ref(false)
const categoryDialog = ref(false)
const deleteDialog = ref(false)
const saving = ref(false)
const savingCat = ref(false)
const deleting = ref(false)
const valid = ref(false)
const catValid = ref(false)

const editingTransaction = ref<Transaction | null>(null)
const deletingTransaction = ref<Transaction | null>(null)

const form = ref<TransactionDto>({ date: '', amount: 0, description: '', categoryId: 0 })
const catForm = ref<TransactionCategoryDto>({ name: '', type: TransactionType.Income })

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const incomeCategories = computed(() => categories.value.filter(c => c.type === 'Income'))
const expenseCategories = computed(() => categories.value.filter(c => c.type === 'Expense'))

const headers = [
  { title: 'Loại', key: 'type', width: '80px' },
  { title: 'Số tiền', key: 'amount' },
  { title: 'Danh mục', key: 'category' },
  { title: 'Mô tả', key: 'description' },
  { title: 'Ngày', key: 'date' }
]

const formatMoney = (amount: number) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount)
}

const formatDate = (dateStr: string) => new Date(dateStr).toLocaleDateString('vi-VN')

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

const loadData = async () => {
  loading.value = true
  try {
    const [transRes, catRes, summaryRes] = await Promise.all([
      transactionService.getAll(),
      transactionCategoryService.getAll(),
      transactionService.getSummary()
    ])
    transactions.value = transRes.data
    categories.value = catRes.data
    summary.value = summaryRes.data
  } catch (e) {
    showSnackbar('Không thể tải dữ liệu', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = (transaction?: Transaction) => {
  if (transaction) {
    editingTransaction.value = transaction
    form.value = {
      date: transaction.date.split('T')[0] ?? '',
      amount: transaction.amount,
      description: transaction.description || '',
      categoryId: transaction.categoryId
    }
  } else {
    editingTransaction.value = null
    form.value = { date: new Date().toISOString().split('T')[0] ?? '', amount: 0, description: '', categoryId: 0 }
  }
  dialog.value = true
}

const saveTransaction = async () => {
  if (!valid.value) return
  saving.value = true
  try {
    if (editingTransaction.value) {
      await transactionService.update(editingTransaction.value.id, form.value)
      showSnackbar('Cập nhật thành công')
    } else {
      await transactionService.create(form.value)
      showSnackbar('Thêm giao dịch thành công')
    }
    dialog.value = false
    loadData()
  } catch (e) {
    showSnackbar('Lỗi khi lưu', 'error')
  } finally {
    saving.value = false
  }
}

const confirmDelete = (transaction: Transaction) => {
  deletingTransaction.value = transaction
  deleteDialog.value = true
}

const deleteTransaction = async () => {
  if (!deletingTransaction.value) return
  deleting.value = true
  try {
    await transactionService.delete(deletingTransaction.value.id)
    showSnackbar('Xóa thành công')
    deleteDialog.value = false
    loadData()
  } catch (e) {
    showSnackbar('Lỗi khi xóa', 'error')
  } finally {
    deleting.value = false
  }
}

const openCategoryDialog = () => {
  catForm.value = { name: '', type: TransactionType.Income }
  categoryDialog.value = true
}

const saveCategory = async () => {
  if (!catValid.value) return
  savingCat.value = true
  try {
    await transactionCategoryService.create(catForm.value)
    showSnackbar('Thêm danh mục thành công')
    categoryDialog.value = false
    loadData()
  } catch (e) {
    showSnackbar('Lỗi khi thêm danh mục', 'error')
  } finally {
    savingCat.value = false
  }
}

onMounted(loadData)
</script>

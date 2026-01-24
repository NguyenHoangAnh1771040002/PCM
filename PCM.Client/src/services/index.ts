import api from './api'
import type { 
  LoginRequest, 
  RegisterRequest, 
  AuthResponse, 
  Member, 
  MemberDto,
  News,
  NewsDto,
  Court,
  CourtDto,
  TransactionCategory,
  TransactionCategoryDto,
  Transaction,
  TransactionDto,
  FinanceSummary,
  Booking,
  BookingDto,
  Challenge,
  ChallengeDto,
  ParticipantDto,
  Match,
  MatchDto,
  MatchResultDto
} from '@/types'
import type { BookingStatus, ChallengeStatus, ParticipantTeam } from '@/types'

// ==================== AUTH ====================
export const authService = {
  login: (data: LoginRequest) => 
    api.post<AuthResponse>('/auth/login', data),
  
  register: (data: RegisterRequest) => 
    api.post<AuthResponse>('/auth/register', data),
  
  me: () => 
    api.get<AuthResponse>('/auth/me')
}

// ==================== MEMBERS ====================
export const memberService = {
  getAll: () => 
    api.get<Member[]>('/members'),
  
  getById: (id: number) => 
    api.get<Member>(`/members/${id}`),
  
  getRanking: () => 
    api.get<Member[]>('/members/top-ranking'),
  
  create: (data: MemberDto) => 
    api.post<Member>('/members', data),
  
  update: (id: number, data: MemberDto) => 
    api.put<void>(`/members/${id}`, data),
  
  delete: (id: number) => 
    api.delete<void>(`/members/${id}`)
}

// ==================== NEWS ====================
export const newsService = {
  getAll: () => 
    api.get<News[]>('/news'),
  
  getById: (id: number) => 
    api.get<News>(`/news/${id}`),
  
  create: (data: NewsDto) => 
    api.post<News>('/news', data),
  
  update: (id: number, data: NewsDto) => 
    api.put<void>(`/news/${id}`, data),
  
  delete: (id: number) => 
    api.delete<void>(`/news/${id}`)
}

// ==================== COURTS ====================
export const courtService = {
  getAll: () => 
    api.get<Court[]>('/courts'),
  
  getById: (id: number) => 
    api.get<Court>(`/courts/${id}`),
  
  create: (data: CourtDto) => 
    api.post<Court>('/courts', data),
  
  update: (id: number, data: CourtDto) => 
    api.put<void>(`/courts/${id}`, data),
  
  delete: (id: number) => 
    api.delete<void>(`/courts/${id}`)
}

// ==================== TRANSACTION CATEGORIES ====================
export const transactionCategoryService = {
  getAll: () => 
    api.get<TransactionCategory[]>('/transaction-categories'),
  
  getById: (id: number) => 
    api.get<TransactionCategory>(`/transaction-categories/${id}`),
  
  create: (data: TransactionCategoryDto) => 
    api.post<TransactionCategory>('/transaction-categories', data),
  
  update: (id: number, data: TransactionCategoryDto) => 
    api.put<void>(`/transaction-categories/${id}`, data),
  
  delete: (id: number) => 
    api.delete<void>(`/transaction-categories/${id}`)
}

// ==================== TRANSACTIONS ====================
export const transactionService = {
  getAll: () => 
    api.get<Transaction[]>('/transactions'),
  
  getById: (id: number) => 
    api.get<Transaction>(`/transactions/${id}`),
  
  getSummary: () => 
    api.get<FinanceSummary>('/transactions/summary'),
  
  create: (data: TransactionDto) => 
    api.post<Transaction>('/transactions', data),
  
  update: (id: number, data: TransactionDto) => 
    api.put<void>(`/transactions/${id}`, data),
  
  delete: (id: number) => 
    api.delete<void>(`/transactions/${id}`)
}

// ==================== BOOKINGS ====================
export const bookingService = {
  getAll: () => 
    api.get<Booking[]>('/bookings'),
  
  getMy: () => 
    api.get<Booking[]>('/bookings/my-bookings'),
  
  getById: (id: number) => 
    api.get<Booking>(`/bookings/${id}`),
  
  create: (data: BookingDto) => 
    api.post<Booking>('/bookings', data),
  
  updateStatus: (id: number, status: BookingStatus) => 
    api.put<void>(`/bookings/${id}/status`, { status }),
  
  delete: (id: number) => 
    api.delete<void>(`/bookings/${id}`)
}

// ==================== CHALLENGES ====================
export const challengeService = {
  getAll: () => 
    api.get<Challenge[]>('/challenges'),
  
  getById: (id: number) => 
    api.get<Challenge>(`/challenges/${id}`),
  
  create: (data: ChallengeDto) => 
    api.post<Challenge>('/challenges', data),
  
  update: (id: number, data: ChallengeDto) => 
    api.put<void>(`/challenges/${id}`, data),
  
  updateStatus: (id: number, status: ChallengeStatus) => 
    api.put<void>(`/challenges/${id}/status`, { status }),
  
  join: (id: number, data: ParticipantDto) => 
    api.post<void>(`/challenges/${id}/join`, data),
  
  assignTeam: (id: number, participantId: number, team: ParticipantTeam) => 
    api.put<void>(`/challenges/${id}/participants/${participantId}/team`, { team }),
  
  delete: (id: number) => 
    api.delete<void>(`/challenges/${id}`)
}

// ==================== MATCHES ====================
export const matchService = {
  getAll: () => 
    api.get<Match[]>('/matches'),
  
  getById: (id: number) => 
    api.get<Match>(`/matches/${id}`),
  
  create: (data: MatchDto) => 
    api.post<Match>('/matches', data),
  
  updateResult: (id: number, data: MatchResultDto) => 
    api.put<void>(`/matches/${id}/result`, data),
  
  delete: (id: number) => 
    api.delete<void>(`/matches/${id}`)
}

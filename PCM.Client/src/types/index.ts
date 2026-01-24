// Enums
export enum TransactionType {
  Income = 'Income',
  Expense = 'Expense'
}

export enum BookingStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  Cancelled = 'Cancelled',
  Completed = 'Completed'
}

export enum ChallengeType {
  Duel = 'Duel',
  MiniGame = 'MiniGame'
}

export enum GameMode {
  None = 'None',
  TeamBattle = 'TeamBattle',
  Tournament = 'Tournament'
}

export enum ChallengeStatus {
  Open = 'Open',
  Ongoing = 'Ongoing',
  Completed = 'Completed',
  Cancelled = 'Cancelled'
}

export enum MatchFormat {
  Singles = 'Singles',
  Doubles = 'Doubles'
}

export enum WinningSide {
  Team1 = 'Team1',
  Team2 = 'Team2',
  NotSet = 'NotSet'
}

export enum ParticipantTeam {
  TeamA = 'TeamA',
  TeamB = 'TeamB',
  None = 'None'
}

export enum ParticipantStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  Rejected = 'Rejected'
}

// Interfaces
export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  fullName: string
  phoneNumber?: string
}

export interface AuthResponse {
  token: string
  email: string
  fullName: string
  roles: string[]
  memberId?: number
  expiration: string
}

export interface UserInfo {
  email: string
  fullName: string
  roles: string[]
  memberId?: number
}

export interface Member {
  id: number
  fullName: string
  email?: string
  phoneNumber?: string
  dateOfBirth?: string
  joinDate: string
  rankLevel: number
  isActive: boolean
  totalMatches: number
  winMatches: number
  userId: string
  createdDate: string
}

export interface MemberDto {
  fullName: string
  email?: string
  phoneNumber?: string
  dateOfBirth?: string
  rankLevel?: number
  isActive?: boolean
}

export interface News {
  id: number
  title: string
  content: string
  isPinned: boolean
  createdDate: string
  modifiedDate?: string
}

export interface NewsDto {
  title: string
  content: string
  isPinned?: boolean
}

export interface Court {
  id: number
  name: string
  isActive: boolean
  description?: string
  createdDate: string
}

export interface CourtDto {
  name: string
  isActive?: boolean
  description?: string
}

export interface TransactionCategory {
  id: number
  name: string
  type: TransactionType
  createdDate: string
}

export interface TransactionCategoryDto {
  name: string
  type: TransactionType
}

export interface Transaction {
  id: number
  date: string
  amount: number
  description?: string
  categoryId: number
  category?: TransactionCategory
  createdById?: number
  createdBy?: Member
  createdDate: string
}

export interface TransactionDto {
  date: string
  amount: number
  description?: string
  categoryId: number
}

export interface FinanceSummary {
  totalIncome: number
  totalExpense: number
  balance: number
  transactions: Transaction[]
}

export interface Booking {
  id: number
  courtId: number
  court?: Court
  memberId: number
  member?: Member
  startTime: string
  endTime: string
  status: BookingStatus
  notes?: string
  createdDate: string
}

export interface BookingDto {
  courtId: number
  startTime: string
  endTime: string
  notes?: string
}

export interface Challenge {
  id: number
  title: string
  type: ChallengeType
  gameMode: GameMode
  status: ChallengeStatus
  config_TargetWins?: number
  currentScore_TeamA: number
  currentScore_TeamB: number
  entryFee: number
  prizePool: number
  description?: string
  createdById: number
  createdBy?: Member
  startDate?: string
  endDate?: string
  createdDate: string
  participants?: Participant[]
  matches?: Match[]
}

export interface ChallengeDto {
  title: string
  type: ChallengeType
  gameMode?: GameMode
  config_TargetWins?: number
  entryFee?: number
  prizePool?: number
  description?: string
  startDate?: string
  endDate?: string
}

export interface Participant {
  id: number
  challengeId: number
  memberId: number
  member?: Member
  team: ParticipantTeam
  entryFeePaid: boolean
  entryFeeAmount: number
  joinedDate: string
  status: ParticipantStatus
}

export interface ParticipantDto {
  challengeId: number
  team?: ParticipantTeam
  entryFeeAmount?: number
}

export interface Match {
  id: number
  date: string
  isRanked: boolean
  challengeId?: number
  challenge?: Challenge
  matchFormat: MatchFormat
  team1_Player1Id: number
  team1_Player1?: Member
  team1_Player2Id?: number
  team1_Player2?: Member
  team2_Player1Id: number
  team2_Player1?: Member
  team2_Player2Id?: number
  team2_Player2?: Member
  winningSide: WinningSide
  scores?: MatchScore[]
  createdDate: string
}

export interface MatchDto {
  date?: string
  isRanked?: boolean
  challengeId?: number
  matchFormat: MatchFormat
  team1_Player1Id: number
  team1_Player2Id?: number
  team2_Player1Id: number
  team2_Player2Id?: number
}

export interface MatchResultDto {
  winningSide: WinningSide
  scores?: MatchScoreDto[]
}

export interface MatchScore {
  id: number
  matchId: number
  setNumber: number
  team1Score: number
  team2Score: number
  isFinalSet: boolean
}

export interface MatchScoreDto {
  setNumber: number
  team1Score: number
  team2Score: number
  isFinalSet?: boolean
}

export interface ApiResponse<T> {
  success: boolean
  data?: T
  message?: string
  errors?: string[]
}

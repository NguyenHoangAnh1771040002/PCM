# 🏓 PCM - Pickleball Club Management

Hệ thống quản lý CLB Pickleball "Vợt Thủ Phố Núi"

## 📋 Mô tả dự án

Dự án fullstack quản lý hoạt động của câu lạc bộ Pickleball, bao gồm:
- Quản lý thành viên và xếp hạng DUPR
- Đặt sân và quản lý lịch sân
- Tổ chức thách đấu và trận đấu
- Quản lý tài chính thu chi
- Quản lý tin tức và thông báo

## 🛠️ Công nghệ sử dụng

### Backend (PCM.API)
- **Framework:** ASP.NET Core (.NET 10)
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Authentication:** JWT Bearer Token
- **Authorization:** Role-based (Admin, Treasurer, Referee, Member)

### Frontend (PCM.Client)
- **Framework:** Vue.js 3 (Composition API)
- **Language:** TypeScript
- **UI Library:** Vuetify 3
- **State Management:** Pinia
- **HTTP Client:** Axios
- **Router:** Vue Router 4

## 📁 Cấu trúc dự án

```
PCM/
├── PCM.slnx                   # Solution file
├── README.md                  # File này
├── start.bat                  # Batch file to start the project
├── PCM.API/                   # Backend API
│   ├── Controllers/           # API endpoints
│   ├── Models/                # Entity models
│   ├── DTOs/                  # Data transfer objects
│   ├── Data/                  # DbContext & seed data
│   ├── Helpers/               # Utility classes
│   ├── Migrations/            # Database migrations
│   ├── Properties/            # Launch settings
│   ├── Services/              # Business logic services
│   ├── appsettings.json       # Configuration file
│   ├── Program.cs             # Entry point
│   └── PCM.API.csproj         # Project file
│
├── PCM.Client/                # Frontend Vue.js
│   ├── index.html             # HTML entry point
│   ├── package.json           # Node.js dependencies
│   ├── vite.config.ts         # Vite configuration
│   ├── tsconfig.json          # TypeScript configuration
│   ├── src/                   # Source code
│   │   ├── assets/            # Static assets
│   │   ├── components/        # Vue components
│   │   ├── layouts/           # Layout components
│   │   ├── plugins/           # Vuetify, Router config
│   │   ├── services/          # API service layer
│   │   ├── stores/            # Pinia stores
│   │   ├── types/             # TypeScript interfaces
│   │   └── views/             # Page components
│   └── public/                # Public assets
```

## 🚀 Hướng dẫn cài đặt và chạy

### Yêu cầu hệ thống
- .NET SDK 10.0 trở lên
- Node.js 18+ & npm
- SQL Server (LocalDB hoặc SQL Server Express)

### Cài đặt (chỉ lần đầu)

#### Bước 1: Cài đặt backend 

```bash
# Cài đặt backend
cd PCM.API
dotnet restore
dotnet ef database update
```

#### Bước 2: Cài đặt frontend

```bash
# Cài đặt frontend
cd PCM.Client
npm install
```

### Chạy

#### Chạy backend

```bash
# Chạy backend
cd PCM.API
dotnet run
```

API sẽ chạy tại: `http://localhost:5176`

#### Chạy frontend

```bash
# Chạy frontend
cd PCM.Client
npm run dev
```

Frontend sẽ chạy tại: `http://localhost:5173`

### Chạy nhanh bằng file batch

```bash
# Double-click file start.bat hoặc chạy lệnh:
start.bat
```

File `start.bat` sẽ tự động:
- ✅ Khởi động Backend API
- ✅ Khởi động Frontend
- ✅ Mở trình duyệt tại http://localhost:5173

## 👥 Tài khoản mẫu

| Email | Password | Vai trò |
|-------|----------|---------|
| admin@pcm.com | Admin@123 | Admin |
| treasurer@pcm.com | Treasurer@123 | Treasurer |
| referee@pcm.com | Referee@123 | Referee |
| member1@pcm.com | Member@123 | Member |
| member2@pcm.com | Member@123 | Member |
| ... đến member12@pcm.com | Member@123 | Member |

## 📚 Database Schema

| Bảng | Mô tả |
|------|-------|
| 002_Users | Tài khoản người dùng (ASP.NET Identity) |
| 002_Members | Thông tin thành viên CLB |
| 002_News | Tin tức, thông báo |
| 002_Courts | Danh sách sân |
| 002_Bookings | Đặt sân |
| 002_Challenges | Thách đấu |
| 002_ChallengeParticipants | Thành viên tham gia thách đấu |
| 002_Matches | Trận đấu |
| 002_MatchParticipants | Người chơi trong trận |
| 002_TransactionCategories | Danh mục thu chi |
| 002_Transactions | Giao dịch tài chính |

## 🔐 Phân quyền

| Chức năng | Admin | Treasurer | Referee | Member |
|-----------|:-----:|:---------:|:-------:|:------:|
| Quản lý thành viên | ✅ | ❌ | ❌ | ❌ |
| Quản lý tin tức | ✅ | ❌ | ❌ | ❌ |
| Quản lý sân | ✅ | ❌ | ❌ | ❌ |
| Quản lý tài chính | ✅ | ✅ | ❌ | ❌ |
| Nhập kết quả trận đấu | ✅ | ❌ | ✅ | ❌ |
| Đặt sân | ✅ | ✅ | ✅ | ✅ |
| Tham gia thách đấu | ✅ | ✅ | ✅ | ✅ |
| Xem bảng xếp hạng | ✅ | ✅ | ✅ | ✅ |

## 📡 API Endpoints

### Authentication
- `POST /api/auth/login` - Đăng nhập
- `POST /api/auth/register` - Đăng ký
- `GET /api/auth/me` - Thông tin user hiện tại

### Members
- `GET /api/members` - Danh sách thành viên
- `GET /api/members/{id}` - Chi tiết thành viên
- `GET /api/members/top-ranking` - Top xếp hạng DUPR
- `PUT /api/members/{id}` - Cập nhật thành viên (Admin)

### News
- `GET /api/news` - Danh sách tin tức
- `POST /api/news` - Tạo tin tức (Admin)
- `PUT /api/news/{id}` - Sửa tin tức (Admin)
- `DELETE /api/news/{id}` - Xóa tin tức (Admin)

### Courts
- `GET /api/courts` - Danh sách sân
- `POST /api/courts` - Tạo sân (Admin)
- `PUT /api/courts/{id}` - Sửa sân (Admin)
- `DELETE /api/courts/{id}` - Xóa sân (Admin)

### Bookings
- `GET /api/bookings` - Danh sách đặt sân
- `GET /api/bookings/available-slots` - Slot trống
- `GET /api/bookings/my-bookings` - Booking của tôi
- `POST /api/bookings` - Đặt sân
- `PUT /api/bookings/{id}/cancel` - Hủy booking

### Challenges
- `GET /api/challenges` - Danh sách thách đấu
- `POST /api/challenges` - Tạo thách đấu
- `POST /api/challenges/{id}/join` - Tham gia thách đấu
- `POST /api/challenges/{id}/auto-divide-teams` - Tự động chia đội
- `PUT /api/challenges/{id}/close` - Đóng thách đấu

### Matches
- `GET /api/matches` - Danh sách trận đấu
- `POST /api/matches` - Tạo trận đấu
- `PUT /api/matches/{id}` - Cập nhật kết quả (Referee/Admin)

### Transactions
- `GET /api/transactions` - Danh sách giao dịch
- `GET /api/transactions/summary` - Thống kê thu chi
- `POST /api/transactions` - Tạo giao dịch (Admin/Treasurer)

### Transaction Categories
- `GET /api/transactioncategories` - Danh mục thu chi

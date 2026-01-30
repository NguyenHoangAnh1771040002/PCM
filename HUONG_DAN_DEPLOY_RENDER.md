# 🚀 Hướng Dẫn Deploy PCM lên Render.com

## 📋 Tổng Quan

Render.com cung cấp hosting miễn phí cho:
- **Static Sites** (Frontend Vue.js) - Hoàn toàn miễn phí
- **Web Services** (Backend .NET) - 750 giờ miễn phí/tháng
- **PostgreSQL** - Miễn phí 90 ngày (sau đó $7/tháng)

> ⚠️ **Lưu ý**: Render không hỗ trợ SQL Server, nên cần chuyển sang PostgreSQL

---

## 🔧 Bước 0: Chuẩn Bị

### 0.1 Đẩy code lên GitHub

```bash
# Tạo repository mới trên GitHub, sau đó:
cd d:\Coding\FullStack\ThucHanh\KiemTra2\DuAn\PCM

# Nếu chưa có git
git init
git add .
git commit -m "Initial commit - PCM Fullstack"

# Thêm remote và push
git remote add origin https://github.com/YOUR_USERNAME/pcm-app.git
git branch -M main
git push -u origin main
```

### 0.2 Tạo tài khoản Render.com

1. Truy cập https://render.com
2. Đăng ký bằng **GitHub** (khuyến nghị để dễ connect repo)

---

## 📦 Bước 1: Tạo PostgreSQL Database

### 1.1 Tạo Database

1. Vào **Dashboard** → **New +** → **PostgreSQL**
2. Điền thông tin:
   - **Name**: `pcm-database`
   - **Database**: `pcm_db`
   - **User**: `pcm_user`
   - **Region**: `Singapore` (gần VN nhất)
   - **Plan**: `Free`
3. Click **Create Database**

### 1.2 Lưu Connection String

Sau khi tạo xong, copy **Internal Database URL** (dạng):
```
postgres://pcm_user:PASSWORD@dpg-xxxxx-a.singapore-postgres.render.com/pcm_db
```

---

## 🔙 Bước 2: Deploy Backend (ASP.NET Core)

### 2.1 Cập nhật code để hỗ trợ PostgreSQL

Thêm package PostgreSQL vào `PCM.API.csproj`:

```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.3" />
```

Cập nhật `Program.cs` để detect PostgreSQL:

```csharp
// Thay đổi phần AddDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString?.Contains("postgres") == true)
{
    // PostgreSQL cho production (Render)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    // SQL Server cho development
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}
```

### 2.2 Tạo Dockerfile cho Render

Tạo file `PCM.API/Dockerfile.render`:

```dockerfile
# Dockerfile cho Render.com
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PCM.API.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 10000

ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PCM.API.dll"]
```

> ⚠️ Render yêu cầu port **10000**

### 2.3 Tạo render.yaml (Blueprint)

Tạo file `render.yaml` ở thư mục gốc PCM:

```yaml
services:
  # Backend API
  - type: web
    name: pcm-api
    runtime: docker
    dockerfilePath: ./PCM.API/Dockerfile.render
    dockerContext: ./PCM.API
    region: singapore
    plan: free
    healthCheckPath: /health
    envVars:
      - key: ConnectionStrings__DefaultConnection
        fromDatabase:
          name: pcm-database
          property: connectionString
      - key: Jwt__Key
        generateValue: true
      - key: Jwt__Issuer
        value: PCMApp
      - key: Jwt__Audience
        value: PCMUsers
      - key: Jwt__ExpirationHours
        value: 24

  # Frontend
  - type: web
    name: pcm-web
    runtime: static
    buildCommand: cd PCM.Client && npm ci && npm run build
    staticPublishPath: PCM.Client/dist
    region: singapore
    headers:
      - path: /*
        name: Cache-Control
        value: public, max-age=31536000
    routes:
      - type: rewrite
        source: /*
        destination: /index.html

databases:
  - name: pcm-database
    databaseName: pcm_db
    user: pcm_user
    region: singapore
    plan: free
```

### 2.4 Deploy Backend trên Render

**Cách 1: Dùng Blueprint (Khuyến nghị)**
1. Vào **Dashboard** → **New +** → **Blueprint**
2. Connect GitHub repo
3. Render sẽ tự động đọc `render.yaml` và tạo services

**Cách 2: Thủ công**
1. **Dashboard** → **New +** → **Web Service**
2. Connect GitHub repo `pcm-app`
3. Cấu hình:
   - **Name**: `pcm-api`
   - **Region**: `Singapore`
   - **Runtime**: `Docker`
   - **Dockerfile Path**: `./PCM.API/Dockerfile.render`
   - **Docker Context**: `./PCM.API`
   - **Plan**: `Free`
4. **Environment Variables**:
   - `ConnectionStrings__DefaultConnection`: (paste PostgreSQL URL)
   - `Jwt__Key`: `YourSuperSecretKeyMin32CharactersLong!`
   - `Jwt__Issuer`: `PCMApp`
   - `Jwt__Audience`: `PCMUsers`
5. Click **Create Web Service**

---

## 🎨 Bước 3: Deploy Frontend (Vue.js)

### 3.1 Cập nhật API URL

Tạo file `PCM.Client/.env.production`:

```env
VITE_API_URL=https://pcm-api.onrender.com/api
```

### 3.2 Deploy Frontend

1. **Dashboard** → **New +** → **Static Site**
2. Connect GitHub repo
3. Cấu hình:
   - **Name**: `pcm-web`
   - **Branch**: `main`
   - **Root Directory**: `PCM.Client`
   - **Build Command**: `npm ci && npm run build`
   - **Publish Directory**: `dist`
4. Click **Create Static Site**

---

## 🌐 Bước 4: Kết Nối Domain Namecheap

### 4.1 Lấy URL từ Render

Sau khi deploy xong, Render sẽ cung cấp URLs:
- Frontend: `https://pcm-web.onrender.com`
- Backend: `https://pcm-api.onrender.com`

### 4.2 Cấu hình Custom Domain trên Render

1. Vào **pcm-web** service → **Settings** → **Custom Domains**
2. Click **Add Custom Domain**
3. Nhập domain của bạn: `nguyenhoanganh.me`
4. Render sẽ cho bạn một **CNAME target** (ví dụ: `pcm-web.onrender.com`)

### 4.3 Cấu hình DNS trên Namecheap

Xóa các record cũ và thêm:

| Type | Host | Value | TTL |
|------|------|-------|-----|
| CNAME | `@` | `pcm-web.onrender.com` | Automatic |
| CNAME | `www` | `pcm-web.onrender.com` | Automatic |
| CNAME | `api` | `pcm-api.onrender.com` | Automatic |

> ⚠️ Một số registrar không hỗ trợ CNAME cho `@`, trong trường hợp đó dùng:
> - **A Record** cho `@` → IP từ Render
> - **CNAME** cho `www` và `api`

### 4.4 Cập nhật Frontend API URL

Sau khi có custom domain, cập nhật `PCM.Client/.env.production`:

```env
VITE_API_URL=https://api.nguyenhoanganh.me/api
```

---

## ✅ Bước 5: Kiểm Tra

### 5.1 Kiểm tra Backend
```bash
curl https://pcm-api.onrender.com/health
# Hoặc: https://api.nguyenhoanganh.me/health
```

### 5.2 Kiểm tra Frontend
Truy cập https://pcm-web.onrender.com hoặc https://nguyenhoanganh.me

### 5.3 Kiểm tra Database
Trong Render Dashboard → Database → **Logs** để xem connection

---

## 🔄 Auto Deploy

Mỗi khi bạn push code lên GitHub, Render sẽ tự động:
1. Detect changes
2. Build lại
3. Deploy version mới

---

## ⚠️ Lưu Ý Quan Trọng

### Free Tier Limitations
- **Spin Down**: Service sẽ tắt sau 15 phút không có request (khởi động lại mất ~30s)
- **Database**: Miễn phí 90 ngày, sau đó cần upgrade
- **Bandwidth**: 100GB/tháng

### Tips
- Dùng health check endpoint (`/health`) để keep alive
- Cân nhắc upgrade lên Paid tier ($7/tháng) cho production thực sự

---

## 📞 Hỗ Trợ

Nếu gặp vấn đề:
1. Xem **Logs** trong Render Dashboard
2. Kiểm tra **Events** tab
3. Đọc [Render Docs](https://render.com/docs)

---

## 📁 Cấu Trúc Files Cần Tạo/Sửa

```
PCM/
├── render.yaml                          # Blueprint config
├── PCM.API/
│   ├── Dockerfile.render               # Dockerfile cho Render
│   └── (cập nhật Program.cs)
├── PCM.Client/
│   └── .env.production                 # API URL production
└── (các file khác giữ nguyên)
```

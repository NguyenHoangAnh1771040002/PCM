@echo off
echo ========================================
echo  PCM - Pickleball Club Management
echo  Starting all services...
echo ========================================

:: Check if Docker is running
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] Docker is not running. Please start Docker Desktop first.
    pause
    exit /b 1
)

echo.
echo [1/3] Building and starting containers...
docker-compose up --build -d

echo.
echo [2/3] Waiting for services to be healthy...
timeout /t 30 /nobreak >nul

echo.
echo [3/3] Checking container status...
docker-compose ps

echo.
echo ========================================
echo  Services are starting up!
echo ========================================
echo.
echo  Database:  SQL Server 2022 (port 1433)
echo  Backend:   http://localhost:5176
echo  Frontend:  http://localhost:8080
echo  Swagger:   http://localhost:5176/swagger
echo.
echo  Default Admin: admin@pcm.com / Admin@123
echo ========================================
echo.
echo Press any key to open the application...
pause >nul
start http://localhost:8080

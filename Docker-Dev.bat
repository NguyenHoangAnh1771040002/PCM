@echo off
echo ========================================
echo  PCM - Docker Development Mode
echo  Starting Database only...
echo ========================================

:: Check if Docker is running
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] Docker is not running. Please start Docker Desktop first.
    pause
    exit /b 1
)

echo.
echo Starting SQL Server 2022 container...
docker-compose -f docker-compose.dev.yml up -d

echo.
echo Waiting for database to be ready...
timeout /t 30 /nobreak >nul

echo.
echo ========================================
echo  Database is ready!
echo ========================================
echo.
echo  SQL Server: localhost,1433
echo  Username:   sa
echo  Password:   PCM@2026!
echo  Database:   PCM_DB
echo.
echo  Connection String for appsettings.json:
echo  Server=localhost;Database=PCM_DB;User Id=sa;Password=PCM@2026!;TrustServerCertificate=True
echo.
echo  Now run start.bat to start Backend and Frontend locally!
echo ========================================
pause

@echo off
echo ========================================
echo  PCM - Pickleball Club Management
echo  Stopping all services...
echo ========================================

docker-compose down

echo.
echo All containers stopped successfully!
echo.
pause

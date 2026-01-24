@echo off
title PCM - Pickleball Club Management
color 0A

echo ========================================
echo    PCM - Pickleball Club Management
echo    Starting Backend and Frontend...
echo ========================================
echo.

:: Start Backend API in new window
echo [1/2] Starting Backend API...
start "PCM API - Backend" cmd /k "cd /d %~dp0PCM.API && dotnet run"

:: Wait for API to start
echo Waiting for API to initialize...
timeout /t 5 /nobreak >nul

:: Start Frontend in new window
echo [2/2] Starting Frontend...
start "PCM Client - Frontend" cmd /k "cd /d %~dp0PCM.Client && npm run dev"

echo.
echo ========================================
echo    Both services are starting!
echo ========================================
echo.
echo    Backend API:  http://localhost:5176
echo    Frontend:     http://localhost:5173
echo.
echo    Press any key to open the app...
echo ========================================
pause >nul

:: Open browser
start http://localhost:5173

echo.
echo To stop the servers, close the terminal windows.
echo.

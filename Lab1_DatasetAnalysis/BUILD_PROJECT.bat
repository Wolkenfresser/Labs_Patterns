@echo off
chcp 65001 >nul
cd /d "%~dp0"
where dotnet >nul 2>&1 || (echo .NET 8 SDK не найден.& pause & exit /b 1)
dotnet build "%~dp0Lab1_DatasetAnalysis.sln"
if errorlevel 1 pause

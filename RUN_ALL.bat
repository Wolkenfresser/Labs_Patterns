@echo off
chcp 65001 >nul
cd /d "%~dp0"
:menu
cls
echo ==========================================================
echo       DATALENS PATTERNS — ВАРИАНТ 13
echo ==========================================================
echo 1. Лабораторная №1 — Singleton + Builder
echo 2. Лабораторная №2 — Abstract Factory + Prototype
echo 3. Лабораторная №3 — Adapter + Bridge
echo 4. Лабораторная №4 — Composite + Decorator
echo 5. Лабораторная №5 — Facade + Flyweight
echo B. Собрать все проекты
echo Q. Выход
echo.
set "choice="
set /p choice=Выберите действие: 
if /i "%choice%"=="1" (
  call "Lab1_DatasetAnalysis\START_PROJECT.bat"
  cd /d "%~dp0"
  goto menu
)
if /i "%choice%"=="2" (
  call "Lab2_DatasetAnalysis\START_PROJECT.bat"
  cd /d "%~dp0"
  goto menu
)
if /i "%choice%"=="3" (
  call "Lab3_DatasetAnalysis\START_PROJECT.bat"
  cd /d "%~dp0"
  goto menu
)
if /i "%choice%"=="4" (
  call "Lab4_DatasetAnalysis\START_PROJECT.bat"
  cd /d "%~dp0"
  goto menu
)
if /i "%choice%"=="5" (
  call "Lab5_DatasetAnalysis\START_PROJECT.bat"
  cd /d "%~dp0"
  goto menu
)
if /i "%choice%"=="B" (
  dotnet build "DataLensPatterns.sln"
  pause
  goto menu
)
if /i "%choice%"=="Q" exit /b 0
echo Неверный выбор.
pause
goto menu

@echo off
chcp 65001 >nul
cd /d "%~dp0"
echo Lab4_DatasetAnalysis — вариант 13
where dotnet >nul 2>&1
if errorlevel 1 (
  echo .NET SDK не найден. Установите .NET 8 SDK.
  pause
  exit /b 1
)
dotnet --version
dotnet run --project "%~dp0SRC\Lab4_CompositeDecorator.csproj"
set "run_result=%errorlevel%"
echo.
if not "%run_result%"=="0" echo Запуск завершился с ошибкой. Проверьте вывод выше.
pause
exit /b %run_result%

@echo off
setlocal

cd /d "%~dp0"

echo Starting PostgreSQL with Docker...
docker compose up -d
if errorlevel 1 (
  echo.
  echo Failed to start Docker services.
  echo Make sure Docker Desktop is running, then try again.
  pause
  exit /b 1
)

echo.
echo Starting ASP.NET app on http://localhost:5078 ...
powershell -NoProfile -ExecutionPolicy Bypass -Command "$env:ASPNETCORE_ENVIRONMENT='Development'; dotnet run --no-build --no-launch-profile --urls http://localhost:5078"

endlocal

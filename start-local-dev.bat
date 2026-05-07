@echo off
setlocal

cd /d "%~dp0"

powershell -NoProfile -ExecutionPolicy Bypass -Command "$env:ASPNETCORE_ENVIRONMENT='Development'; dotnet run --no-build --no-launch-profile --urls http://localhost:5078"

endlocal

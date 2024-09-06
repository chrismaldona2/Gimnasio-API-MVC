@echo off
cd /d "%~dp0Data"
dotnet ef database update
pause
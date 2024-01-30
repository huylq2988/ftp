@echo off
cd..
echo Clearing FTP.App\bin...
rd FTP.App\bin /s /q

echo Clearing FTP.App\obj...
rd FTP.App\obj /s /q

echo.

echo Clearing FTP.Database\FTP.PMIS\bin...
rd FTP.Database\FTP.PMIS\bin /s /q

echo Clearing FTP.Database\FTP.PMIS\obj...
rd FTP.Database\FTP.PMIS\obj /s /q

echo.

echo Delete successfully. Press any key to exit
title Delete successfully. Press any key to exit
pause>nul
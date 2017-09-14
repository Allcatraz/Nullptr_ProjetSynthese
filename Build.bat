@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

echo =============================
echo Building Unity project
echo =============================

echo == Removing old builds ==
IF EXIST Build (
	rmdir /S /Q Build
)

CALL GenerateCode.bat

echo == Building a Windows build with Unity  ==
START /W unity -quit -batchmode -projectPath "%cd%" -buildWindows64Player Build/ProjetSynthese/ProjetSynthese.exe
IF %ERRORLEVEL% NEQ 0 (
	echo == Windows Build failled ==
	EXIT /B 1
)

echo == Building a Windows installer for the Windows build  ==
BuildTools\InnoSetup\ISCC.exe "BuildTools\CreateSetup.iss" >NUL
IF %ERRORLEVEL% NEQ 0 (
	echo == Windows installer build failled ==
	EXIT /B 1
)
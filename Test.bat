@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

echo =============================
echo Testing Unity project
echo =============================

IF NOT EXIST TestReports (
	mkdir TestReports
)

CALL GenerateCode.bat

SET RETURNCODE=0

echo == Building projet for Edit mode tests ==
START /W unity -quit -batchmode -runTests -projectPath "%cd%" -testResults "%cd%/TestReports/EditMode.xml" -testPlatform editmode
SET /A RETURNCODE=!RETURNCODE! + !ERRORLEVEL!

echo == Running Edit mode tests ==
START /W unity  -batchmode -runTests -projectPath "%cd%" -testResults "%cd%/TestReports/EditMode.xml" -testPlatform editmode
SET /A RETURNCODE=!RETURNCODE! + !ERRORLEVEL!

IF %ERRORLEVEL% NEQ 0 (
	echo == Edit mode tests failled ==
)

echo == Building projet for Play mode tests ==
START /W unity -quit -batchmode -runTests -projectPath "%cd%" -testResults "%cd%/TestReports/PlayMode.xml" -testPlatform playmode
SET /A RETURNCODE=!RETURNCODE! + !ERRORLEVEL!

echo == Running Play mode tests ==
START /W unity -batchmode -runTests -projectPath "%cd%" -testResults "%cd%/TestReports/PlayMode.xml" -testPlatform playmode
SET /A RETURNCODE=!RETURNCODE! + !ERRORLEVEL!

IF %ERRORLEVEL% NEQ 0 (
	echo == Play mode tests failled ==
)

IF !RETURNCODE! NEQ 0 (
	EXIT /B 1 
)
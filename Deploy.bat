@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

echo =============================
echo Deploying Unity project
echo =============================


IF EXIST Build (
	Build\Setup\ProjetSyntheseSetup.exe /SP- /VERYSILENT /CLOSEAPPLICATIONS
	IF %ERRORLEVEL% NEQ 0 (
		EXIT /B 1
	)
) else (
	echo Nothing to deploy found
	EXIT /B 1
)
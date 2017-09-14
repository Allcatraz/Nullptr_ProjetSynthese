@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

echo =============================
echo Compute number of lines
echo =============================

set /a totalNumLines = 0
for /r %%f in (*.cs ) do (
	for /f %%C in ('Find /V /C "" ^< %%f') do (
		set Count=%%C
	)
	set /a totalNumLines+=!Count!
)
echo == Total number of code lines: %totalNumLines% ==
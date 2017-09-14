@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

echo =============================
echo Generating Unity Test Report
echo =============================


SET RETURNCODE=0

echo == Building Edit mode and Play mode test report ==
mkdir public\testreport >NUL 2> nul
copy /Y BuildTools\ReportUnit\Static public\testreport >NUL
START /D BuildTools\ReportUnit\Bin BuildTools\ReportUnit\Bin\ReportUnit "../../../TestReports" "../../../public/testreport" >NUL
SET /A RETURNCODE=!RETURNCODE! + !ERRORLEVEL!
IF %ERRORLEVEL% NEQ 0 (
	echo == Test report build failled ==
)

echo == Building Code Documentation ==
BuildTools\Doxygen\doxygen.exe BuildTools\Doxyfile >NUL 2> nul
SET /A RETURNCODE=!RETURNCODE! + !ERRORLEVEL!
IF %ERRORLEVEL% NEQ 0 (
	echo == Code Documentation build failled ==
)

IF !RETURNCODE! NEQ 0 (
	EXIT /B 1 
)
@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

echo == Generating C# code ==

BuildTools\Harmony\HarmonyCodeGenerator.exe "%cd%" "%cd%\Assets\Generated"

IF %ERRORLEVEL% NEQ 0 (
	echo == Code Generation failled ==
	EXIT /B 1
)
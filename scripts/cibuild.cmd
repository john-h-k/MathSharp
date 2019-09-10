@ECHO OFF
powershell.exe -NoLogo -NoProfile -ExecutionPolicy ByPass -Command "& """%~dp0build.ps1""" -verbosity diagnostic -ci %*"
EXIT /B %ERRORLEVEL%

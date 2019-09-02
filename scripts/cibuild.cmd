@ECHO OFF
powershell.exe -NoLogo -NoProfile -ExecutionPolicy ByPass -Command "& """%~dp0build.ps1"""-verbosity diag -ci %*"
EXIT /B %ERRORLEVEL%

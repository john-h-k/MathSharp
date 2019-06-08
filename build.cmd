@ECHO OFF
WHERE pwsh >NUL
IF %ERRORLEVEL% NEQ 0 ( 
    echo Using PS for Windows
    powershell.exe -NoLogo -NoProfile -ExecutionPolicy ByPass -Command "& """%~dp0scripts\build.ps1""" %*" 
) ELSE ( 
    echo Using PS Core 6
    pwsh.exe -NoLogo -NoProfile -ExecutionPolicy ByPass -Command "& """%~dp0scripts\build.ps1""" %*" )
EXIT /B %ERRORLEVEL%
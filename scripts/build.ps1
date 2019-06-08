[CmdletBinding(PositionalBinding=$false)]
Param(
    [ValidateSet("Debug", "Release")] [string]$configuration = "Debug", # default Debug
    [switch]$clean,
    [switch]$restore,
    [switch]$skipbuild,
    [switch]$help,
    [switch]$skiptests,
    [switch]$ci,
    [switch]$package,
    [ValidateSet("Any CPU", "x86", "x64", "ARM", "ARM64")] [string]$arch = "Any CPU", # default Any CPU, but not self contained
    [ValidateSet("quiet", "minimal", "normal", "detailed", "diagnostic")] [string]$verbosity = "minimal",
    [Parameter(ValueFromRemainingArguments=$true)] [string[]]$msbuildargs
)

$RepoRoot = Join-Path -Path $PSScriptRoot -ChildPath ".."
$solution = Join-Path -Path $RepoRoot -ChildPath "MathSharp.sln"

function Write-Help() {
    Write-Host -Object "Switches and options: "
    Write-Host -Object "    -clean                              Clean the solution"
    Write-Host -Object "    -configuration [Debug|Release]      Use Debug or Release mode when building"
    Write-Host -Object "    -restore                            Restore all dependencies"
    Write-Host -Object "    -skiptests                          Skip execution of tests - faster"
    Write-Host -Object "    -help                               Print this text"
    Write-Host -Object "    -arch [Any CPU|x86|x64|ARM|ARM64]   Select the architecture to build for"
    Write-Host -Object "\n"
    Write-Host -Object "All other arguments are passed through to MSBuild"
}

function New-Library() {
    & dotnet build --no-restore -c "$configuration" -v "$verbosity" "$solution" $msbuildargs

    if ($LastExitCode -ne 0) {
        throw "'New-Library'' failed for '$solution'"
    }
}

function Test-Library() {
    & dotnet test --no-build --no-restore -c "$configuration" -v "$verbosity" "$solution"  $msbuildargs 

    if ($LastExitCode -ne 0) {
        throw "'Test-Library' failed for '$solution'"
    }
}

function Restore-Dependencies() {
    & dotnet restore -v "$verbosity" "$solution" $msbuildargs
    if ($LastExitCode -ne 0) {
        throw "'Restore-Dependencies' failed for '$solution'"
    }
}

function Clear-Artifacts() {
    & dotnet clean -c "$configuration" -v "$verbosity" "$solution"
    if ($LastExitCode -ne 0) {
        throw "'Clear-Artifacts' failed for '$solution'"
    }
}

function New-NugetPackage() {
    & dotnet pack -c "$configuration" -v "$verbosity" --no-build --no-restore "$solution" $msbuildargs 

    if ($LastExitCode -ne 0) {
        throw "'New-NugetPackage' failed for '$solution'"
    }
}

try {
    if ($ci) {
        $restore=$true
        $skipbuild=$false
        $skiptests=$false
        $package=$true
    }

    if ($help) {
        Write-Help
        exit 0
    }

    if ($clean) {
        Clear-Artifacts
    }

    if ($restore) {
        Restore-Dependencies
    }

    if (!$skipbuild) {
        New-Library
    }

    if (!$skiptests) {
        Test-Library
    }

    if ($package) {
        New-NugetPackage
    }
}
catch {
    Write-Host -Object $_
    Write-Host -Object $_.Exception
    Write-Host -Object $_.ScriptStackTrace
    exit 1
}


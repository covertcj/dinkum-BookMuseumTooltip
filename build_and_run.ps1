<#
.SYNOPSIS
Builds the .NET project, copies the DLL to the Dinkum plugins directory, and starts the game.

.DESCRIPTION
Builds the project in Debug or Release configuration based on the --release argument. Copies the resulting DLL to the Dinkum game's BepInEx plugins directory and starts the game.

.PARAMETER Release
Builds the project in Release configuration if specified.

.NOTES
Requires the 'DINKUM_DIR' environment variable to be set to the Dinkum game root directory.
#>

param(
    [switch]$Release
)

$dinkumDir = $env:DINKUM_DIR
if (-not $dinkumDir) {
    Write-Error "Environment variable 'DINKUM_DIR' is not set."
    exit 1
}

$buildConfig = if ($Release) { "Release" } else { "Debug" }

dotnet build --configuration $buildConfig

$sourcePath = ".\bin\$buildConfig\netstandard2.0\io.cjc.BookMuseumTooltip.dll"
$destinationPath = Join-Path -Path $dinkumDir -ChildPath "BepInEx\plugins\io.cjc.BookMuseumTooltip.dll"
$executablePath = Join-Path -Path $dinkumDir -ChildPath "Dinkum.exe"

try {
    Copy-Item -Path $sourcePath -Destination $destinationPath -Force
    Write-Host "DLL file copied successfully."
}
catch {
    Write-Error "Failed to copy DLL file: $_"
    exit 1
}

try {
    Start-Process -FilePath $executablePath
    Write-Host "Dinkum started successfully."
}
catch {
    Write-Error "Failed to start Dinkum: $_"
    exit 1
}

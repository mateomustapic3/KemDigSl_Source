param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [string]$Runtime = 'win-x64',

    [switch]$SelfContained = $true
)

$ErrorActionPreference = 'Stop'

$projectRoot = Split-Path -Parent $PSScriptRoot
$distRoot = Join-Path $projectRoot 'dist'
$installerDir = Join-Path $distRoot 'installer'
$payloadScript = Join-Path $PSScriptRoot 'prepare_quick_installer_payload.ps1'
$issPath = Join-Path $projectRoot 'KemDigSlInstaller.Quick.iss'
$iscc = 'C:\Program Files (x86)\Inno Setup 6\ISCC.exe'

if (-not (Test-Path $payloadScript)) {
    throw "Missing payload script: $payloadScript"
}

if (-not (Test-Path $issPath)) {
    throw "Missing installer script: $issPath"
}

if (-not (Test-Path $iscc)) {
    throw "Inno Setup compiler not found: $iscc"
}

function Get-FreeDriveLetter {
    $used = (Get-PSDrive -PSProvider FileSystem).Name
    foreach ($letter in [char[]]([char]'Z'..[char]'P')) {
        if ($used -notcontains $letter.ToString()) {
            return ('{0}:' -f $letter)
        }
    }

    throw 'No free drive letter available for subst.'
}

if (Test-Path $installerDir) {
    Remove-Item -LiteralPath $installerDir -Recurse -Force
}

$payloadArgs = @(
    '-ExecutionPolicy', 'Bypass',
    '-File', $payloadScript,
    '-Configuration', $Configuration,
    '-Runtime', $Runtime
)

if ($SelfContained) {
    $payloadArgs += '-SelfContained'
}

& powershell @payloadArgs
if ($LASTEXITCODE -ne 0) {
    throw "Payload preparation failed with exit code $LASTEXITCODE"
}

$substDrive = Get-FreeDriveLetter
$substCreated = $false

try {
    & cmd /c "subst $substDrive `"$projectRoot`""
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to create subst drive $substDrive for $projectRoot"
    }

    $substCreated = $true
    $sourceDir = "$substDrive\dist\quick_payload\app"

    Write-Host "Compiling quick installer ..."
    & $iscc "/DSourceDir=$sourceDir" $issPath
    if ($LASTEXITCODE -ne 0) {
        throw "ISCC failed with exit code $LASTEXITCODE"
    }
}
finally {
    if ($substCreated) {
        & cmd /c "subst $substDrive /d" | Out-Null
    }
}

Write-Host ""
Write-Host "Quick installer output: $installerDir"

param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [string]$Runtime = 'win-x64',

    [switch]$SelfContained = $true
)

$ErrorActionPreference = 'Stop'

function Copy-Tree {
    param(
        [Parameter(Mandatory = $true)][string]$Source,
        [Parameter(Mandatory = $true)][string]$Destination,
        [string[]]$ExcludeDirs = @(),
        [string[]]$ExcludeFiles = @()
    )

    if (-not (Test-Path $Source)) {
        Write-Host "Skip (missing): $Source"
        return
    }

    New-Item -ItemType Directory -Force -Path $Destination | Out-Null

    $args = @(
        $Source, $Destination,
        '/E', '/R:1', '/W:1',
        '/NFL', '/NDL', '/NJH', '/NJS', '/NP'
    )

    if ($ExcludeDirs.Count -gt 0) {
        $args += '/XD'
        $args += $ExcludeDirs
    }

    if ($ExcludeFiles.Count -gt 0) {
        $args += '/XF'
        $args += $ExcludeFiles
    }

    $null = & robocopy @args
    if ($LASTEXITCODE -ge 8) {
        throw "robocopy failed with exit code $LASTEXITCODE (Source=$Source)"
    }
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$distRoot = Join-Path $projectRoot 'dist'
$payloadRoot = Join-Path $distRoot 'quick_payload'
$publishDir = Join-Path $payloadRoot 'app'

if (Test-Path $payloadRoot) {
    Remove-Item -LiteralPath $payloadRoot -Recurse -Force
}

New-Item -ItemType Directory -Force -Path $publishDir | Out-Null

Write-Host "Publishing application ($Configuration, $Runtime, self-contained=$SelfContained) ..."
$publishArgs = @(
    'publish',
    (Join-Path $projectRoot 'Project.csproj'),
    '-c', $Configuration,
    '-r', $Runtime,
    '-o', $publishDir,
    '-p:PublishSingleFile=false',
    '-p:PublishTrimmed=false'
)

if ($SelfContained) {
    $publishArgs += @('--self-contained', 'true')
}
else {
    $publishArgs += @('--self-contained', 'false')
}

& dotnet @publishArgs
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE"
}

$generalExcludeDirs = @('.git', '.github', '.vscode', '.vs', 'bin', 'obj', '__pycache__', 'outputs', 'venv', '.venv', 'env', 'notebooks')
$generalExcludeFiles = @('*.pyc', '*.pyo', '*.log')

Write-Host "Copying offline runtime assets ..."

Copy-Tree -Source (Join-Path $projectRoot 'PythonRuntime') -Destination (Join-Path $publishDir 'PythonRuntime') -ExcludeDirs @('.git', '.github', '.vscode', '.vs', '__pycache__') -ExcludeFiles @('*.pyc', '*.pyo')

$foldersToCopy = @(
    'python',
    'CARTOONIFY',
    'DDCOLOR',
    'CODEFORMER',
    'GFPGAN',
    'BCKG_REMOVAL',
    'DETECTION',
    'STYLE_TRANSFER',
    'ESRGAN',
    'images'
)

foreach ($folder in $foldersToCopy) {
    Copy-Tree -Source (Join-Path $projectRoot $folder) -Destination (Join-Path $publishDir $folder) -ExcludeDirs $generalExcludeDirs -ExcludeFiles $generalExcludeFiles
}

$filesToCopy = @(
    'ai-image.ico'
)

foreach ($file in $filesToCopy) {
    $sourceFile = Join-Path $projectRoot $file
    if (Test-Path $sourceFile) {
        Copy-Item -LiteralPath $sourceFile -Destination (Join-Path $publishDir $file) -Force
    }
}

Get-ChildItem -Path $publishDir -Recurse -File -Filter *.pdb -ErrorAction SilentlyContinue |
    Remove-Item -Force

Get-ChildItem -Path $publishDir -Recurse -File -Include .gitignore -ErrorAction SilentlyContinue |
    Remove-Item -Force

Write-Host ""
Write-Host "Quick installer payload ready: $publishDir"

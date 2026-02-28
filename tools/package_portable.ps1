param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [string]$Runtime = 'win-x64',

    [switch]$SelfContained
)

$ErrorActionPreference = 'Stop'

function Robocopy-Dir {
    param(
        [Parameter(Mandatory = $true)][string]$Source,
        [Parameter(Mandatory = $true)][string]$Destination
    )

    if (-not (Test-Path $Source)) {
        Write-Host "Skip (missing): $Source"
        return
    }

    New-Item -ItemType Directory -Force -Path $Destination | Out-Null

    $excludeDirs = @(
        '.git', '.github', '.vscode', '.vs',
        'bin', 'obj',
        '__pycache__',
        'venv', '.venv', 'env',
        'notebooks', 'tests',
        'outputs'
    )

    $excludeFiles = @('*.pyc', '*.pyo', '*.log')

    $args = @(
        $Source, $Destination,
        '/E', '/R:1', '/W:1',
        '/NFL', '/NDL', '/NJH', '/NJS', '/NP',
        '/XD'
    ) + $excludeDirs + @('/XF') + $excludeFiles

    $null = & robocopy @args
    if ($LASTEXITCODE -ge 8) {
        throw "robocopy failed with exit code $LASTEXITCODE (Source=$Source)"
    }
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$distRoot = Join-Path $projectRoot 'dist'
$outRoot = Join-Path $distRoot 'portable'
$publishDir = Join-Path $outRoot 'app'

if (Test-Path $outRoot) {
    Remove-Item -Recurse -Force $outRoot
}

New-Item -ItemType Directory -Force -Path $publishDir | Out-Null

Write-Host "Publishing ($Configuration, $Runtime, self-contained=$SelfContained) ..."
$publishArgs = @(
    'publish',
    (Join-Path $projectRoot 'Project.csproj'),
    '-c', $Configuration,
    '-r', $Runtime,
    '-o', $publishDir
)

if ($SelfContained) {
    $publishArgs += @('--self-contained', 'true')
} else {
    $publishArgs += @('--self-contained', 'false')
}

& dotnet @publishArgs

Write-Host "Copying offline assets into: $publishDir"
$foldersToCopy = @(
    'PythonRuntime',
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

foreach ($f in $foldersToCopy) {
    Robocopy-Dir -Source (Join-Path $projectRoot $f) -Destination (Join-Path $publishDir $f)
}

Write-Host ""
Write-Host "Done."
Write-Host "Portable folder: $publishDir"
Write-Host "Run on target machine: `"$publishDir\\Project.exe`""


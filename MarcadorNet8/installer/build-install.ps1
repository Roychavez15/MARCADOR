# Publica Marcador Admin + Display en una sola carpeta, self-contained win-x64.
# No requiere .NET instalado en la PC destino (incluye runtime ~190 MB).
# Uso: desde esta carpeta, ejecutar:  powershell -ExecutionPolicy Bypass -File .\build-install.ps1
# Opcional: -Zip  genera además Marcador-win-x64.zip junto a la carpeta out

param(
    [switch] $Zip,
    [string] $Runtime = "win-x64"
)

$ErrorActionPreference = "Stop"
$here = $PSScriptRoot
$root = Split-Path $here -Parent
$out = Join-Path $here "out\$Runtime"

Write-Host "Salida: $out" -ForegroundColor Cyan
if (Test-Path $out) {
    Remove-Item $out -Recurse -Force
}
New-Item -ItemType Directory -Path $out -Force | Out-Null

$pubArgs = @(
    "-c", "Release",
    "-r", $Runtime,
    "--self-contained", "true",
    "-o", $out,
    "/p:DebugType=None",
    "/p:DebugSymbols=false"
)

Write-Host "dotnet publish Marcador.Admin..." -ForegroundColor Yellow
& dotnet publish (Join-Path $root "Marcador.Admin\Marcador.Admin.csproj") @pubArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "dotnet publish Marcador.Display..." -ForegroundColor Yellow
& dotnet publish (Join-Path $root "Marcador.Display\Marcador.Display.csproj") @pubArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$bytes = (Get-ChildItem $out -Recurse -File | Measure-Object -Property Length -Sum).Sum
$mb = [math]::Round($bytes / 1MB, 1)
Write-Host "Listo: $out  (~$mb MB)" -ForegroundColor Green

if ($Zip) {
    $zipPath = Join-Path $here "Marcador-$Runtime.zip"
    if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
    Compress-Archive -Path (Join-Path $out "*") -DestinationPath $zipPath -CompressionLevel Optimal
    Write-Host "ZIP: $zipPath" -ForegroundColor Green
}

$inno = @(
    "${env:LOCALAPPDATA}\Programs\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles}\Inno Setup 6\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

if ($inno) {
    Write-Host "Compilando instalador con Inno Setup..." -ForegroundColor Yellow
    & $inno (Join-Path $here "Marcador.Inno.iss")
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Setup generado en installer\dist\" -ForegroundColor Green
    }
} else {
    Write-Host "Inno Setup 6 no encontrado. Copia la carpeta 'out\$Runtime' a la otra PC o instala Inno Setup y vuelve a ejecutar este script." -ForegroundColor DarkYellow
}

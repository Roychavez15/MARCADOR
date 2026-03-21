; Requiere Inno Setup 6 (gratis): https://jrsoftware.org/isdl.php
; Primero ejecutar build-install.ps1 para generar out\win-x64
; Luego compilar este script (ISCC) o dejar que build-install.ps1 lo invoque si ISCC está instalado.

#define MyAppName "Marcador LBG"
#define MyAppVersion "1.0.0"
#define PublishDir "out\win-x64"
#define OutputDir "dist"

[Setup]
AppId={{A7E2F1B4-8C3D-4E5F-9A0B-1C2D3E4F5A6B}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir={#SourcePath}\{#OutputDir}
OutputBaseFilename=MarcadorLBG-Setup-{#MyAppVersion}-win-x64
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopadmin"; Description: "Acceso directo al escritorio (Administración)"; GroupDescription: "Accesos directos:"
Name: "desktopdisplay"; Description: "Acceso directo al escritorio (Pantalla marcador)"; GroupDescription: "Accesos directos:"

[Files]
Source: "{#SourcePath}\{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Administración"; Filename: "{app}\Marcador.Admin.exe"; WorkingDir: "{app}"
Name: "{group}\Pantalla marcador"; Filename: "{app}\Marcador.Display.exe"; WorkingDir: "{app}"
Name: "{autodesktop}\Marcador — Admin"; Filename: "{app}\Marcador.Admin.exe"; Tasks: desktopadmin; WorkingDir: "{app}"
Name: "{autodesktop}\Marcador — Pantalla"; Filename: "{app}\Marcador.Display.exe"; Tasks: desktopdisplay; WorkingDir: "{app}"

[Run]
Filename: "{app}\Marcador.Admin.exe"; Description: "Iniciar Administración (recomendado: primero Admin, luego Pantalla)"; Flags: nowait postinstall skipifsilent

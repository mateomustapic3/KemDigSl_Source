#define MyAppName "KemDigSl"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "KemDigSl"
#define MyAppExeName "Project.exe"
#ifndef SourceDir
  #define SourceDir "dist\\quick_payload\\app"
#endif

[Setup]
AppId={{E6E15D7C-7D1F-4A07-89F4-5C1A4A3F12D3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DefaultDirName={autopf32}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableDirPage=no
PrivilegesRequired=admin
OutputDir=dist\installer
OutputBaseFilename=KemDigSl_Setup_Quick
SetupIconFile={#SourceDir}\ai-image.ico
Compression=none
SolidCompression=no
DiskSpanning=yes
DiskSliceSize=1500000000
WizardStyle=modern
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: checkedonce

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent

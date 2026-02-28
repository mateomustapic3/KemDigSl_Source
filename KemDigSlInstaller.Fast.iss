#define MyAppName "KemDigSl"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "KemDigSl"
#define MyAppExeName "Project.exe"
#ifndef SourceDir
  #define SourceDir "dist\\portable\\app"
#endif

[Setup]
AppId={{E6E15D7C-7D1F-4A07-89F4-5C1A4A3F12D3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf32}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableDirPage=no
PrivilegesRequired=admin
OutputDir=dist\installer
OutputBaseFilename=KemDigSl_Setup_Fast
SetupIconFile={#SourceDir}\ai-image.ico
Compression=zip
SolidCompression=no
DiskSpanning=yes
DiskSliceSize=700000000
WizardStyle=modern
UninstallDisplayIcon={app}\ai-image.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: checkedonce

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; IconFilename: "{app}\ai-image.ico"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; IconFilename: "{app}\ai-image.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent

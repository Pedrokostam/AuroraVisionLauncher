; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "AuroraVisionLauncher"
;#define VERSION "0.0.0.0"
#define MyAppPublisher "VanCorp"
#define MyAppURL "https://github.com/Pedrokostam/AuroraVisionLauncher"
#define MyAppExeName "AuroraVisionLauncher.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{2C508297-5294-4681-BB5E-D2B20F6FC4AB}
AppName={#MyAppName}
AppVersion={#VERSION}
;AppVerName={#MyAppName} {#VERSION}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
; "ArchitecturesAllowed=x64compatible" specifies that Setup cannot run
; on anything but x64 and Windows 11 on Arm.
ArchitecturesAllowed=x64compatible
; "ArchitecturesInstallIn64BitMode=x64compatible" requests that the
; install be done in "64-bit mode" on x64 or Windows 11 on Arm,
; meaning it should use the native 64-bit Program Files directory and
; the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64compatible
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=C:\Users\Pedro\source\repos\AuroraVisionLauncher\LICENSE.txt
InfoBeforeFile=C:\Users\Pedro\source\repos\AuroraVisionLauncher\LICENSE.txt
InfoAfterFile=C:\Users\Pedro\source\repos\AuroraVisionLauncher\LICENSE.txt

; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern

CloseApplications=yes
;CreateUninstallRegKey=yes
;UpdateUninstallLogAppName=yes
[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\Pedro\source\repos\AuroraVisionLauncher\Release\AuroraVisionLauncher\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Pedro\source\repos\AuroraVisionLauncher\Release\AuroraVisionLauncher\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

;[Registry]
;;Registry data from file fiexe.reg
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiexe"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiproj"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.avproj"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.avexe"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiexe\DefaultIcon"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiexe\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "C:\Users\Pedro\AppData\Local\AuroraVisionLauncher\Icons\FabImageRuntime.ico,0"; Flags: uninsdeletevalue; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiexe\shell"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiexe\shell\open"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiexe\shell\open\command"; Flags: uninsdeletekey; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\Classes\AuroraVisionLauncher.fiexe\shell\open\command"; ValueType: string; ValueName: ""; ValueData: "\""C:\Users\Pedro\source\repos\AuroraVisionLauncher\AuroraVisionLauncher\bin\publish\AuroraVisionLauncher.exe\"" \""%1\"""; Flags: uninsdeletevalue; Check: not IsAdminInstallMode
;;End of registry data from file fiexe.reg

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

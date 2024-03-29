﻿# Auto-generated by EclipseNSIS Script Wizard
# 27.07.2010 15:45:25

Name $(SM_Prog_NAME)
!define NameInt "Miakro911"
!define DirName "Miakro911"
!define CompName "9-Bits"
!define BinDir "..\..\_bin\@bin_type@\"
#!define DirName "@ProgDirName@"

#!define SHORT_APP_NAME "Xobni Analytics"
#!define SUPPORT_EMAIL "support@xobni.com"

SetCompressor /SOLID lzma
SetDatablockOptimize on
CRCCheck on

RequestExecutionLevel admin

# General Symbol Definitions
!define REGKEY_old "SOFTWARE\${NameInt}"
!define REGKEY "SOFTWARE\${CompName}\${NameInt}"
# MUI Symbol Definitions
!define MUI_ICON "..\art\icon_green.ico"
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_UNICON "..\art\icon_no_green.ico"
!define MUI_UNFINISHPAGE_NOAUTOCLOSE
!define MUI_COMPONENTSPAGE_SMALLDESC


Caption "$(Prog_NAME)"
BrandingText "$(Branding)"

# Included files
!include Sections.nsh
!include MUI2.nsh
!include StrRep.nsh
!include ReplaceInFile.nsh
!include x64.nsh

# Reserved Files
ReserveFile "${NSISDIR}\Plugins\AdvSplash.dll"

# Variables
Var StartMenuGroup
Var Inst_code

# Installer pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE ..\licenseansi.rtf
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

#uninstaller pages
!insertmacro MUI_UNPAGE_CONFIRM
;!insertmacro MUI_UNPAGE_COMPONENTS
!insertmacro MUI_UNPAGE_INSTFILES

# Installer languages
!insertmacro MUI_LANGUAGE English
!insertmacro MUI_LANGUAGE Russian

!include "DotNETLoc.nsh"
!include include\LangStrings.nsh
!include include\Macros.nsh
!include include\Common.nsh
!include include\Party3d.nsh

# Installer attributes
OutFile setup.exe
InstallDir "$PROGRAMFILES\${DirName}"
CRCCheck on
XPStyle on
ShowInstDetails show
InstallDirRegKey HKCU "${REGKEY_old}" Path
InstallDirRegKey HKCU "${REGKEY}" Path
ShowUninstDetails show

InstType $(SEC_PackClient_NAME)
InstType $(SEC_PackServer_NAME)
InstType $(SEC_PackServerFull_NAME)
InstType $(SEC_PackFull_NAME)

# Installer sections
Section $(SEC_Rabnet_NAME) SEC_Rabnet
    SectionIn 1 4

    Call CloseAll

    SetOutPath $INSTDIR\bin
    SetOverwrite on
	
	Call InstBothReqFiles
	
    File ${BinDir}\bin\rabnet.exe
	File ${BinDir}\bin\rabnet.exe.config	   
    
	File ${BinDir}\bin\Pickers.dll 	
    File ${BinDir}\bin\RdlEngine.dll
    File ${BinDir}\bin\RdlViewer.dll
	File ${BinDir}\bin\SplitButton.dll			
	#File ${BinDir}\bin\CAS.DLL
	File ${BinDir}\bin\gui_genetics.dll   
	
	File ${BinDir}\bin\changeLog.html
    File ${BinDir}\bin\rabHelp.chm
	
    SetOutPath $INSTDIR\bin\reports
    File ${BinDir}\bin\reports\*.rdl
	File ${BinDir}\..\PlugIns\*.dll
	
    SetOutPath $INSTDIR\bin
    
    ######## Temporary fix of bug M0000308
    ExpandEnvStrings $2 "%USERNAME%"
    
    ExecWait 'cacls "$INSTDIR\bin" /E /G "$2":F'
    ExecWait 'cacls "$INSTDIR\bin\rabnet.exe.config" /E /G "$2":F'
    ######## end

    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut $SMPROGRAMS\$StartMenuGroup\$(SM_Prog_NAME).lnk $INSTDIR\bin\rabnet.exe
    CreateShortcut $DESKTOP\$(SM_Prog_NAME).lnk $INSTDIR\bin\rabnet.exe
#    WriteRegStr HKEY_CURRENT_USER Software\hzkakzvat\rabnet Path "C:\Program Files\7-Zip"
    WriteRegStr HKCU "${REGKEY}\components" "rabnet" 1
SectionEnd

Section $(SEC_RabDump_NAME) SEC_RabDump
#Section /o $(SEC_RabDump_NAME) SEC_RabDump
    SectionIn 2 3 4

    Call CloseAll

    SetOutPath $INSTDIR\bin
    SetOverwrite on
	
	Call InstBothReqFiles
	
	File ${BinDir}\bin\rabdump.exe
	File ${BinDir}\bin\rabdump.exe.config
	File ${BinDir}\bin\ccxmlrpc.dll    	   	
	#File ${BinDir}\bin\gnclient.ini
	
    SetOutPath $INSTDIR\7z
    File ${BinDir}\7z\7-zip.chm
    File ${BinDir}\7z\7za.exe
    File ${BinDir}\7z\copying.txt
    File ${BinDir}\7z\license.txt
    File ${BinDir}\7z\readme.txt

    CreateShortcut $SMPROGRAMS\$StartMenuGroup\$(SM_Dump_NAME).lnk $INSTDIR\bin\rabdump.exe
    WriteRegStr HKCU "${REGKEY}\components" "rabdump" 1

    ######## Temporary fix of bug M0000308
    ExpandEnvStrings $2 "%USERNAME%"

    ExecWait 'cacls "$INSTDIR\bin" /E /G "$2":F'
    ######## end

    !insertmacro SelectSection "SEC_Updater"  
SectionEnd

Section $(SEC_Mysql_NAME) SEC_Mysql
    SectionIn 3 4
    DetailPrint $(MYSQLINSTALLER_Start)

    ExecWait 'msiexec /i "$EXEDIR\mysql\mysql-essential-5.1.49-win32.msi" /qr INSTALLDIR="$PROGRAMFILES\MySQL\MySQL Server 5.1\"  DATADIR="$PROGRAMFILES\MySQL\MySQL Server 5.1\" /L* C:\MSI-MySQL-Log.txt' $Inst_code
#    StrCmp $Inst_code 0 ok
    DetailPrint $Inst_code
#    ok:
    DetailPrint "$Inst_code"
    DetailPrint $(MYSQLINSTALLER_Configure)
#    RmDir /r /REBOOTOK "$PROGRAMFILES\MySQL\MySQL Server 5.1\Data"
    ExecWait '"$PROGRAMFILES\MySQL\MySQL Server 5.1\bin\mysqlinstanceconfig.exe" -i -q "-lC:\mysql_install_log.txt" "-p$PROGRAMFILES\MySQL\MySQL Server 5.1" AddBinToPath=yes ConnectionUsage=DSS ServiceName=MySQL5_1 ServerType=SERVER DatabaseType=INNODB Port=3306 RootCurrentPassword=mysql Charset=utf8 StrictMode=no'
#    ExecWait '"$PROGRAMFILES\MySQL\MySQL Server 5.1\bin\mysqlinstanceconfig.exe" -i -q "-lC:\mysql_install_log.txt" "-p$PROGRAMFILES\MySQL\MySQL Server 5.1" AddBinToPath=yes ConnectionUsage=DSS ServiceName=MySQL5_1 ServerType=SERVER DatabaseType=MIXED Port=3306 RootCurrentPassword=mysql Charset=utf8 StrictMode=no'
#    ExecWait '"$PROGRAMFILES\MySQL\MySQL Server 5.1\bin\mysqlinstanceconfig.exe" -i -q "-lC:\mysql_install_log.txt" "-p$PROGRAMFILES\MySQL\MySQL Server 5.1" AddBinToPath=yes ConnectionUsage=DSS ServiceName=MySQL5_1 RootPassword=mysql ServerType=SERVER DatabaseType=MIXED Port=3306 RootCurrentPassword=mysql Charset=utf8 StrictMode=no'
    DetailPrint $(MYSQLINSTALLER_Done)
#"C:\Program Files\MySQL\MySQL Server 6.0\bin\mysqlinstanceconfig.exe" -i -q ServiceName=MySQL RootPassword=mysql ServerType=DEVELOPMENT DatabaseType=MYISAM Port=3306 RootCurrentPassword=mysql
SectionEnd
    
Section -com_comps SEC_Common
    SetOverwrite on  
    Call InstCommonFiles
	
    CreateShortcut $SMPROGRAMS\$StartMenuGroup\$(SM_Conv_NAME).lnk $INSTDIR\bin\mia_conv.exe
    CreateShortcut $SMPROGRAMS\$StartMenuGroup\$(SM_Up_NAME).lnk $INSTDIR\bin\updater.exe
    WriteRegStr HKCU "${REGKEY}\components" com_comps 1
SectionEnd

Section -sec_updater SEC_Updater
    DetailPrint $(UPDATER_Run)
    ExecWait '"$INSTDIR\bin\updater.exe" /d'
  
    Exec "$INSTDIR\bin\rabdump.exe"

    ######## Temporary fix of bug M0000308
    ExpandEnvStrings $2 "%USERNAME%"

    ExecWait 'cacls "$INSTDIR\bin\rabdump.exe.config" /E /G "$2":F'
    ######## end
SectionEnd

# Macro for selecting uninstaller sections
!macro SELECT_SECTION_TEST 
    Push $R0
    ReadRegStr $R0 HKCU "${REGKEY}\components" "Main"
    StrCmp $R0 1 0 next${SECTION_ID}

    !insertmacro SELECT_UNSECTION "rabnet" ${SEC_Rabnet}
    !insertmacro SELECT_UNSECTION "rabdump" ${SEC_RabDump}
    !insertmacro SELECT_UNSECTION "rabdump" ${SEC_Updater}

    !insertmacro SelectSection "${SEC_Common}"
    #!insertmacro SELECT_UNSECTION "com_comps" ${SEC_Common}

    GoTo done${SECTION_ID}
next${SECTION_ID}:
    
done${SECTION_ID}:
    Pop $R0
!macroend

!macro SELECT_SECTION_TEST_old
    Push $R0
    ReadRegStr $R0 HKCU "${REGKEY_old}\components" "Main"
    StrCmp $R0 1 0 nextold${SECTION_ID}

    !insertmacro SELECT_UNSECTION_old "rabnet" ${SEC_Rabnet}
    !insertmacro SELECT_UNSECTION_old "rabdump" ${SEC_RabDump}
    !insertmacro SELECT_UNSECTION_old "rabdump" ${SEC_Updater}

    GoTo doneold${SECTION_ID}
nextold${SECTION_ID}:
    
doneold${SECTION_ID}:
    Pop $R0
!macroend


# Uninstaller sections
Section un.$(SEC_Rabnet_NAME) UNSEC_Rabnet
    Call un.CloseAll

    ;DeleteRegValue HKEY_CURRENT_USER Software\hzkakzvat\rabnet Path
    Delete /REBOOTOK $SMPROGRAMS\$StartMenuGroup\rabnet.lnk
    Delete /REBOOTOK $DESKTOP\$(SM_Prog_NAME).lnk
	
	Call un.Reports   

    Delete /REBOOTOK $INSTDIR\bin\rabnet.exe
	Delete /REBOOTOK $INSTDIR\bin\rabnet.exe.config
	Delete /REBOOTOK $INSTDIR\bin\db.Interface.dll
    Delete /REBOOTOK $INSTDIR\bin\db.mysql.dll
    Delete /REBOOTOK $INSTDIR\bin\engine.dll
    Delete /REBOOTOK $INSTDIR\bin\MySql.Data.dll
    Delete /REBOOTOK $INSTDIR\bin\Pickers.dll
    Delete /REBOOTOK $INSTDIR\bin\rabHelp.chm
    Delete /REBOOTOK $INSTDIR\bin\RdlEngine.dll
    Delete /REBOOTOK $INSTDIR\bin\log4net.dll
    Delete /REBOOTOK $INSTDIR\bin\RdlViewer.dll    
	#Delete /REBOOTOK $INSTDIR\bin\CAS.dll		
	Delete /REBOOTOK $INSTDIR\bin\changeLog.txt
	Delete /REBOOTOK $INSTDIR\bin\SplitButton.dll
	
    RmDir /REBOOTOK /r $INSTDIR\bin\upd
	RmDir /REBOOTOK $INSTDIR\bin
	
    DeleteRegValue HKCU "${REGKEY}\components" "rabnet"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(SM_Prog_NAME).lnk"
SectionEnd

Section un.$(SEC_RabDump_NAME) UNSEC_RabDump
    Call un.CloseAll
	
	Delete /REBOOTOK $INSTDIR\bin\log4net.dll
	
	Delete /REBOOTOK $INSTDIR\bin\db.Interface.dll
	Delete /REBOOTOK $INSTDIR\bin\db.mysql.dll
	Delete /REBOOTOK $INSTDIR\bin\engine.dll
	
	Delete /REBOOTOK $INSTDIR\bin\ccxmlrpc.dll
    Delete /REBOOTOK $INSTDIR\bin\rabdump.exe
	Delete /REBOOTOK $INSTDIR\bin\rabdump.exe.config

    RmDir /REBOOTOK /r $INSTDIR\7z
    RmDir /REBOOTOK /r $INSTDIR\bin\updates
	RmDir /REBOOTOK /r $INSTDIR\bin
    DeleteRegValue HKCU "${REGKEY}\components" "rabdump"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(SM_Dump_NAME).lnk"
SectionEnd

Section un.com_comps UNSEC_Common
	    
	Delete /REBOOTOK $INSTDIR\bin\mia_conv.exe
	Delete /REBOOTOK $INSTDIR\bin\mia_conv.exe.config
	Delete /REBOOTOK $INSTDIR\bin\log4net.dll
	Delete /REBOOTOK $INSTDIR\bin\updater.exe
	Delete /REBOOTOK $INSTDIR\bin\updater.exe.config
	
    Delete /REBOOTOK $SMPROGRAMS\$StartMenuGroup\$(SM_Conv_NAME).lnk
    Delete /REBOOTOK $SMPROGRAMS\$StartMenuGroup\$(SM_Up_NAME).lnk
    DeleteRegValue HKCU "${REGKEY}\components" com_comps
SectionEnd

Section un.post UNSEC_Sys	
    DeleteRegKey HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk"
    Delete /REBOOTOK $INSTDIR\uninstall.exe
    DeleteRegKey HKCU "${REGKEY_old}"
    DeleteRegValue HKCU "${REGKEY}" Path
    DeleteRegKey /IfEmpty HKCU "${REGKEY}\components"
    DeleteRegKey /IfEmpty HKCU "${REGKEY}"
    RmDir /r /REBOOTOK "$SMPROGRAMS\$StartMenuGroup"
    RmDir /REBOOTOK "$INSTDIR"
SectionEnd

# Installer functions
Function .onInit
    
    ;LogSet on
    
    InitPluginsDir
    StrCpy $StartMenuGroup $(SM_Prog_NAME)
	
    #SectionSetText ${SEC_Rabnet} $(SEC_Rabnet_NAME) 
    #SectionSetText ${SEC_RabDump} $(SEC_RabDump_NAME) 

    IfSilent +6
    Push $R1
    File /oname=$PLUGINSDIR\spltmp.bmp ..\art\logo1_big.bmp
    advsplash::show 1000 600 400 -1 $PLUGINSDIR\spltmp
    Pop $R1
    Pop $R1

	${If} ${RunningX64} 
		DetailPrint $(INTALL64)
		; disable registry redirection (enable access to 64-bit portion of registry)
		SetRegView 64
		; change install dir 
		StrCpy $INSTDIR "$PROGRAMFILES64\${DirName}"
	${EndIf}
	
    !insertmacro SELECT_SECTION_TEST_old
    !insertmacro SELECT_SECTION_TEST
    

;    Push "AutoUpdate"         ; push the search string onto the stack
;    Push "DefaultValue"   ; push a default value onto the stack
;    Call GetParameterValue
;    Pop $2
;    MessageBox MB_OK "Value of OUTPUT parameter is '$2'"

    
FunctionEnd

Function CloseAll
	Call CloseRabNet
	Call CloseRabDump
FunctionEnd

Function CloseRabDump
    Push $5

    push "rabdump.exe"
    processwork::existsprocess
    pop $5
    IntCmp $5 0 done

    push "rabdump.exe"
    processwork::CloseProcess
;    processwork::KillProcess
    Sleep 5000

    loop:
        push "rabdump.exe"
        processwork::existsprocess
        pop $5
        IntCmp $5 0 done

        DetailPrint $(KillRabDump)

        push "rabdump.exe"
;        processwork::CloseProcess
        processwork::KillProcess
        Sleep 2000
        Goto loop

    BailOut:
        Abort

    done:
    Pop $5

FunctionEnd

# Uninstaller functions
Function un.onInit
    ReadRegStr $INSTDIR HKCU "${REGKEY}" Path
    StrCpy $StartMenuGroup $(SM_Prog_NAME)
    ;!insertmacro SELECT_UNSECTION "rabnet" ${UNSEC_Rabnet}
    ;!insertmacro SELECT_UNSECTION "rabdump" ${UNSEC_RabDump}
    ;!insertmacro SELECT_UNSECTION "com_comps" ${UNSEC_Common}
FunctionEnd

Function un.Reports
    Delete /REBOOTOK $INSTDIR\bin\reports\*.rdl	
	Delete /REBOOTOK $INSTDIR\bin\reports\*.dll
	
	RmDir /REBOOTOK  $INSTDIR\bin\reports
FunctionEnd

Function un.CloseAll
	Call un.CloseRabNet
	Call un.CloseRabDump
FunctionEnd

Function un.CloseRabDump

    Push $5

    push "rabdump.exe"
    processwork::existsprocess
    pop $5
    IntCmp $5 0 done

    loop:
        push "rabdump.exe"
        processwork::existsprocess
        pop $5
        IntCmp $5 0 done

        DetailPrint $(KillRabDump)

        push "rabdump.exe"
;        processwork::CloseProcess
        processwork::KillProcess
        Sleep 2000
        Goto loop

    BailOut:
        Abort

    done:
    Pop $5

FunctionEnd

# Section Descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${SEC_Rabnet} $(SEC_Rabnet_DESC)
	!insertmacro MUI_DESCRIPTION_TEXT ${SEC_RabDump} $(SEC_RabDump_DESC)
	!insertmacro MUI_DESCRIPTION_TEXT ${SEC_Mysql} $(SEC_Mysql_DESC)
!insertmacro MUI_FUNCTION_DESCRIPTION_END


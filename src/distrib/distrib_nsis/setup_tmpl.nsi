# Auto-generated by EclipseNSIS Script Wizard
# 27.07.2010 15:45:25

Name $(Prog_NAME)
!define NameInt "MiakroRabnet"
!define DirName "MiakroRabnet"
!define CompName "HZKakZvat"
#!define DirName "@ProgDirName@"


#!define SHORT_APP_NAME "Xobni Analytics"
#!define SUPPORT_EMAIL "support@xobni.com"

SetCompressor /SOLID lzma

RequestExecutionLevel admin



# TargetMinimalOS 5.1


# General Symbol Definitions
!define REGKEY "SOFTWARE\${NameInt}"

# MUI Symbol Definitions
!define MUI_ICON "..\art\icon_green.ico"
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_UNICON "..\art\icon_no_green.ico"
!define MUI_UNFINISHPAGE_NOAUTOCLOSE

!define MUI_COMPONENTSPAGE_SMALLDESC


# Included files
!include Sections.nsh
!include MUI2.nsh

# Reserved Files
ReserveFile "${NSISDIR}\Plugins\AdvSplash.dll"

# Variables
Var StartMenuGroup

# Installer pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE ..\..\docs\licenseansi.rtf
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

# Installer languages
!insertmacro MUI_LANGUAGE English
!insertmacro MUI_LANGUAGE Russian

!include "DotNETLoc.nsh"

# Installer attributes
OutFile setup.exe
InstallDir "$PROGRAMFILES\${DirName}"
CRCCheck on
XPStyle on
ShowInstDetails show
InstallDirRegKey HKLM "${REGKEY}" Path
ShowUninstDetails show

InstType $(SEC_PackClient_NAME)
InstType $(SEC_PackServer_NAME)
InstType $(SEC_PackServerFull_NAME)

# Installer sections
Section $(SEC_Rabnet_NAME) SEC_Rabnet
    SectionIn 1 
    SetOutPath $INSTDIR
    SetOverwrite on
    File ..\..\..\bin\protected\rabnet.exe
    File ..\..\..\bin\protected\db.mysql.dll
    File ..\..\..\bin\protected\engine.dll
#    File ..\..\..\bin\protected\gui_genetics.dll
    File ..\..\..\bin\protected\MySql.Data.dll
    File ..\..\..\bin\protected\Pickers.dll
    File ..\..\..\bin\protected\rabHelp.chm
    File ..\..\..\bin\protected\RdlEngine.dll
    File ..\..\..\bin\protected\RdlViewer.dll
    SetOutPath $INSTDIR\reports
    File ..\..\..\bin\protected\reports\age.rdl
    File ..\..\..\bin\protected\reports\breeds.rdl
    File ..\..\..\bin\protected\reports\by_month.rdl
    File ..\..\..\bin\protected\reports\dead.rdl
    File ..\..\..\bin\protected\reports\deadreason.rdl
    File ..\..\..\bin\protected\reports\empty_rev.rdl
    File ..\..\..\bin\protected\reports\fucker.rdl
    File ..\..\..\bin\protected\reports\fucks_by_date.rdl
    File ..\..\..\bin\protected\reports\okrol_user.rdl
    File ..\..\..\bin\protected\reports\plem.rdl
    File ..\..\..\bin\protected\reports\rabbit.rdl
    File ..\..\..\bin\protected\reports\realization.rdl
    File ..\..\..\bin\protected\reports\replace_plan.rdl
    File ..\..\..\bin\protected\reports\shed.rdl
    File ..\..\..\bin\protected\reports\zooteh.rdl
    File ..\..\..\bin\protected\reports\zooteh_nofuck.rdl
    SetOutPath $INSTDIR
    SetOverwrite off
    File ..\..\..\bin\protected\rabnet.exe.config
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut $SMPROGRAMS\$StartMenuGroup\$(SM_Prog_NAME).lnk $INSTDIR\rabnet.exe
#    WriteRegStr HKEY_CURRENT_USER Software\hzkakzvat\rabnet Path "C:\Program Files\7-Zip"
    WriteRegStr HKLM "${REGKEY}\Components" "rabnet" 1
SectionEnd

Section /o $(SEC_RabDump_NAME) SEC_RabDump
    SectionIn 2 3
    SetOutPath $INSTDIR
    SetOverwrite on
    File ..\..\..\bin\protected\GrdAPI32.DLL
    File ..\..\..\bin\protected\rabdump.exe
    File ..\..\..\bin\protected\key.dll
    File ..\..\..\bin\tools\updater.exe
    File ..\..\..\bin\protected\mia_conv.exe
    #SetOutPath $INSTDIR
    #SetOverwrite off
    #File ..\..\..\bin\protected\rabdump.exe.config
    CreateShortcut $SMPROGRAMS\$StartMenuGroup\$(SM_Dump_NAME).lnk $INSTDIR\rabdump.exe
    WriteRegStr HKLM "${REGKEY}\Components" "rabdump" 1
#    DetailPrint $(UPDATER_Run)
#    ExecWait '"$INSTDIR\updater.exe"'
    !insertmacro SelectSection "SEC_Updater"
SectionEnd

Section /o $(SEC_Mysql_NAME) SEC_Mysql
    SectionIn 3
    DetailPrint $(MYSQLINSTALLER_Start)
    ExecWait 'msiexec /i "$EXEDIR\mysql\mysql-essential-5.1.49-win32.msi" /qr INSTALLDIR="$PROGRAMFILES\MySQL\MySQL Server 5.1\"  DATADIR="$PROGRAMFILES\MySQL\MySQL Server 5.1\" /L* C:\MSI-MySQL-Log.txt'
    DetailPrint $(MYSQLINSTALLER_Configure)
#    RmDir /r /REBOOTOK "$PROGRAMFILES\MySQL\MySQL Server 5.1\Data"
    ExecWait '"$PROGRAMFILES\MySQL\MySQL Server 5.1\bin\mysqlinstanceconfig.exe" -i -q "-lC:\mysql_install_log.txt" "-p$PROGRAMFILES\MySQL\MySQL Server 5.1" AddBinToPath=yes ConnectionUsage=DSS ServiceName=MySQL5_1 ServerType=SERVER DatabaseType=MIXED Port=3306 RootCurrentPassword=mysql Charset=utf8 StrictMode=no'
#    ExecWait '"$PROGRAMFILES\MySQL\MySQL Server 5.1\bin\mysqlinstanceconfig.exe" -i -q "-lC:\mysql_install_log.txt" "-p$PROGRAMFILES\MySQL\MySQL Server 5.1" AddBinToPath=yes ConnectionUsage=DSS ServiceName=MySQL5_1 RootPassword=mysql ServerType=SERVER DatabaseType=MIXED Port=3306 RootCurrentPassword=mysql Charset=utf8 StrictMode=no'
    DetailPrint $(MYSQLINSTALLER_Done)
#"C:\Program Files\MySQL\MySQL Server 6.0\bin\mysqlinstanceconfig.exe" -i -q ServiceName=MySQL RootPassword=mysql ServerType=DEVELOPMENT DatabaseType=MYISAM Port=3306 RootCurrentPassword=mysql
SectionEnd
    
Section -com_comps SEC_Common
    SetOutPath $INSTDIR
    SetOverwrite on
    File ..\..\..\bin\protected\log4net.dll
    WriteRegStr HKLM "${REGKEY}\Components" com_comps 1
SectionEnd

Section -post SEC_Sys
    !insertmacro CheckDotNET
    WriteRegStr HKLM "${REGKEY}" Path $INSTDIR
    WriteRegStr HKLM "${REGKEY}\Components" "Main" 1
    SetOutPath $INSTDIR
    WriteUninstaller $INSTDIR\uninstall.exe
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk" $INSTDIR\uninstall.exe
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayName "$(^Name)"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayIcon $INSTDIR\uninstall.exe
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" UninstallString $INSTDIR\uninstall.exe
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoModify 1
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoRepair 1
SectionEnd

Section /o -sec_updater SEC_Updater
    DetailPrint $(UPDATER_Run)
    ExecWait '"$INSTDIR\updater.exe" batch'
SectionEnd

# Macro for selecting uninstaller sections
!macro SELECT_UNSECTION SECTION_NAME UNSECTION_ID
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY}\Components" "${SECTION_NAME}"
    StrCmp $R0 1 0 next${UNSECTION_ID}
    !insertmacro SelectSection "${UNSECTION_ID}"
    GoTo done${UNSECTION_ID}
next${UNSECTION_ID}:
    !insertmacro UnselectSection "${UNSECTION_ID}"
done${UNSECTION_ID}:
    Pop $R0
!macroend

!macro SELECT_SECTION SECTION_NAME SECTION_ID
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY}\Components" "${SECTION_NAME}"
    StrCmp $R0 1 0 next${SECTION_ID}
    !insertmacro SelectSection "${SECTION_ID}"
    GoTo done${SECTION_ID}
next${SECTION_ID}:
    !insertmacro UnselectSection "${SECTION_ID}"
done${SECTION_ID}:
    Pop $R0
!macroend

!macro SELECT_SECTION_TEST 
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY}\Components" "Main"
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


# Uninstaller sections
Section /o -un.com_comps UNSEC_Common
    Delete /REBOOTOK $INSTDIR\mia_conv.exe
    Delete /REBOOTOK $INSTDIR\log4net.dll
    DeleteRegValue HKLM "${REGKEY}\Components" com_comps
SectionEnd

Section /o "-un.rabdump" UNSEC_RabDump
    Delete /REBOOTOK $INSTDIR\GrdAPI32.DLL
    Delete /REBOOTOK $INSTDIR\key.dll
    Delete /REBOOTOK $INSTDIR\rabdump.exe
    Delete /REBOOTOK $INSTDIR\updater.exe
    DeleteRegValue HKLM "${REGKEY}\Components" "rabdump"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(SM_Dump_NAME).lnk"
SectionEnd

Section /o "-un.rabnet" UNSEC_Rabnet
    ;DeleteRegValue HKEY_CURRENT_USER Software\hzkakzvat\rabnet Path
    Delete /REBOOTOK $SMPROGRAMS\$StartMenuGroup\rabnet.lnk
    Delete /REBOOTOK $INSTDIR\reports\zooteh_nofuck.rdl
    Delete /REBOOTOK $INSTDIR\reports\zooteh.rdl
    Delete /REBOOTOK $INSTDIR\reports\shed.rdl
    Delete /REBOOTOK $INSTDIR\reports\replace_plan.rdl
    Delete /REBOOTOK $INSTDIR\reports\realization.rdl
    Delete /REBOOTOK $INSTDIR\reports\rabbit.rdl
    Delete /REBOOTOK $INSTDIR\reports\plem.rdl
    Delete /REBOOTOK $INSTDIR\reports\okrol_user.rdl
    Delete /REBOOTOK $INSTDIR\reports\fucks_by_date.rdl
    Delete /REBOOTOK $INSTDIR\reports\fucker.rdl
    Delete /REBOOTOK $INSTDIR\reports\empty_rev.rdl
    Delete /REBOOTOK $INSTDIR\reports\deadreason.rdl
    Delete /REBOOTOK $INSTDIR\reports\dead.rdl
    Delete /REBOOTOK $INSTDIR\reports\by_month.rdl
    Delete /REBOOTOK $INSTDIR\reports\breeds.rdl
    Delete /REBOOTOK $INSTDIR\reports\age.rdl
    RmDir /REBOOTOK $INSTDIR/reports
    Delete /REBOOTOK $INSTDIR\RdlViewer.dll
    Delete /REBOOTOK $INSTDIR\RdlEngine.dll
    Delete /REBOOTOK $INSTDIR\rabnet.exe
    Delete /REBOOTOK $INSTDIR\rabHelp.chm
    Delete /REBOOTOK $INSTDIR\Pickers.dll
    Delete /REBOOTOK $INSTDIR\MySql.Data.dll
    Delete /REBOOTOK $INSTDIR\engine.dll
    Delete /REBOOTOK $INSTDIR\db.mysql.dll
    DeleteRegValue HKLM "${REGKEY}\Components" "rabnet"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(SM_Prog_NAME).lnk"
SectionEnd

Section -un.post UNSEC_Sys
    DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk"
    Delete /REBOOTOK $INSTDIR\uninstall.exe
    DeleteRegValue HKLM "${REGKEY}" Path
    DeleteRegKey /IfEmpty HKLM "${REGKEY}\Components"
    DeleteRegKey /IfEmpty HKLM "${REGKEY}"
    RmDir /r /REBOOTOK $SMPROGRAMS\$StartMenuGroup
    RmDir /REBOOTOK $INSTDIR
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


    !insertmacro SELECT_SECTION_TEST
    

;    Push "AutoUpdate"         ; push the search string onto the stack
;    Push "DefaultValue"   ; push a default value onto the stack
;    Call GetParameterValue
;    Pop $2
;    MessageBox MB_OK "Value of OUTPUT parameter is '$2'"

    
FunctionEnd

# Uninstaller functions
Function un.onInit
    ReadRegStr $INSTDIR HKLM "${REGKEY}" Path
    StrCpy $StartMenuGroup $(SM_Prog_NAME)
    !insertmacro SELECT_UNSECTION "rabnet" ${UNSEC_Rabnet}
    !insertmacro SELECT_UNSECTION "rabdump" ${UNSEC_RabDump}
    !insertmacro SELECT_UNSECTION "com_comps" ${UNSEC_Common}
FunctionEnd

# Section Descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_Rabnet} $(SEC_Rabnet_DESC)
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_RabDump} $(SEC_RabDump_DESC)
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_Mysql} $(SEC_Mysql_DESC)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

# Installer Language Strings
# TODO Update the Language Strings with the appropriate translations.

LangString ^UninstallLink ${LANG_ENGLISH} "Uninstall $(^Name)"
LangString ^UninstallLink ${LANG_RUSSIAN} "�������� $(^Name)"

LangString SEC_Rabnet_DESC ${LANG_ENGLISH} "It is installed on computers in the network that are working livestock"
LangString SEC_Rabnet_DESC ${LANG_RUSSIAN} "��������������� �� ���������� � ���� � �������� �������� ����������"

LangString SEC_RabDump_DESC ${LANG_ENGLISH} "It is installed on the server that stores and runs the database"
LangString SEC_RabDump_DESC ${LANG_RUSSIAN} "��������������� �� ������ �� ������� �������� � �������� ���� ������"

LangString SEC_Mysql_DESC ${LANG_ENGLISH} "Database. It is installed on a Miakro Rabnet server"
LangString SEC_Mysql_DESC ${LANG_RUSSIAN} "���� ������. ��������������� �� ������ ������������� ������ Rabnet"

LangString SEC_Rabnet_NAME ${LANG_ENGLISH} "Rabnet client"
LangString SEC_Rabnet_NAME ${LANG_RUSSIAN} "Rabnet ���������� �����"

LangString SEC_RabDump_NAME ${LANG_ENGLISH} "Rabnet server tools"
LangString SEC_RabDump_NAME ${LANG_RUSSIAN} "Rabnet ��������� ����������"

LangString SEC_Mysql_NAME ${LANG_ENGLISH} "MySQL Server 5.1"
LangString SEC_Mysql_NAME ${LANG_RUSSIAN} "MySQL Server 5.1"

LangString SEC_PackClient_NAME ${LANG_ENGLISH} "Working PC"
LangString SEC_PackClient_NAME ${LANG_RUSSIAN} "������� ���������"

LangString SEC_PackServer_NAME ${LANG_ENGLISH} "Server"
LangString SEC_PackServer_NAME ${LANG_RUSSIAN} "������"

LangString SEC_PackServerFull_NAME ${LANG_ENGLISH} "Server Full (+MySQL)"
LangString SEC_PackServerFull_NAME ${LANG_RUSSIAN} "������ ������ (+MySQL)"

LangString MYSQLINSTALLER_Start ${LANG_ENGLISH} "Installing MySQL..."
LangString MYSQLINSTALLER_Start ${LANG_RUSSIAN} "��������� MySQL..."

LangString MYSQLINSTALLER_Done ${LANG_ENGLISH} "MySQL installed successfully"
LangString MYSQLINSTALLER_Done ${LANG_RUSSIAN} "MySQL ����������"

LangString MYSQLINSTALLER_Configure ${LANG_ENGLISH} "Configuring MySQL..."
LangString MYSQLINSTALLER_Configure ${LANG_RUSSIAN} "��������� MySQL..."

LangString UPDATER_Run ${LANG_ENGLISH} "Starting Updater..."
LangString UPDATER_Run ${LANG_RUSSIAN} "������ Updater..."

LangString Prog_NAME ${LANG_ENGLISH} "Miakro Rabnet"
LangString Prog_NAME ${LANG_RUSSIAN} "������ Rabnet"

LangString SM_Prog_NAME ${LANG_ENGLISH} "Miakro Rabnet"
LangString SM_Prog_NAME ${LANG_RUSSIAN} "������ Rabnet"

LangString SM_Dump_NAME ${LANG_ENGLISH} "RabDump"
LangString SM_Dump_NAME ${LANG_RUSSIAN} "��������� �����"


VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "@AppName_en@"
#VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "A test comment"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "@CompanyName_en@"
#VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" "Test Application is a trademark of Fake company"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "@Copys_en@"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "@FileDescr_en@"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "@AppVer@"

VIAddVersionKey /LANG=${LANG_RUSSIAN} "ProductName" "@AppName@"
#VIAddVersionKey /LANG=${LANG_RUSSIAN} "Comments" "A test comment"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "CompanyName" "@CompanyName@"
#VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalTrademarks" "Test Application is a trademark of Fake company"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalCopyright" "@Copys@"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "FileDescription" "@FileDescr@"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "FileVersion" "@AppVer@"

VIProductVersion "@AppVer@"


 ; GetParameters
 ; input, none
 ; output, top of stack (replaces, with e.g. whatever)
 ; modifies no other variables.
 
Function GetParameters
 
  Push $R0
  Push $R1
  Push $R2
  Push $R3
 
  StrCpy $R2 1
  StrLen $R3 $CMDLINE
 
  ;Check for quote or space
  StrCpy $R0 $CMDLINE $R2
  StrCmp $R0 '"' 0 +3
    StrCpy $R1 '"'
    Goto loop
  StrCpy $R1 " "
 
  loop:
    IntOp $R2 $R2 + 1
    StrCpy $R0 $CMDLINE 1 $R2
    StrCmp $R0 $R1 get
    StrCmp $R2 $R3 get
    Goto loop
 
  get:
    IntOp $R2 $R2 + 1
    StrCpy $R0 $CMDLINE 1 $R2
    StrCmp $R0 " " get
    StrCpy $R0 $CMDLINE "" $R2
 
  Pop $R3
  Pop $R2
  Pop $R1
  Exch $R0
 
FunctionEnd



; GetParameterValue
; Chris Morgan<cmorgan@alum.wpi.edu> 5/10/2004
; -Updated 4/7/2005 to add support for retrieving a command line switch
;  and additional documentation
;
; Searches the command line input, retrieved using GetParameters, for the
; value of an option given the option name.  If no option is found the
; default value is placed on the top of the stack upon function return.
;
; This function can also be used to detect the existence of just a
; command line switch like /OUTPUT  Pass the default and "OUTPUT"
; on the stack like normal.  An empty return string "" will indicate
; that the switch was found, the default value indicates that
; neither a parameter or switch was found.
;
; Inputs - Top of stack is default if parameter isn't found,
;  second in stack is parameter to search for, ex. "OUTPUT"
; Outputs - Top of the stack contains the value of this parameter
;  So if the command line contained /OUTPUT=somedirectory, "somedirectory"
;  will be on the top of the stack when this function returns
;
; Register usage
;$R0 - default return value if the parameter isn't found
;$R1 - input parameter, for example OUTPUT from the above example
;$R2 - the length of the search, this is the search parameter+2
;      as we have '/OUTPUT='
;$R3 - the command line string
;$R4 - result from StrStr calls
;$R5 - search for ' ' or '"'
 
Function GetParameterValue
  Exch $R0  ; get the top of the stack(default parameter) into R0
  Exch      ; exchange the top of the stack(default) with
            ; the second in the stack(parameter to search for)
  Exch $R1  ; get the top of the stack(search parameter) into $R1
 
  ;Preserve on the stack the registers used in this function
  Push $R2
  Push $R3
  Push $R4
  Push $R5
 
  Strlen $R2 $R1+2    ; store the length of the search string into R2
 
  Call GetParameters  ; get the command line parameters
  Pop $R3             ; store the command line string in R3
 
  # search for quoted search string
  StrCpy $R5 '"'      ; later on we want to search for a open quote
  Push $R3            ; push the 'search in' string onto the stack
  Push '"/$R1='       ; push the 'search for'
  Call StrStr         ; search for the quoted parameter value
  Pop $R4
  StrCpy $R4 $R4 "" 1   ; skip over open quote character, "" means no maxlen
  StrCmp $R4 "" "" next ; if we didn't find an empty string go to next
 
  # search for non-quoted search string
  StrCpy $R5 ' '      ; later on we want to search for a space since we
                      ; didn't start with an open quote '"' we shouldn't
                      ; look for a close quote '"'
  Push $R3            ; push the command line back on the stack for searching
  Push '/$R1='        ; search for the non-quoted search string
  Call StrStr
  Pop $R4
 
  ; $R4 now contains the parameter string starting at the search string,
  ; if it was found
next:
  StrCmp $R4 "" check_for_switch ; if we didn't find anything then look for
                                 ; usage as a command line switch
  # copy the value after /$R1= by using StrCpy with an offset of $R2,
  # the length of '/OUTPUT='
  StrCpy $R0 $R4 "" $R2  ; copy commandline text beyond parameter into $R0
  # search for the next parameter so we can trim this extra text off
  Push $R0
  Push $R5            ; search for either the first space ' ', or the first
                      ; quote '"'
                      ; if we found '"/output' then we want to find the
                      ; ending ", as in '"/output=somevalue"'
                      ; if we found '/output' then we want to find the first
                      ; space after '/output=somevalue'
  Call StrStr         ; search for the next parameter
  Pop $R4
  StrCmp $R4 "" done  ; if 'somevalue' is missing, we are done
  StrLen $R4 $R4      ; get the length of 'somevalue' so we can copy this
                      ; text into our output buffer
  StrCpy $R0 $R0 -$R4 ; using the length of the string beyond the value,
                      ; copy only the value into $R0
  goto done           ; if we are in the parameter retrieval path skip over
                      ; the check for a command line switch
 
; See if the parameter was specified as a command line switch, like '/output'
check_for_switch:
  Push $R3            ; push the command line back on the stack for searching
  Push '/$R1'         ; search for the non-quoted search string
  Call StrStr
  Pop $R4
  StrCmp $R4 "" done  ; if we didn't find anything then use the default
  StrCpy $R0 ""       ; otherwise copy in an empty string since we found the
                      ; parameter, just didn't find a value
 
done:
  Pop $R5
  Pop $R4
  Pop $R3
  Pop $R2
  Pop $R1
  Exch $R0 ; put the value in $R0 at the top of the stack
FunctionEnd


!define StrStr "!insertmacro StrStr"
 
!macro StrStr ResultVar String SubString
  Push `${String}`
  Push `${SubString}`
  Call StrStr
  Pop `${ResultVar}`
!macroend
 
Function StrStr
/*After this point:
  ------------------------------------------
  $R0 = SubString (input)
  $R1 = String (input)
  $R2 = SubStringLen (temp)
  $R3 = StrLen (temp)
  $R4 = StartCharPos (temp)
  $R5 = TempStr (temp)*/
 
  ;Get input from user
  Exch $R0
  Exch
  Exch $R1
  Push $R2
  Push $R3
  Push $R4
  Push $R5
 
  ;Get "String" and "SubString" length
  StrLen $R2 $R0
  StrLen $R3 $R1
  ;Start "StartCharPos" counter
  StrCpy $R4 0
 
  ;Loop until "SubString" is found or "String" reaches its end
  loop:
    ;Remove everything before and after the searched part ("TempStr")
    StrCpy $R5 $R1 $R2 $R4
 
    ;Compare "TempStr" with "SubString"
    StrCmp $R5 $R0 done
    ;If not "SubString", this could be "String"'s end
    IntCmp $R4 $R3 done 0 done
    ;If not, continue the loop
    IntOp $R4 $R4 + 1
    Goto loop
  done:
 
/*After this point:
  ------------------------------------------
  $R0 = ResultVar (output)*/
 
  ;Remove part before "SubString" on "String" (if there has one)
  StrCpy $R0 $R1 `` $R4
 
  ;Return output to user
  Pop $R5
  Pop $R4
  Pop $R3
  Pop $R2
  Pop $R1
  Exch $R0
FunctionEnd


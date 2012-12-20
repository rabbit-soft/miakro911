Section -post SEC_Sys
    !insertmacro CheckDotNET
    WriteRegStr HKCU "${REGKEY}" Path $INSTDIR
    WriteRegStr HKCU "${REGKEY}\components" "Main" 1
    SetOutPath $INSTDIR
    WriteUninstaller $INSTDIR\uninstall.exe
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk" $INSTDIR\uninstall.exe
    WriteRegStr HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayName "$(^Name)"
    WriteRegStr HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayIcon $INSTDIR\uninstall.exe
    WriteRegStr HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" UninstallString $INSTDIR\uninstall.exe
    WriteRegDWORD HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoModify 1
    WriteRegDWORD HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoRepair 1

    DeleteRegKey HKCU "${REGKEY_old}"
SectionEnd

Function InstBothReqFiles	
	File ${BinDir}\bin\MySql.Data.dll
	File ${BinDir}\bin\log4net.dll
	File ${BinDir}\bin\db.Interface.dll
	File ${BinDir}\bin\db.mysql.dll
	File ${BinDir}\bin\engine.dll
FunctionEnd

Function InstCommonFiles
	SetOutPath $INSTDIR\bin
    File ${BinDir}\bin\mia_conv.exe
	File ${BinDir}\bin\mia_conv.exe.config
    File ${BinDir}\bin\MySql.Data.dll
    File ${BinDir}\bin\updater.exe
	File ${BinDir}\bin\updater.exe.config	
	File ${BinDir}\bin\log4net.dll
FunctionEnd

Function CloseRabNet
    Push $5

    push "rabnet.exe"
    processwork::existsprocess
    pop $5
    IntCmp $5 0 done

    push "rabnet.exe"
    processwork::CloseProcess
;    processwork::KillProcess
    Sleep 5000

    loop:
        push "rabnet.exe"
        processwork::existsprocess
        pop $5
        IntCmp $5 0 done

        DetailPrint $(KillRabNet)

        push "rabnet.exe"
;        processwork::CloseProcess
        processwork::KillProcess
        Sleep 2000
        Goto loop

    BailOut:
        Abort

    done:
    Pop $5

FunctionEnd

Function un.CloseRabNet
    Push $5

    push "rabnet.exe"
    processwork::existsprocess
    pop $5
    IntCmp $5 0 done

    loop:
        push "rabnet.exe"
        processwork::existsprocess
        pop $5
        IntCmp $5 0 done

        DetailPrint $(KillRabNet)

        push "rabnet.exe"
;        processwork::CloseProcess
        processwork::KillProcess
        Sleep 2000
        Goto loop

    BailOut:
        Abort

    done:
    Pop $5

FunctionEnd
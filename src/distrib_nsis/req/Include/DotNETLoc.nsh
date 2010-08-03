# DotNET version checking macro.
# Written by AnarkiNet(AnarkiNet@gmail.com)
# Edited by X-plora (the.xplora@gmail.com)
# Detects the Microsoft .NET Framework version 2.0 Redistributable and runs it if the user does not have the correct version.

!include WordFunc.nsh
!include LogicLib.nsh
!insertmacro VersionCompare

LangString DOTNETINSTALLER_Check ${LANG_ENGLISH} "Checking your .NET Framework..."
LangString DOTNETINSTALLER_Check ${LANG_RUSSIAN} "Проверяем версию .NET Framework..."

LangString DOTNETINSTALLER_No ${LANG_ENGLISH} ".NET Framework not found"
LangString DOTNETINSTALLER_No ${LANG_RUSSIAN} ".NET Framework не найден"

LangString DOTNETINSTALLER_WrongVer1 ${LANG_ENGLISH} ".NET Framework Version found"
LangString DOTNETINSTALLER_WrongVer1 ${LANG_RUSSIAN} "Найдена версия .NET Framework"

LangString DOTNETINSTALLER_WrongVer2 ${LANG_ENGLISH} "but is not up to the required version"
LangString DOTNETINSTALLER_WrongVer2 ${LANG_RUSSIAN} "но она не подходит вместо ожидаемой версии"

LangString DOTNETINSTALLER_Pause ${LANG_ENGLISH} "Pausing installation while downloaded .NET Framework installer runs."
LangString DOTNETINSTALLER_Pause ${LANG_RUSSIAN} "Приостанавливаем установку пока идет установка .NET Framework."

LangString DOTNETINSTALLER_Complete ${LANG_ENGLISH} "Completed .NET Framework install."
LangString DOTNETINSTALLER_Complete ${LANG_RUSSIAN} "Устновка .NET Framework завершена."

LangString DOTNETINSTALLER_Up2Date ${LANG_ENGLISH} ".NET Framework found and is up to date..."
LangString DOTNETINSTALLER_Up2Date ${LANG_RUSSIAN} ".NET Framework найден и актуален..."



!macro CheckDotNET

#!insertmacro MUI_LANGUAGE English
#!insertmacro MUI_LANGUAGE Russian

#!define DOTNET_URL "http://www.microsoft.com/downloads/info.aspx?na=90&p=&SrcDisplayLang=en&SrcCategoryId=&SrcFamilyId=0856EACB-4362-4B0D-8EDD-AAB15C5E04F5&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f5%2f6%2f7%2f567758a3-759e-473e-bf8f-52154438565a%2fdotnetfx.exe"
!define DOTNET_VERSION "2.0" ; minimum .net runtime version

DetailPrint $(DOTNETINSTALLER_Check)
StrCpy $8 ""
CheckBegin:
	Push $0
	Push $1
	 
	System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1"
	StrCmp $1 0 +2
		StrCpy $0 "none"
	 
	Pop $1
	Exch $0
	Pop $0
	${If} $0 == "none"
		DetailPrint $(DOTNETINSTALLER_No)
	  ${If} $8 == ""
			Goto InstallDotNet
		${Else}
			Goto InvalidDotNetAfterInstall
		${EndIf}
	${EndIf}
	 
	StrCpy $0 $0 "" 1 # skip "v"
	${VersionCompare} $0 ${DOTNET_VERSION} $1
	${If} $1 == 2
		DetailPrint "$(DOTNETINSTALLER_WrongVer1): $0, $(DOTNETINSTALLER_WrongVer2): ${DOTNET_VERSION}"
	  ${If} $8 == ""
			Goto InstallDotNet
		${Else}
			Goto InvalidDotNetAfterInstall
		${EndIf}
	${EndIf}
	Goto ValidDotNET

InstallDotNet:
	DetailPrint $(DOTNETINSTALLER_Pause)
	NsisDotNetInstaller::LaunchInstaller "$EXEDIR\dotnetfx20\dotnetfx.exe"
	DetailPrint $(DOTNETINSTALLER_Complete)
#	Delete "$TEMP\dotnetfx.exe"
#	DetailPrint "Completed cleaning temporary files.  Verifying install..."
	StrCpy $8 "AfterInstall"
	Goto CheckBegin
	
InvalidDotNetAfterInstall:
	
ValidDotNET:
	DetailPrint $(DOTNETINSTALLER_Up2Date)
!macroend
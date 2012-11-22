# Macro for selecting uninstaller sections
!macro SELECT_UNSECTION_old SECTION_NAME UNSECTION_ID
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY_old}\Components" "${SECTION_NAME}"
    StrCmp $R0 1 0 nextold${UNSECTION_ID}
    !insertmacro SelectSection "${UNSECTION_ID}"
    GoTo doneold${UNSECTION_ID}
nextold${UNSECTION_ID}:
    !insertmacro UnselectSection "${UNSECTION_ID}"
doneold${UNSECTION_ID}:
    Pop $R0
!macroend

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

!macro SELECT_SECTION_old SECTION_NAME SECTION_ID
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY_old}\Components" "${SECTION_NAME}"
    StrCmp $R0 1 0 nextold${SECTION_ID}
    !insertmacro SelectSection "${SECTION_ID}"
    GoTo doneold${SECTION_ID}
nextold${SECTION_ID}:
    !insertmacro UnselectSection "${SECTION_ID}"
doneold${SECTION_ID}:
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

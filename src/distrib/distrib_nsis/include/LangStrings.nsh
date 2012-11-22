# Installer Language Strings
# TODO Update the Language Strings with the appropriate translations.

LangString ^UninstallLink ${LANG_ENGLISH} "Uninstall $(^Name)"
LangString ^UninstallLink ${LANG_RUSSIAN} "�������� $(^Name)"

LangString SEC_Rabnet_DESC ${LANG_ENGLISH} "It is installed on computers in the network that are working livestock"
LangString SEC_Rabnet_DESC ${LANG_RUSSIAN} "��������������� �� ���������� � ���� � �������� �������� ����������"

LangString SEC_RabDump_DESC ${LANG_ENGLISH} "It is installed on the server that stores and runs the database"
LangString SEC_RabDump_DESC ${LANG_RUSSIAN} "��������������� �� ������ �� ������� �������� � �������� ���� ������"

LangString SEC_Mysql_DESC ${LANG_ENGLISH} "Database. It is installed on the Miakro-9.11 server"
LangString SEC_Mysql_DESC ${LANG_RUSSIAN} "���� ������. ��������������� �� ������ ������������� ������-9.11"

LangString SEC_Rabnet_NAME ${LANG_ENGLISH} "Miakro-9.11 client"
LangString SEC_Rabnet_NAME ${LANG_RUSSIAN} "������-9.11 ���������� �����"

LangString SEC_RabDump_NAME ${LANG_ENGLISH} "Miakro-9.11 server tools"
LangString SEC_RabDump_NAME ${LANG_RUSSIAN} "������-9.11 ��������� ����������"

LangString SEC_Mysql_NAME ${LANG_ENGLISH} "MySQL Server 5.1"
LangString SEC_Mysql_NAME ${LANG_RUSSIAN} "MySQL Server 5.1"

LangString SEC_PackClient_NAME ${LANG_ENGLISH} "Working PC"
LangString SEC_PackClient_NAME ${LANG_RUSSIAN} "������� ���������"

LangString SEC_PackServer_NAME ${LANG_ENGLISH} "Server"
LangString SEC_PackServer_NAME ${LANG_RUSSIAN} "������"

LangString SEC_PackServerFull_NAME ${LANG_ENGLISH} "Server Full (+MySQL)"
LangString SEC_PackServerFull_NAME ${LANG_RUSSIAN} "������ ������ (+MySQL)"

LangString SEC_PackFull_NAME ${LANG_ENGLISH} "Full (Client+Server+MySQL)"
LangString SEC_PackFull_NAME ${LANG_RUSSIAN} "������ ����� (������+������+MySQL)"

LangString MYSQLINSTALLER_Start ${LANG_ENGLISH} "Installing MySQL..."
LangString MYSQLINSTALLER_Start ${LANG_RUSSIAN} "��������� MySQL..."

LangString MYSQLINSTALLER_Done ${LANG_ENGLISH} "MySQL installed successfully"
LangString MYSQLINSTALLER_Done ${LANG_RUSSIAN} "MySQL ����������"

LangString MYSQLINSTALLER_Configure ${LANG_ENGLISH} "Configuring MySQL..."
LangString MYSQLINSTALLER_Configure ${LANG_RUSSIAN} "��������� MySQL..."

LangString UPDATER_Run ${LANG_ENGLISH} "Starting Updater..."
LangString UPDATER_Run ${LANG_RUSSIAN} "������ Updater..."

LangString Prog_NAME ${LANG_ENGLISH} "Miakro-9.11 (1.0.7.1168)"
LangString Prog_NAME ${LANG_RUSSIAN} "������-9.11 (1.0.7.1168)"

LangString SM_Prog_NAME ${LANG_ENGLISH} "Miakro-9.11"
LangString SM_Prog_NAME ${LANG_RUSSIAN} "������-9.11"

LangString SM_Dump_NAME ${LANG_ENGLISH} "RabDump"
LangString SM_Dump_NAME ${LANG_RUSSIAN} "��������� �����"

LangString SM_Conv_NAME ${LANG_ENGLISH} ".mia Converter"
LangString SM_Conv_NAME ${LANG_RUSSIAN} "��������� �� .mia"

LangString SM_Up_NAME ${LANG_ENGLISH} "DB Updater"
LangString SM_Up_NAME ${LANG_RUSSIAN} "���������� ��"

LangString KillRabDump ${LANG_ENGLISH} "Killing RabDump..."
LangString KillRabDump ${LANG_RUSSIAN} "��������� ��������� �����..."

LangString KillRabNet ${LANG_ENGLISH} "Killing RabNet..."
LangString KillRabNet ${LANG_RUSSIAN} "��������� Rabnet..."

LangString Branding ${LANG_ENGLISH} "9-Bits"
LangString Branding ${LANG_RUSSIAN} "9-���"


VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "Miakro-9.11"
#VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "A test comment"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "9-Bits"
#VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" "Test Application is a trademark of Fake company"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "(C) 9-Bits 2010"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Miakro-9.11 1.0 Installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "1.0.7.1168"

VIAddVersionKey /LANG=${LANG_RUSSIAN} "ProductName" "������-9.11"
#VIAddVersionKey /LANG=${LANG_RUSSIAN} "Comments" "A test comment"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "CompanyName" "9-���"
#VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalTrademarks" "Test Application is a trademark of Fake company"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "LegalCopyright" "(C) 9-��� 2010"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "FileDescription" "��������� ��������� ������-9.11 1.0"
VIAddVersionKey /LANG=${LANG_RUSSIAN} "FileVersion" "1.0.7.1168"

VIProductVersion "1.0.7.1168"
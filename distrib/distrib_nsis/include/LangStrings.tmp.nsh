# Installer Language Strings
# TODO Update the Language Strings with the appropriate translations.

LangString ^UninstallLink ${LANG_ENGLISH} "Uninstall $(^Name)"
LangString ^UninstallLink ${LANG_RUSSIAN} "Удаление $(^Name)"

LangString SEC_Rabnet_DESC ${LANG_ENGLISH} "It is installed on computers in the network that are working livestock"
LangString SEC_Rabnet_DESC ${LANG_RUSSIAN} "Устанавливается на компьютеры в сети с которыми работают зоотехники"

LangString SEC_RabDump_DESC ${LANG_ENGLISH} "It is installed on the server that stores and runs the database"
LangString SEC_RabDump_DESC ${LANG_RUSSIAN} "Устанавливается на сервер на котором хранится и работает база данных"

LangString SEC_Mysql_DESC ${LANG_ENGLISH} "Database. It is installed on the Miakro-9.11 server"
LangString SEC_Mysql_DESC ${LANG_RUSSIAN} "База данных. Устанавливается на сервер обслуживающий Миакро-9.11"

LangString SEC_Rabnet_NAME ${LANG_ENGLISH} "Miakro-9.11 client"
LangString SEC_Rabnet_NAME ${LANG_RUSSIAN} "Миакро-9.11 клиентская часть"

LangString SEC_RabDump_NAME ${LANG_ENGLISH} "Miakro-9.11 server tools"
LangString SEC_RabDump_NAME ${LANG_RUSSIAN} "Миакро-9.11 серверные приложения"

LangString SEC_Mysql_NAME ${LANG_ENGLISH} "MySQL Server 5.1"
LangString SEC_Mysql_NAME ${LANG_RUSSIAN} "MySQL Server 5.1"

LangString SEC_PackClient_NAME ${LANG_ENGLISH} "Working PC"
LangString SEC_PackClient_NAME ${LANG_RUSSIAN} "Рабочий компьютер"

LangString SEC_PackServer_NAME ${LANG_ENGLISH} "Server"
LangString SEC_PackServer_NAME ${LANG_RUSSIAN} "Сервер"

LangString SEC_PackServerFull_NAME ${LANG_ENGLISH} "Server Full (+MySQL)"
LangString SEC_PackServerFull_NAME ${LANG_RUSSIAN} "Сервер Полный (+MySQL)"

LangString SEC_PackFull_NAME ${LANG_ENGLISH} "Full (Client+Server+MySQL)"
LangString SEC_PackFull_NAME ${LANG_RUSSIAN} "Полный набор (Клиент+Сервер+MySQL)"

LangString MYSQLINSTALLER_Start ${LANG_ENGLISH} "Installing MySQL..."
LangString MYSQLINSTALLER_Start ${LANG_RUSSIAN} "Установка MySQL..."

LangString MYSQLINSTALLER_Done ${LANG_ENGLISH} "MySQL installed successfully"
LangString MYSQLINSTALLER_Done ${LANG_RUSSIAN} "MySQL установлен"

LangString MYSQLINSTALLER_Configure ${LANG_ENGLISH} "Configuring MySQL..."
LangString MYSQLINSTALLER_Configure ${LANG_RUSSIAN} "Настройка MySQL..."

LangString UPDATER_Run ${LANG_ENGLISH} "Starting Updater..."
LangString UPDATER_Run ${LANG_RUSSIAN} "Запуск Updater..."

LangString Prog_NAME ${LANG_ENGLISH} "Miakro-9.11 (@AppVer@)"
LangString Prog_NAME ${LANG_RUSSIAN} "Миакро-9.11 (@AppVer@)"

LangString SM_Prog_NAME ${LANG_ENGLISH} "Miakro-9.11"
LangString SM_Prog_NAME ${LANG_RUSSIAN} "Миакро-9.11"

LangString SM_Dump_NAME ${LANG_ENGLISH} "RabDump"
LangString SM_Dump_NAME ${LANG_RUSSIAN} "Резервные копии"

LangString SM_Conv_NAME ${LANG_ENGLISH} ".mia Converter"
LangString SM_Conv_NAME ${LANG_RUSSIAN} "Конвертер из .mia"

LangString SM_Up_NAME ${LANG_ENGLISH} "DB Updater"
LangString SM_Up_NAME ${LANG_RUSSIAN} "Обновление БД"

LangString KillRabDump ${LANG_ENGLISH} "Killing RabDump..."
LangString KillRabDump ${LANG_RUSSIAN} "Закрываем Резервные копии..."

LangString KillRabNet ${LANG_ENGLISH} "Killing RabNet..."
LangString KillRabNet ${LANG_RUSSIAN} "Закрываем Rabnet..."

LangString Branding ${LANG_ENGLISH} "@CompanyName_en@"
LangString Branding ${LANG_RUSSIAN} "@CompanyName@"


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
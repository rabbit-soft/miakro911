<?php

class Conf
{
    /**
     * Адрес сервера MySQL
     */
    public static $DB_HOST="localhost";
    public static $DB_USER= "pay44ne";
    public static $DB_PWD = "vtufdjym";
    public static $LOG_QRS =true;
    public static $UPDATE_PATH="/usr/";    //где находится новая программа

    /**
     * @var array Файлы которые надо обновлять
     */
    /*public static $UPDATE_FILES = array("ccxmlrpc.dll",
        "log4net.dll",
        "Microsoft.Office.Interop.Excel.dll",
        "Microsoft.Office.Interop.Word.dll",
        "pAdmin.exe",
        "pEngine.dll",
        "tva.dll"
    );*/
}

/*
    define("DB_HOST", "localhost");
    define("DB_USER", "pay44ne");
    define("DB_PWD", "vtufdjym");
    define("LOG_QRS",true);    //логировать запросы к БД
	define("UPDATE_PATH","/usr/");    //где находится новая программа
*/
?>
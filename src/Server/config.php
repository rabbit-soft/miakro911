<?php
/*
 * Содержит настройки сайта
 * Должен быть включен в файле index.php
 */
class Conf
{
    /**
     * Адрес сервера MySQL
     */
    public static $DB_HOST= "localhost";
    public static $DB_USER= "pay44ne";
    public static $DB_PWD = "vtufdjym";
    public static $DB_NAME = "rabserv";
    public static $LOG_QRS =true;
    public static $UPDATE_PATH="/usr/";    //где находится новая программа

}

?>
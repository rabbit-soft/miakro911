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
    public static $DB_HOST= "192.168.0.95";
    public static $DB_USER= "pay44ne";
    public static $DB_PWD = "vtufdjym";
    public static $DB_NAME = "rabserv";
    public static $LOG_QRS =true;
    public static $UPDATE_PATH="/usr/";
    public static $DONG_UPDATE_HOST = '192.168.0.115'; //где находится новая программа
    public static $DUMPS_DIR = './dumps/';
}

?>
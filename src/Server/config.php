<?php
/*
 * Содержит настройки сайта
 * Должен быть включен в файле forrpc.php
 */
class Conf
{
    /**
     * Адрес сервера MySQL
     */
    public static $DB_HOST= "localhost";
    public static $DB_USER= "root";
    public static $DB_PWD = "";
    public static $DB_NAME = "rabserv";
    public static $LOG_QRS =true;
    public static $UPDATE_PATH="_update/";
    public static $DONG_UPDATE_HOST = '192.168.0.115'; //где находится новая программа
    public static $DUMPS_DIR = './dumps/';
}

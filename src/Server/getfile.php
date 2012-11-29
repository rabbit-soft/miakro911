<?php
require_once "config.php";
require_once "log4php/Logger.php";

Logger::configure('log4php.xml');
$log = Logger::getLogger("getfile");
//$log->debug("FILE CONNECT");
//$log->debug("GET FILE \n".var_export($_POST,true));
//var_dump($_POST); exit();
if(isset($_POST['getfile']) && file_exists(Conf::$UPDATE_PATH.$_POST['getfile']))
{
    $path = Conf::$UPDATE_PATH.$_POST['getfile'];

    header("Content-type: application/octet-stream");
    header("Content-disposition: attachment;filename=".$_POST["getfile"]);
    //header("Content-length:".filesize($path));
    //header("Content-MD5:".md5_file($path));
    //$log->debug($path);
    $rawdata = file_get_contents($path);
    $rawdata = gzdeflate($rawdata);
    exit($rawdata);
}

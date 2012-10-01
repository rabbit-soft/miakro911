<?php
include_once 'config.php';
include_once 'log4php/Logger.php';

Logger::configure('log4php.xml');
$log = Logger::getLogger("getdump");

$filepath= Conf::$DUMPS_DIR.$_POST["clientId"].'/'.$_POST["file"];   $log->debug($filepath);
if(!file_exists($filepath))
{
    $log->error('dumpFile not exists: '.$filepath);
    exit();
}

$filesize = filesize($filepath);
if(!isset($_POST["offset"]))
    $offset = 0;
else
    $offset = $_POST["offset"];
ob_clean();
header("Content-type: application/octet-stream");
header("Content-disposition: attachment;filename=".$_POST["file"]);
header("Content-length:".($filesize-$offset));
header("Content-MD5:".md5_file($filepath));

echo file_get_contents($filepath,null,null,$offset);
?>
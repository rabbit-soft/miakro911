<?php
include_once 'ServFunc.php';
include_once 'log4php/Logger.php';
include_once 'gamlib/helper.php';
include_once 'config.php';

/**
 * @param string $path - Путь к полученной резервной копии.
 * @return md5 dump-файла
 */
function md5dump($path)
{
	exec("7z x -pns471lbNITfq3 ".$path);
	$path = basename_my($path,".7z").".dump";
	$res = md5_file($path);
	unlink($path);
	return $res;
}
/**
 * Проверяет существование папки, если нет, то создает ее.
 * @param string $path - Путь к папке.
 */
function checkDir($path)
{
	if(!file_exists($path) or !is_dir($path))
	{
		mkdir($path);
	}
}
;
Logger::configure('log4php.xml');
$log = Logger::getLogger("uploader");
$log->info('dump Upload '.$_FILES["file"]["name"]);
$log->debug(var_export($_POST,true));
$log->debug(var_export($_FILES,true));

$target_path = Conf::$DUMPS_DIR;
checkDir($target_path);
$target_path = $target_path . $_POST["clientId"]."/";
checkDir($target_path);
$target_path = $target_path . $_FILES["file"]["name"];
if(move_uploaded_file($_FILES["file"]["tmp_name"], $target_path))
{
    /*$md5dump = md5dump($target_path); //todo 7z
    $log->debug($md5dump ." ". $_GET['md5dump']);
    if($md5dump != $_GET['md5dump'])
    {
        delete($target_path);
        exit("2");
    }*/
    $md5dump = $_POST['md5dump'];
    ServFunc::AddDump($_POST["clientId"], $_FILES["file"]["name"], $md5dump);
	echo "0";
} 
else
    echo "1";

?>
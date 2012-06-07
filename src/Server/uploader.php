<?php
include 'ServFunc.php';
/**
 * Enter description here ...
 * @param string $path - Путь к полученной резервной копии.
 * @return md5 dump-файла
 */
function md5dump($path)
{
	exec("7z x -pns471lbNITfq3 ".$path);
	$path = basename($path,".7z").".dump";
	if(!file_exists($path))
	{
		
	}
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

/**
 * Функция добавления новой резервной копии
 * @param string $farmname - Название фермы, которое записано в ключе
 * @param string $filepath - Путь к файлу.
 */
/*function addNewDump($farmname,$filepath)
{
	$path = "dumps.xml";
	if(!file_exists($path))
	{
		$file = fopen($path, "c");
		fwrite($file,"<?xml version=\"1.0\" encoding=\"UTF-8\"?><dumps/>");
		fclose($file);
	}
	$file = fopen($path, "r");
	$cont = fread($file,filesize($path));
	$xml =simplexml_load_string($cont);
	fclose($file);
	
	$maxind = 0;	
	foreach ($xml->dump as $a)
	{	
		if($maxind < $a['id']*1)
		{
			$maxind = $a['id']*1;
		}
	}
	$maxind = $maxind+1;
	$dmp=$xml->addChild("dump");
 	$dmp->addAttribute("id",$maxind);
	$dmp->addAttribute("farm",iconv("cp1251", "UTF-8", $farmname));
	$dmp->addAttribute("datetime",date("Y-m-d H:i"));
	$dmp->addAttribute("file",iconv("cp1251", "UTF-8", basename($filepath)));	
	$dmp->addAttribute("md5dump",md5dump($filepath));
	$file = fopen($path, "w");
	fwrite($file,$xml->asXML());
	fclose($file);
}*/


$target_path = "uploads/";	checkDir($target_path);
$target_path = $target_path . $_POST["farm"]."/"; checkDir($target_path);
$target_path = $target_path . $_POST["type"]."/"; checkDir($target_path);
$target_path = $target_path . basename( $_FILES["uploadedfile"]["name"]);
/*if(file_exists($target_path))
{
	exit("ok");
}*/
//print_r($_FILES["uploadedfile"]);
if(move_uploaded_file($_FILES["uploadedfile"]["tmp_name"], $target_path)) 
{
    //echo "The file ".  basename( $_FILES['uploadedfile']['name'])." has been uploaded";
	//echo "<br/> ".$_POST['farm'];
	
	//эту функцию можно заменить записью в БД
	//addNewDump($_POST["farm"],$target_path);
    ServFunc::AddDump($_POST["farm"], $target_path, md5dump($target_path));
	echo "[File Upload Successful]";
} 
else
{
    echo "[File Upload Error]";
}
?>
<?php
include 'DBworker.php';
echo DBworker::GetWebRep_LastDate($_POST["farm"],$_POST["db"]);
/*$NODATES = "nodates";
$MINDATE = "0001-01-01";
$dmp = "webrep_glob.xml";
if(!file_exists($dmp)) 
{
	echo $NODATES;
	exit();
}
$file = fopen($dmp,"r");
$cont = fread($file,filesize($dmp));
$xml = simplexml_load_string($cont);
fclose($file);
$maxdate = new Datetime($MINDATE);
foreach($xml->oneglobalday as $g)
{
	if($g["farmname"] == iconv("cp1251", "UTF-8",$_POST["farm"]) && $g["database"] == iconv("cp1251", "UTF-8",$_POST["db"]))
	{	
		$dt1 = new DateTime($g["date"]);
		if($dt1>$maxdate) $maxdate = $dt1;
	}
}
if($maxdate ==$MINDATE)
	echo $NODATES;
else echo $maxdate->format('Y-m-d H:i:s');
*/

?>
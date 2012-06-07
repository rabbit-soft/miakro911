<?php

include 'ServFunc.php';
echo ServFunc::GetDumpList($_POST["farm"]);

/*$dmp = "dumps.xml";
if(!file_exists($dmp)) 
{
	echo "<dumplist/>";
	exit();
}
$file = fopen($dmp,"r");
$cont = fread($file,filesize($dmp));
$xml = simplexml_load_string($cont);
fclose($file);
//$maxtime = strtotime("0001-01-01 00:00");
//$maxind =-1;
echo "<dumplist>";
for($i=0; $i<$xml->count(); $i++ )
{
	if($xml->dump[$i]["farm"] == iconv("cp1251", "UTF-8",$_POST["farm"]))
	{
		echo $xml->dump[$i]->asXML()."\n";
	}
}
echo "</dumplist>";
/*if($maxind != -1)
{
	echo "\n\nmaximum:\n".$xml->dump[$maxind]->asXML();
}*/


?>
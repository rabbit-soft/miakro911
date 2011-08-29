<?php
include 'DBworker.php';
/*function addRepot_Global($inxml)
{
	$_GFN = "webrep_glob.xml";
	if(!file_exists($_GFN))
	{
		$file = fopen($_GFN, "c");
		fwrite($file,"<?xml version=\"1.0\" encoding=\"UTF-8\"?><webrep_glob/>");		
		fclose($file);
	}
	$file = fopen($_GFN, "r");
	$cont = fread($file,filesize($_GFN));
	$xml =simplexml_load_string($cont);
	fclose($file);
	foreach($inxml->oneglobalday as $g)
	{
		$chld = $xml->addChild($g->getName());
		$chld->addAttribute("farmname",$_POST["farm"]);
		$chld->addAttribute("database",$inxml["database"]);
		foreach($g->attributes() as $nm => $vl)
		{
			$chld->addAttribute($nm,$vl);
		}	
	}
	$file = fopen($_GFN, "w");
	fwrite($file,$xml->asXML());
	fclose($file);
}


function parseReport($farmname,$xmltext)
{
	$xml =simplexml_load_string($xmltext);
	
	foreach($xml->global as $g) 
	{
		addRepot_Global($g); 
	}
	
}*/

DBworker::ParseWebReport($_POST["farm"],$_POST["report"]);
?>
<?php
/**
 * Преобразует строку в ассоциативный массив
 * @param string $params - строка типа "par1=val1;par2=val2;"
 * @return array
 */
function parseParams($params)
{
	if($params!="")
	{
		$pairs = rtrim($params,";");
		$pairs = explode(";",$params);
		$params = array();
		foreach ($pairs as $pair)
			if($pair !="")
			{				
				$pair = explode("=",$pair);
				$params[$pair[0]] = $pair[1];	
			}
	}
	return $params;
}

function vexit($v)
{
	exit(var_export($v,true));
}

function vecho($v,$message='')
{
	if($message !='')
		echo $message."\n";
	echo var_export($v,true)."\n\n";
}

?>
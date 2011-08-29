<?php

class Logger
{
	
	public static function WriteLine($line)
	{
		$file = fopen("log.txt", "a");
		fwrite($file,date("Y-m-d H:i").": ".$line."\n");
		fclose($file);
	}
}

?>
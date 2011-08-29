<?php
include 'logger.php';


class DBworker
{
	/*
	/// тут не инициализируются переменные автоматически как в C#
	private static $DB_HOST = "localhost";
	private static $DB_USER = "rabdump";
	private static $DB_PASS = "rabdump";
	private static $DB_DBNAME = "rabdump";
	*/
	
	private static function connect()
	{
		$res = mysql_connect("localhost","rabdump","rabdump");
		mysql_select_db("rabdump",$res);
		mysql_query("SET NAMES UTF8");
		return $res;
	}
	private static function disconnect($sql)
	{
		return mysql_close($sql);
	}
	
	/** 
	 * Получить название фермы, данные по которой может проссматривать пользователь
	 * @param string $user
	 * @param string $pass
	 */
	public static function GetUserFarm($user,$pass)
	{
		//$user = iconv("cp1251","UTF-8",$user);
		//$pass = iconv("cp1251","UTF-8",$pass);
		$result = "";
		$sql = DBworker::connect();
		if(!$sql) 
		{
			DBworker::disconnect($sql);
			return "";
		}
		$query = "SELECT COALESCE(farm,'') AS frm FROM users WHERE name='$user' AND pass=Password('$pass') LIMIT 1";
		$rd = mysql_query($query,$sql);
		if($rd)
		{
			$row = mysql_fetch_assoc($rd);
			$result = $row["frm"];
		}
		DBworker::disconnect($sql);
		return $result;
	}
	
	/**
	 * Возвращает XML со списком имеющихся Резервных Копий
	 * @param string $farmname - Название организации
	 * @return XML
	 */
	public static function GetDumpList($farmname)
	{
		$farmname = iconv("cp1251","UTF-8",$farmname);
		$xml = new SimpleXMLElement('<?xml version="1.0" encoding="UTF-8"?><dumplist/>');
		$sql = DBworker::connect();		
		$query = "SELECT farm, datetime,filename,md5dump FROM dumplist WHERE farm='$farmname'";
		$rd = mysql_query($query,$sql);
		if($rd)
		{
			mysql_fetch_assoc($rd);
			while ($row = mysql_fetch_assoc($rd))
			{
				$dmp = $xml->addChild("dump");				
				foreach ($row as $name =>$value)
					$dmp->addAttribute($name, $value);
			}
		}
		DBworker::disconnect($sql);
		return $xml->asXML();
	}
	
	/**	
	 * Добавляет запись в БД о полученой Резервной копии
	 * @param string $farm - Название организации
	 * @param string $filename - Имя файла РК
	 * @param string $md5dump - md5 dump-файла
	 */
	public static function AddDump($farm,$filename,$md5dump)
	{
		$farm = iconv("cp1251","UTF-8",$farm);
		$filename = iconv("cp1251","UTF-8",basename($filename));
		
		$sql = DBworker::connect();
		$query = "SELECT farm, filename, md5dump FROM dumplist 
				  WHERE farm='$farm' AND filename='$filename' AND md5dump='$md5dump'";
		$rd = mysql_query($query,$sql);
		if($rd) 
		{		
			if(mysql_fetch_array($rd))
			{
				Logger::WriteLine("AddDump: row is  exists");
				DBworker::disconnect($sql);
				return;
			}
		}
		$query = "INSERT INTO dumplist(datetime, farm, filename, md5dump) VALUES (NOW(),'$farm','$filename','$md5dump')";
		mysql_query($query,$sql);	
		DBworker::disconnect($sql);
	}

	/**
	 * Получает статистику по ферме
	 * @param unknown_type $farmname - Название организации
	 * @param unknown_type $db - Имя базыданных
	 * @return массив отсортированный по возрастанию:
	 */
	public static function GetWebRep_Global($farmname,$db)
	{
		$farmname = iconv("cp1251","UTF-8",$farmname);
		$sql = DBworker::connect();
		$query = "SELECT Date_Format(date,'%d.%m.%Y') AS dt , fucks, okrols, proholosts, born, 
					killed, deads, rabbits FROM globalreport WHERE farm='$farmname' AND `database`='$db' ORDER BY date ASC";
		$rd = mysql_query($query,$sql);
		$result = array();
		while($row = mysql_fetch_assoc($rd))		
			array_push($result, $row);			
		return $result;
		DBworker::disconnect($sql);
	}
	
	/**
	 * Возвращает список баз данных, по которым есть отчеты
	 * @param string $farmname
	 * @return array 
	 */
	public static function GetWebRep_DBs($farmname)
	{
		$result = array();
		$farmname = iconv("cp1251","UTF-8",$farmname);
		$sql = DBworker::connect();
		$query = "SELECT DISTINCT `database` FROM globalreport WHERE farm='$farmname'";
		$rd = mysql_query($query,$sql);
		while($row = mysql_fetch_assoc($rd))
			array_push($result, $row["database"]);	
		DBworker::disconnect($sql);
		return $result;
	}
	
	/**
	 * Получает дату последнего отчета
	 * @param string $farmname - Название организации
	 * @package string $db - имя БазыДанных
	 * @return string Дату либо "nodates"
	 */
	public static function GetWebRep_LastDate($farmname,$db)
	{
		$result = "nodates";
		$farmname = iconv("cp1251", "UTF-8", $farmname);
		$db = iconv("cp1251", "UTF-8", $db);
		$sql = DBworker::connect();
		$rd = mysql_query("SELECT Coalesce(Max(`date`),'$result') AS dt FROM globalReport 
				WHERE farm='$farmname' AND `database`='$db'",$sql);
		if($rd)
		{
			$row = mysql_fetch_assoc($rd);
			$result = $row["dt"];
		}
		DBworker::disconnect($sql);
		return $result;
	}
	
	/**
	 * Разбирает полученную от rabdump'а XML со статистикой
	 * @param string $farmname - Название организации
	 * @param string $xml_text - Текст XML-статистики
	 */
	public static function ParseWebReport($farmname,$xml_text)
	{
		$farmname = iconv("cp1251", "UTF-8", $farmname);
		$xml_text = stripcslashes($xml_text);					
		$xml = simplexml_load_string($xml_text);
		foreach ($xml->global as $g)
			DBworker::AddWebRep_Global($farmname,$g);
	}
	
	/**
	 * Добавляет в БД Глобальный отчет
	 * @param string $farmname - Название организации
	 * @param simpleXMLElement $xml
	 */
	private static function AddWebRep_Global($farmname,$xml)
	{
		$sql = DBworker::connect();
		$database = $xml["database"];
		foreach ($xml->oneglobalday as $gd)
		{
			$query = "INSERT INTO globalReport( farm, `database`, `date`, fucks, proholosts, born, killed, deads, rabbits) 
						VALUES('$farmname','$database','".$gd["date"]."',".$gd["fucks"].",".$gd["proholosts"].",
						".$gd["born"].",".$gd["killed"].",".$gd["deads"].",".$gd["rabbits"].")";
			Logger::WriteLine($query);
			mysql_query($query,$sql);
		}
		DBworker::disconnect($sql);
	}
	
	
	
	
}
?>
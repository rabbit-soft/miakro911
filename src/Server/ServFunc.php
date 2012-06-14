<?php
include_once "log4php/Logger.php";
include_once 'gamlib/DBworker.php';
include_once 'config.php';

class ServFunc
{
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
        $sql = DBworker::GetConnection();
        if(!$sql)
        {
            $sql->close();
            return "";
        }
        $query = "SELECT COALESCE(farm,'') AS frm FROM users WHERE name='$user' AND pass=Password('$pass') LIMIT 1";
        $rd = mysql_query($query,$sql);
        if($rd)
        {
            $row = mysql_fetch_assoc($rd);
            $result = $row["frm"];
        }
        $sql->close();
        return $result;
    }

	/**
	 * Добавляет запись в БД о полученой Резервной копии
	 * @param string $farm - Название организации
	 * @param string $filename - Имя файла РК
	 * @param string $md5dump - md5 dump-файла
	 */
	public static function AddDump($clientId,$filename,$md5dump)
	{
		/*$query = "SELECT farm, filename, md5dump FROM dumplist
				  WHERE clientId=$clientId AND filename='$filename' AND md5dump='$md5dump'";
		DBworker::GetListOfStruct($query);*/
        global $log;
        $log->debug('hi'.Conf::$LOG_QRS);
		$query = "INSERT INTO dumplist(datetime, clientId, filename, md5dump) VALUES (NOW(),'$clientId','$filename','$md5dump')";
        DBworker::Execute($query);
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
		$sql = DBworker::GetConnection();
		$query = "SELECT Date_Format(date,'%d.%m.%Y') AS dt , fucks, okrols, proholosts, born,
					killed, deads, rabbits FROM globalreport WHERE farm='$farmname' AND `database`='$db' ORDER BY date ASC";
		$rd = mysql_query($query,$sql);
		$result = array();
		while($row = mysql_fetch_assoc($rd))
			array_push($result, $row);
		return $result;
		$sql->close();
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
		$sql = DBworker::GetConnection();
		$query = "SELECT DISTINCT `database` FROM globalreport WHERE farm='$farmname'";
		$rd = mysql_query($query,$sql);
		while($row = mysql_fetch_assoc($rd))
			array_push($result, $row["database"]);
		$sql->close();
		return $result;
	}

	/**
	 * Добавляет в БД Глобальный отчет
	 * @param string $farmname - Название организации
	 * @param simpleXMLElement $xml
	 */
	private static function AddWebRep_Global($farmname,$xml)
	{
		$sql = DBworker::GetConnection();
		$database = $xml["database"];
		foreach ($xml->oneglobalday as $gd)
		{
			$query = "INSERT INTO globalReport( farm, `database`, `date`, fucks, proholosts, born, killed, deads, rabbits)
						VALUES('$farmname','$database','".$gd["date"]."',".$gd["fucks"].",".$gd["proholosts"].",
						".$gd["born"].",".$gd["killed"].",".$gd["deads"].",".$gd["rabbits"].")";
			mysql_query($query,$sql);
		}
		$sql->close();
	}
}

?>
<?php

class MySqlException extends Exception{}

class DBworker
{
	//private static $sql = null;
	
	private static function connect($dbName = null)
	{
		//if(self::$sql != null)self::disconnect();
		if($dbName == null) $dbName = "grdupdate";
		$link = new mysqli("localhost","root","",$dbName);
		//mysql_select_db($dbName,$link);
		if(mysqli_connect_errno()) 
			throw new MySqlException("Не удалось подключиться к БД");
		//mysqli_set_charset($link,"utf8");
		//mysql_query("set names utf8;",$link);
		return $link;
	}
	
	/*private static function disconnect()
	{
		return mysqli_close(self::$sql);		
	}*/
	
	/**
	 * Выполняет запрос к базе данных и возвращает Двумерный масив. 
	 * Желательно чтобы имена возвращаемых полей совпадали с именами
	 * структур в клиентском приложении.
	 * @param string $query - Текст SQL-запроса к Базе Данных
	 * @throws MySqlException 
	 */
	public static function GetListOfStruct($query)
	{
		$mysqli = self::connect();
		$rd = self::executeQuerys($mysqli,$query);
		
		$result = array();
		while ($row = $rd->fetch_assoc())
		{		
			$arr2d = array();
			foreach ($row as $name =>$value)
				$arr2d[$name] = $value;
			$result[] = $arr2d;
		}
		$rd->close();		
		$mysqli->close();
		return $result;
	}
	
	/**
	 * Выполняет команду БД. Ничего не возвращает
	 * @param string $query - Текст SQL-запроса к Базе Данных
	 * @param string $blob - Бинарная строка
	 * @throws MySqlException
	 */
	public static function Execute($query)
	{

		$mysqli = self::connect();
		$rd = self::executeQuerys($mysqli,$query);
		$mysqli->close();
	}	

	public static function GetConnection()
	{
		return self::connect();
	}
	
	private static function executeQuerys($mysqli,$query)
	{
		global $log;
		$log->debug("Query: \n".$query);
				
		//участок кода для мультизапросов
		$query = rtrim($query,";");
		$qrs = explode(";",$query);
		$i=0;
		while($i < (count($qrs)-1) )
		{
			if(!$mysqli->query($qrs[$i])) 
			{
				$log->error($mysqli->error);
				$mysqli->close();
				throw new MySqlException("Ошибка в запросе");
			}
			$i++;
		}
		
		$rd = $mysqli->query($qrs[$i]);				
		if(!$rd) 
		{
			$log->error($mysqli->error);
			$mysqli->close();
			throw new MySqlException("Ошибка в запросе");
		}
		
		return $rd;
	}
}
?>
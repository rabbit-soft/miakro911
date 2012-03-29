<?php
include_once 'DBworker.php';
include_once 'helper.php';
include 'ServerFunc.php';
include "qReports.php";
/**
 * MethodsCaller
 * @author Gam6it
 */
class MC
{		
	/**
	 * Вызывает метод, запрошенный Клиентом
	 * @param string $methodName -Имя запрашиваемого метода 
	 * @param array $params - Список параметров
	 * @return XML-RPC request or fault strings
	 * @tutorial Если в Клиентском приложении возвращаемое значение метода помечено как void,
	 *  то здешний метод должен вернуть пустую строку
	 */
	public static function callMethod($methodName,$params=NULL)
	{	
		global $log, $UID;//определена в index.php
		$log->debug("callMethod: '$methodName'");
		$log->debug("params: \n".var_export($params,true));
		try 
		{
			$return_value = "";
			$logThis = false;
			switch ($methodName)
			{				
				case "user.genkey": $return_value = user_gen_key($params[0]); break;
				default:return self::methodNotFound($methodName); 					
			}
			//$log->trace("callMethod->return_value\n".var_export($return_value,true));
			if($logThis)
			{
				//TODO Логирование !!
			}				
			return XMLRPC::response($return_value);
		}
		catch (MySqlException $mexc)
		{
			$log->error($mexc->getMessage());
			return XMLRPC::error(3, $mexc->getMessage());
		}
		catch (Exception $exc)
		{
			$log->error($exc->getMessage());
			return XMLRPC::error(2, $exc->getMessage());
		}		
	}	
	
	private static function methodNotFound($methodName)
	{
		return XMLRPC::error(1, "Запрашиваемый метод '$methodName', не найден.");
	}
	
	public static function user_gen_key($userId)
	{
		global $UID;
		$key = self::getRandomKey();
		$newpass =  $UID == $userId ? 0: 1;
		$query = "update padmin.Users set U_NU_KEYS=?,U_NEWPASS=$newpass where UID=$userId";
		
		$mysqli = DBworker::GetConnection();
		$stmt =  $mysqli->prepare($query);
		$stmt->bind_param('b', $null);
		$stmt->send_long_data(0, $key);
		$stmt->execute();
		$stmt->close();
		$mysqli->close();
		
		$key = array_values(unpack("V*", $key));
		return $key;
	}
}
?>
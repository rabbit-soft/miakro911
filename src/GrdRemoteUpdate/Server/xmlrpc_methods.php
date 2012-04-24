<?php
include_once "DBworker.php";
include_once "helper.php";
include_once "libxmlrpc.php";

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
		$log->info("callMethod: $methodName");
		$log->debug("params: \n".var_export($params,true));
		try 
		{
			$return_value = "";
			$logThis = false;
			switch ($methodName)
			{				
				case "client.get.update": $return_value = self::client_get_update($params[0],$params[1]); break;
				case "user.genkey": $return_value = self::user_gen_key($params[0]); break;
				case "ping": $return_value = "pong"; break;
				case "client.add": self::client_add($params[0],$params[1],$params[2],$params[3]); break;
				case "clients.get": $return_value = self::clients_get($params[0]); break;
				case "client.money.add": self::client_money_add($params[0],$params[1]); break;
                case "get.payments": $return_value = self::get_payments($params[0]); break;
                case "get.costs": $return_value = self::get_costs($params[0]); break;
                case "vendor.add.dongle": self::vendor_add_dongle($params[0],$params[1],$params[2]) ; break;
				case "vendor.update.dongle": $return_value = self::vendor_update_dongle($params[0],$params[1],$params[2],$params[3],$params[4],$params[5],$params[6]) ; break;
				case "vendor.shedule.dongle": $return_value = self::vendor_shedule_dongle($params[0],$params[1],$params[2],$params[3],$params[4],$params[5],$params[6]) ; break;				
                case "dongle.update.success": $return_value = self::dongle_update_success($params[0],$params[1]); break;
                default:return self::methodNotFound($methodName);
			}
			$log->trace("callMethod->return_value\n".var_export($return_value,true));
			if($logThis)
			{
				//TODO Логирование !!
			}				
			return XMLRPC::Response($return_value);
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
		$query = "UPDATE grdupdate.users set u_key=?,u_new_pass=$newpass where u_id=$userId";
		
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
	
	private static function getRandomKey($count=0)
	{
		if($count==0)
			$count = rand(50, 75);
		$result="";
		for($i=0;$i<$count;$i++)	
			$result .= pack("V",rand());		
		return $result;
	}
	
	private static function clients_get($clientId = null)
	{   self::debug("clientid ".$clientId);
		$query = sprintf("SELECT c_id,c_org,c_address,c_contact,c_money,c_saas FROM clients %s;",(isset($clientId) ? "WHERE c_id=$clientId":""));
		$clients = DBworker::GetListOfStruct($query);
		for($i=0;$i<count($clients);$i++)
		{
			$cid = $clients[$i]["c_id"];
			$query = "SELECT DISTINCT d_id,d_model,u_farms,u_flags,
			Date_Format(u_start_date,'%Y-%m-%d') sd,
			Date_Format(u_end_date,'%Y-%m-%d') ed,
			u_time_flags,
			Date_Format(u_time_flags_end,'%Y-%m-%d') tfe
				FROM dongles d ,updates
				WHERE d_id=u_dongle AND d_returned=0 AND d_client=$cid AND u_date=(select max(u_date) from updates WHERE d.d_id=u_dongle);";
			$clients[$i]["dongles"] = DBworker::GetListOfStruct($query);
			settype($clients[$i]["c_saas"], Type::bool);
		}
		//self::debug(var_export($clients,true));
		return $clients;
	}
	
	private static function client_add($org, $contact, $address,$saas)
	{
        $key = self::getRandomKey(65);
		DBworker::Execute("INSERT INTO clients(c_org,c_contact,c_address,c_saas,c_key) VALUES('$org','$contact','$address',$saas,'$key');");

        /*$stmt =  $mysqli->prepare("update clients set c_key=? where c_id=2");
        $stmt->bind_param('b', $key);
        $err = $stmt->error;
        $stmt->send_long_data(0, $key);//TODO возможно это кастыль
        $err = $stmt->error;
        $err = $stmt->execute();
        $err = $stmt->error;
        $mysqli->close();*/

		//DBworker::Execute($query); //TODO key
	}
	
	private static function client_money_add($orgId, $money)
	{
		DBworker::Execute("INSERT INTO money(m_client,m_debet,m_date,m_comment) VALUES($orgId,$money,NOW(),'добавили денег');
			UPDATE clients SET c_money=c_money+$money WHERE c_id=$orgId;");
	}
	
	private static function vendor_add_dongle($dongle, $orgId, $model)
	{
		/*$result = DBworker::GetListOfStruct("SELECT COUNT(*) cnt FROM dongles WHERE d_id=$dongle;");
		if($result[0]['cnt']!=0)
			throw new Exception("Данный ключ уже добавлен в БД");*/ // todo release uncomment!
		DBworker::Execute(
			"INSERT INTO dongles(d_id,d_client,d_model) VALUES($dongle,$orgId,$model);");
	}
	
	private static function vendor_update_dongle($base64_question, $orgId, $farms, $flags, $startDate, $endDate, $dongle)
	{
		$timeFlags = 0;//заглушка
		$timeFlagsEnd = "NOW()"; //Заглушка
		
		$org = DBworker::GetListOfStruct("SELECT c_key,c_org FROM clients WHERE c_id=$orgId;");
		$key = base64_encode($org[0]['c_key']); //ключ-шифрования, который будет вшит в ключ-защиты, чтобы шифровать посылки к сереверу
		$result = self::reqest('dongle.update',
			array(
				XMLRPC::Prepare($base64_question,Type::str), 
				XMLRPC::Prepare($orgId, Type::int),
				XMLRPC::Prepare($org[0]['c_org'], Type::str),
				XMLRPC::Prepare($farms,Type::int), 
				XMLRPC::Prepare($flags,Type::int), 
				XMLRPC::Prepare($startDate,Type::str), 
				XMLRPC::Prepare($endDate, Type::str),
				XMLRPC::Prepare($key, Type::str)));
		if(!$result[0])		
			throw new Exception("Ошибка сервиса обновления ключей\n".$result[1]['faultCode']);
		//self::debug("responce ".var_export($result,true));	
		self::money_sub($orgId,$farms,$endDate);
		DBworker::Execute("INSERT INTO updates(u_dongle, u_client, u_date, u_farms, u_start_date, u_end_date, u_flags, u_time_flags, u_time_flags_end) 
										VALUES($dongle,$orgId,NOW(),$farms,'$startDate','$endDate',$flags,$timeFlags,$timeFlagsEnd);");	
		
		return $result[1];	
	}
	
	private static function client_get_update($base64_question,$dongleId)
	{
		global $UID;
		$timeFlags = 0;//заглушка
		$timeFlagsEnd = "NOW()"; //Заглушка
		
		$org = DBworker::GetListOfStruct("SELECT c_key,c_org FROM clients WHERE c_id=$UID;");
		$data = DBworker::GetListOfStruct("SELECT u_farms,u_flags,u_start_date,u_end_date FROM updates WHERE u_client=$UID AND u_waiting=1 AND u_dongle=$dongleId;");
		if(count($data)==0)
			throw new Exception("Нет назначенных обновлений");
		$key = base64_encode($org[0]['c_key']); //ключ-шифрования, который будет вшит в ключ-защиты, чтобы шифровать посылки к сереверу
		$result = self::reqest('dongle.update',
			array(
				XMLRPC::Prepare($base64_question,Type::str), 
				XMLRPC::Prepare($UID, Type::int),
				XMLRPC::Prepare($org[0]['c_org'], Type::str),
				XMLRPC::Prepare($data[0]['u_farms'],Type::int), 
				XMLRPC::Prepare($data[0]['u_flags'],Type::int), 
				XMLRPC::Prepare($data[0]['u_start_date'],Type::str), 
				XMLRPC::Prepare($data[0]['u_end_date'], Type::str),
				XMLRPC::Prepare($key, Type::str)));
		if(!$result[0])		
			throw new Exception("Ошибка сервиса обновления ключей\n".$result[1]['faultCode']);
        return $result[1];
	}
	
	private static function vendor_shedule_dongle($orgId, $farms, $flags, $startDate, $endDate, $dongle)
	{
		$timeFlags = 0;//заглушка
		$timeFlagsEnd = "NOW()"; //Заглушка
		
		self::money_sub($orgId,$farms,$endDate);
		DBworker::Execute("INSERT INTO updates(u_dongle, u_client, u_date, u_farms, u_start_date, u_end_date, u_flags, u_time_flags, u_time_flags_end,u_waiting) 
									VALUES($dongle,$orgId,NOW(),$farms,'$startDate','$endDate',$flags,$timeFlags,$timeFlagsEnd,1);");
	}
	
	private static function money_sub($orgId,$farms,$endDate)
	{
		$client = DBworker::GetListOfStruct("SELECT c_money, c_saas FROM clients WHERE c_id=$orgId;");

		if($client[0]["c_saas"])
		{	 
			$dng = DBworker::GetListOfStruct("SELECT DATE_FORMAT(u_end_date,'%Y-%m-%d') endd FROM updates WHERE u_client=$orgId ORDER BY u_date desc limit 1;");		
			$months = self::dtDiff($dng[0]['endd'],$endDate);	//self::debug("months ".$months);
			$money = $farms*5*$months;
			$comment = "плата за SAAS версию $months мсц. $farms фрм.";
		}
		else
		{
            $money=$farms*100;
            $comment ="плата за коробочную версию";
        }
		if((int)$money > (int)$client[0]["c_money"])
				throw  new Exception("Недостаточно средств на счету.\nНеобходимо: $money.\nДоступно:".$client[0]["c_money"]);
		DBworker::Execute("INSERT INTO money(m_client,m_credit,m_date,m_comment) VALUES($orgId,$money,NOW(),'$comment');
								UPDATE clients SET c_money=c_money-$money WHERE c_id=$orgId;");	
	}
	
	private static function reqest($method, $params)
	{
		self::debug("responce");
		return XMLRPC::Request('192.168.0.103:11000', '/rpc2', $method, $params);
	}
	
	private static function debug($message)
	{
		global $log;
		$log->debug($message);
	}
	
	private static function dtDiff($sd,$ed)
	{	self::debug("dtDiff ".$sd." ".$ed);
		$sd = split("-", $sd);
		$ed = split("-", $ed);
		$months = ((int)$ed[0] - (int)$sd[0]) * 12; 
		$months += (int)$ed[1] - (int)$sd[1]; 		
		return $months;
	}

    private static function dongle_update_success($dongleId)
    {
        global $UID;
        DBworker::Execute("UPDATE updates SET u_waiting=0 WHERE u_client=$UID AND u_waiting=1 AND u_dongle=$dongleId;");
    }

    private static function get_payments($clientId)
    {
        return DBworker::GetListOfStruct("SELECT m_date,m_debet,m_credit,m_comment FROM money WHERE m_client=$clientId ORDER BY m_date;");
    }

    private static function get_costs($array1)
    {
        $result= DBworker::GetListOfStruct("SELECT o_value FROM options WHERE o_name='price';");
        $ret = array($result[0]['o_value'],$result[1]['o_value']);
        return $ret;
    }
}
?>
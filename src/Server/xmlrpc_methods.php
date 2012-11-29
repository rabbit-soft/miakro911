<?php
include_once "gamlib/DBworker.php";
include_once "gamlib/helper.php";

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
		global $log, $UID;//определена в forrpc.php
		$log->info("callMethod: $methodName");
		$log->debug("params: \n".var_export($params,true));
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
            case "get.dumplist": $return_value = self::get_dumplist($params[0]); break;
            case "get.update.files": $return_value = self::get_update_files(); break;
            case "vendor.add.dongle": self::vendor_add_dongle($params[0],$params[1],$params[2]) ; break;
            case "vendor.update.dongle": $return_value = self::vendor_update_dongle($params[0],$params[1],$params[2],$params[3],$params[4],$params[5],$params[6]) ; break;
            case "vendor.shedule.dongle": $return_value = self::vendor_shedule_dongle($params[0],$params[1],$params[2],$params[3],$params[4],$params[5],$params[6]) ; break;
            case "dongle.update.success": $return_value = self::dongle_update_success($params[0],$params[1]); break;
            case "webrep.get.lastdate": $return_value = self::GetWebRep_LastDate($params[0]); break;
            case "webrep.send.global": self::ParseWebReport($params[0],$params[1]); break;
            default: throw new pException("called method not exists",pErrCode::ServerMethodNotFound);
        }
        $log->trace("callMethod->return_value\n".var_export($return_value,true));
        if($logThis)
        {
            //TODO Логирование !!
        }
        return $return_value;
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
        Conf::$LOG_QRS = false;
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
			$clients[$i]["c_saas"] = Type::Cast($clients[$i]["c_saas"], Type::bool);
		}
        Conf::$LOG_QRS = true;
		//self::debug(var_export($clients,true));
		return $clients;
	}
	
	private static function client_add($org, $contact, $address,$saas)
	{
        global $log;
        $mysqli = DBworker::GetConnection();
        $key = $mysqli->real_escape_string(self::getRandomKey(65));
        $query = "INSERT INTO clients(c_org,c_contact,c_address,c_saas,c_key) VALUES('$org','$contact','$address',$saas,'$key');";
        $log->debug('Query:     '.$query);
        if(!$mysqli->query($query))
            throw new MySqlException($mysqli->error,$mysqli->errno);

        //DBworker::Execute($query);
        /*$mysqli = DBworker::GetConnection();
        $stmt =  $mysqli->prepare("INSERT INTO clients(c_org,c_contact,c_address,c_saas,c_key) VALUES('$org','$contact','$address',$saas,?);");
        $stmt->bind_param('b', null);
        $err = $stmt->error;
        $stmt->send_long_data(0, $key);//TODO возможно это кастыль
        $err = $stmt->error;
        $err = $stmt->execute();
        $err = $stmt->error;
        $mysqli->close();
        global $log; $log->debug('err '.$err);*/

	}
	
	private static function client_money_add($orgId, $money)
	{
		DBworker::Execute("INSERT INTO money(m_client,m_debet,m_date,m_comment) VALUES($orgId,$money,NOW(),'добавили денег');
			UPDATE clients SET c_money=c_money+$money WHERE c_id=$orgId;");
	}
	
	private static function vendor_add_dongle($dongle, $orgId, $model)
	{
		$result = DBworker::GetListOfStruct("SELECT COUNT(*) cnt FROM dongles WHERE d_id=$dongle;");
		if($result[0]['cnt']==0)
		    DBworker::Execute("INSERT INTO dongles(d_id,d_client,d_model) VALUES($dongle,$orgId,$model);");
	}
	
	private static function vendor_update_dongle($base64_question, $orgId, $farms, $flags, $startDate, $endDate, $dongle)
	{
		$timeFlags = 0;//заглушка
		$timeFlagsEnd = "NOW()"; //Заглушка
        self::money_sub($orgId,$farms,$endDate);
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
			$dng = DBworker::GetListOfStruct("SELECT DATE_FORMAT(Coalesce(u_end_date,NOW()),'%Y-%m-%d') endd FROM updates WHERE u_client=$orgId ORDER BY u_date desc limit 1;");
			$dng = count($dng)==0 ? date("Y-m-d") : $dng[0]['endd'];

            $months = self::dtDiff($dng,$endDate);	//self::debug("months ".$months);
			$money = $farms*5*$months;  //todo если прошили на больше ферм, но дата осталась та же
			$comment = "плата за SAAS версию $months мсц. $farms фрм.";
		}
		else
		{
            $money=$farms*100;
            $comment ="плата за коробочную версию";
        }
		if((int)$money > (int)$client[0]["c_money"])
				throw  new Exception("Недостаточно средств на счету.\nНеобходимо: $money.\nДоступно:".$client[0]["c_money"]);
        if($money==0) return;
		DBworker::Execute("INSERT INTO money(m_client,m_credit,m_date,m_comment) VALUES($orgId,$money,NOW(),'$comment');
								UPDATE clients SET c_money=c_money-$money WHERE c_id=$orgId;");	
	}
	
	private static function reqest($method, $params)
	{
		return XMLRPC::Request(Conf::$DONG_UPDATE_HOST.':11000', '/rpc2', $method, $params);
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

    /**
     * Получает дату последнего отчета
     * @param string $farmname - Название организации
     * @package string $db - имя БазыДанных
     * @return string Дату либо "nodates"
     */
    private static function GetWebRep_LastDate($db)
    {
        //$result = "nodates";
        //$farmname = iconv("cp1251", "UTF-8", $farmname);
        //$db = iconv("cp1251", "UTF-8", $db);
        global $UID;
        $query ="SELECT Max(`date`) dt FROM globalReport WHERE clientId=$UID AND `database`='$db'";
        $result = DBworker::GetListOfStruct($query);
        return $result[0]['dt'];
    }

    /**
     * Разбирает полученную от rabdump'а XML со статистикой
     * @param string $farmname - Название организации
     * @param string $xml_text - Текст XML-статистики
     */
    private static function ParseWebReport($database,$array)
    {
        global $UID;
        $query = "INSERT INTO globalReport( clientId, `database`, `date`, fucks, proholosts, born, killed, deads, rabbits)
						VALUES";
        foreach ($array as $day)
        {
            $query.=sprintf("($UID,'$database','%s',%s,%s,%s,%s,%s,%s),",
                $day["Date"],$day["Fucks"],$day["Proholosts"],$day["Born"],$day["Killed"],$day["Deads"],$day["Rabbits"]);
        }
        $query = rtrim($query,',');
        DBworker::Execute($query);
    }
    /**
     * Возвращает XML со списком имеющихся Резервных Копий
     * @param string $farmname - Название организации
     * @return XML
     */
    private static function get_dumplist()
    {
        global $UID;
        $query = "SELECT farm, datetime, filename, md5dump FROM dumplist WHERE clientId=$UID order by datetime desc";
        return DBworker::GetListOfStruct($query);
    }

    private static function get_update_files($pand="")
    {
        $result=array();
        $path =Conf::$UPDATE_PATH.$pand;
        if (is_dir($path))
        {
            if ($dh = opendir($path))
            {
                while (($file = readdir($dh)) !== false)
                {
                    if($file=="." ||$file=="..")continue;
                    $row = array();
                    if((filetype($path.$file) == "dir"))
                    {
                        $tabs = self::get_update_files("$file/");
                        $result = array_merge($result,$tabs);
                    }
                    else
                    {
                        $row['Name']=$file;
                        $row['md5'] = md5_file($path.$file);
                        $row['Path']=$pand;
                        $row['Version']=implode('.',GetFileVersion($path.$file));
                        $result[]=$row;
                    }
                }
                closedir($dh);
            }
        }
        return $result;
    }
}
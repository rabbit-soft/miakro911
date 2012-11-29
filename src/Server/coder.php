<?php
include_once 'gamlib/DBworker.php';
include_once 'gamlib/xxtea.php';

class DecryptionException extends Exception{}

class Coder
{
    private static $def_user_key = "user_with_old_key";
	/**
	 * Шифрует
	 * @param string $str - НЕ шифрованная строка
	 * @return string - Шифрованная строка
	 */
	public static function Encrypt($str)
	{
		global $UID;
        //$log->debug("ecr str: ".var_export($str,true));
		if(!isset($UID))
			return $str;
        $str = gzdeflate($str);
        if($UID == 0)
        {
            return XXTEA::Encrypt($str,self::$def_user_key);
        }
		$query = defined("CLIENT")
			? "select c_key as u_key from clients where c_id=$UID limit 1;" 
			: "select u_key from users where u_id=$UID limit 1;";
		$keys = DBworker::GetListOfStruct($query);
		                        //$log->debug("deflate ".$str);
        return XXTEA::Encrypt($str,$keys[0]['u_key']); //$log->debug("chipher ".$text);
	}

    /**
     * Разшифровывает
     * @param string $str - Шифрованная строка
     * @param int $uid
     * @throws DecryptionException
     * @return string - Расшифрованная строка
     */
	public static function Decrypt($str,&$uid=0)
	{
        if(!isset($str) || strlen($str)==0)
            throw new DecryptionException("income string is empty"); //$log->info($str);
		global $log;
        Conf::$LOG_QRS = false;
		//$log->debug("Codestr: ".$str);
		if($str == null or strlen($str)==0) return;
		$uid = unpack("V1",$str);
		$uid = $uid[1];
        if($uid==0)
        {
            $text = XXTEA::Decrypt(substr($str, 4),self::$def_user_key);
            $text = gzinflate($text);
            if(!simplexml_load_string($text))
                throw new DecryptionException("Ошибка чтения посылки Даже с def ключом");
            return $text;
        }
		$keys = DBworker::GetListOfStruct("select u_key,u_new_key from users where u_id=$uid limit 1;");
		if(count($keys)==0)
		{
			$keys = DBworker::GetListOfStruct("select c_key as u_key from clients where c_id=$uid limit 1;");
			if(count($keys)==0)
				throw new DecryptionException("Ошибка чтения посылки");
			define("CLIENT", true);
		}
			
		//$log->debug("key of user: ".var_export($keys,true));
		$text = XXTEA::Decrypt(substr($str, 4),$keys[0]['u_key']);
		$text = gzinflate($text);
		if(!simplexml_load_string($text))
		{
			$text = XXTEA::Decrypt(substr($str, 4),$keys[0]['u_new_key']);			
			$text = gzinflate($text);
			if(!simplexml_load_string($text))
				throw new DecryptionException("Ошибка чтения посылки");
			else DBworker::Execute("update grdupdate.users set u_key=u_new_key where u_id=$uid;
									update grdupdate.users set u_new_key=null where u_id=$uid");
		}		
		//$log->debug("\nCHIPHER\n".self::xxtea_decrypt(substr($str, 4),'123'));
		//if($keys[0]['U_BLOCK'] !="0")
			//throw new DecryptionException("Пользователь заблокирован");
        Conf::$LOG_QRS = true;
		return $text;		
	}
	

}

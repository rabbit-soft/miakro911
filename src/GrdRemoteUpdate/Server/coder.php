<?php
include_once 'DBworker.php';

class DecryptionException extends Exception{}

class Coder
{
	/**
	 * Шифрует
	 * @param string $str - НЕ шифрованная строка
	 * @return string - Шифрованная строка
	 */
	public static function Encrypt($str)
	{
		global $UID;
		$keys = DBworker::GetListOfStruct("select U_KEYS from padmin.Users where UID=$UID limit 1;");
		$text = self::xxtea_encrypt($str,$keys[0]['U_KEYS']);
		return $text;
	}
	
	/**
	 * Разшифровывает
	 * @param string $str - Шифрованная строка
	 * @return string - Расшифрованная строка
	 */
	public static function Decrypt($str,&$uid=0)
	{	
		global $log;	
		$log->debug("Codestr: ".$str);
		if($str == null or strlen($str)==0)  return;
		$uid = unpack("V1",$str);
		$uid = $uid[1];		
		$keys = DBworker::GetListOfStruct("update padmin.Users set U_LAST_CONN=NOW() where UID=$uid;
		select U_KEYS,U_NU_KEYS,U_BLOCK from padmin.Users where UID=$uid limit 1;");
		//$log->debug(var_export($keys,true));
		$text = self::xxtea_decrypt(substr($str, 4),$keys[0]['U_KEYS']);
		
		if(!simplexml_load_string($text))
		{
			$text = self::xxtea_decrypt(substr($str, 4),$keys[0]['U_NU_KEYS']);			
			if(!simplexml_load_string($text))
				throw new DecryptionException("Ошибка чтения посылки");
			else DBworker::Execute("update padmin.Users set U_KEYS=U_NU_KEYS where UID=$uid;
			update padmin.Users set U_NU_KEYS=null where UID=$uid");
		}		
		//$log->debug("\nCHIPHER\n".self::xxtea_decrypt(substr($str, 4),'123'));
		if($keys[0]['U_BLOCK'] !="0")
			throw new DecryptionException("Пользователь заблокирован");
		return $text;		
	}
	
	 /**
	  * @param string $str - Исходный текст
	  * @param string $key - Пароль
	  */
	 public static function xxtea_encrypt($str, $key) 
	 {
	 	if ($str == "") return "";     
	    $v = self::str2long($str, true);
	    $k = self::str2long($key, false); 
	    if (count($k) < 4)
	        for ($i = count($k); $i < 4; $i++)
	            $k[$i] = 0;           
	    $n = count($v) - 1; //последний элемент, содержит размер строки. n=индекс последненго существенного эллемента

	    $z = $v[$n]; //$z = последний эллемент массива
	    $y = $v[0];  //$y - первый эллемент массива
	    $delta = 0x9E3779B9;  
	    $q = floor(6 + 52 / ($n + 1)); //n+1 индекс эллемента, в котором содержится размер строки
	    $sum = 0; 
	    while (0 < $q--) 
	    { 
	        $sum = self::int32($sum + $delta);  
	        $e = $sum >> 2 & 3;  
	        for ($p = 0; $p < $n; $p++) 
	        {
	            $y = $v[$p + 1];  
	            $mx = self::int32((($z >> 5 & 0x07ffffff) ^ $y << 2) + (($y >> 3 & 0x1fffffff) ^ $z << 4)) ^ self::int32(($sum ^ $y) + ($k[$p & 3 ^ $e] ^ $z));  
	            $z = $v[$p] = self::int32($v[$p] + $mx);	            
	        }
	        $y = $v[0];  
	        $mx = self::int32((($z >> 5 & 0x07ffffff) ^ $y << 2) + (($y >> 3 & 0x1fffffff) ^ $z << 4)) ^ self::int32(($sum ^ $y) + ($k[$p & 3 ^ $e] ^ $z));  
	        $z = $v[$n] = self::int32($v[$n] + $mx);
	    }	
	    return self::long2str($v, false);
	 }
 	 
 	 /**
	  * @param string $str - Исходный текст
	  * @param string $key - Пароль
	  */
	 public static function xxtea_decrypt($str, $key) 
	 {
	    if ($str == "") return "";
	    $v = self::str2long($str, false); 
	    $k = self::str2long($key, false);
	    if (count($k) < 4)
	        for ($i = count($k); $i < 4; $i++)
	            $k[$i] = 0;
	    $n = count($v) - 1;

	    $z = $v[$n];  
	    $y = $v[0];  
	    $delta = 0x9E3779B9;  
	    $q = floor(6 + 52 / ($n + 1));
	    $sum = self::int32($q * $delta);
	    while ($sum != 0) 
	    {  
	        $e = $sum >> 2 & 3;  
	        for ($p = $n; $p > 0; $p--) 
	        {
	            $z = $v[$p - 1];  
	            $mx = self::int32((($z >> 5 & 0x07ffffff) ^ $y << 2) + (($y >> 3 & 0x1fffffff) ^ $z << 4)) ^ self::int32(($sum ^ $y) + ($k[$p & 3 ^ $e] ^ $z));  
	            $y = $v[$p] = self::int32($v[$p] - $mx);  
	        }
	        $z = $v[$n];  
	        $mx = self::int32((($z >> 5 & 0x07ffffff) ^ $y << 2) + (($y >> 3 & 0x1fffffff) ^ $z << 4)) ^ self::int32(($sum ^ $y) + ($k[$p & 3 ^ $e] ^ $z));  
	        $y = $v[0] = self::int32($v[0] - $mx);  
	        $sum = self::int32($sum - $delta);  
	    }  	   
	    return self::long2str($v, true);
	}
	 
	private static function long2str($v, $w) 
	{  
	     $len = count($v); 
	     $n = ($len - 1) << 2;
	     if ($w) 
	     {
	         $m = $v[$len - 1];  
	         if (($m < $n - 3) || ($m > $n)) 
	         	return false;  
	         $n = $m;  
	     } 
	     $s = array();  
	     for ($i = 0; $i < $len; $i++) 
	         $s[$i] = pack("V", $v[$i]);  
	     if ($w)
	         return substr(join('', $s), 0, $n);
	     else
	         return join('', $s);
	 }
	 
	 /**
	  * Переводит человекочитаемую строку в массив из Uint
	  * @param string $s
	  * @param boolean $w - В последний эллемент массива записать размер строки $s
	  */
	 private static function str2long($s, $w) 
	 {  
	     $v = unpack("V*", $s. str_repeat("\0", (4 - strlen($s) % 4) & 3));
	     $v = array_values($v);
	     if ($w) 	       
	         $v[count($v)] = strlen($s);      
	     return $v;  
	 }
 
	 private static function int32($n) 
	 {  
	     while ($n >= 2147483648) $n -= 4294967296;  
	     while ($n <= -2147483649) $n += 4294967296;  
	     return (int)$n;  
	 }
	 
}

?>
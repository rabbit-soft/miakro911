<?php 
/* 
An XML-RPC implementation by Keith Devens, version 2.5f. 
http://www.keithdevens.com/software/xmlrpc/ 

Release history available at: 
http://www.keithdevens.com/software/xmlrpc/history/ 

This code is Open Source, released under terms similar to the Artistic License. 
Read the license at http://www.keithdevens.com/software/license/ 

Note: this code requires version 4.1.0 or higher of PHP. 
*/ 

require_once 'log4php/Logger.php';

class XML 
{ 
	var $parser; #a reference to the XML parser
	var $document; #the entire XML structure built up so far
	var $current; #a pointer to the current item - what is this
	var $parent; #a pointer to the current parent - the parent will be an array
	var $parents; #an array of the most recent parent at each level

	var $last_opened_tag;

	function XML($data=null)
	{
		$this->parser = xml_parser_create();

		xml_parser_set_option ($this->parser, XML_OPTION_CASE_FOLDING, 0);
		xml_set_object($this->parser, $this);
		xml_set_element_handler($this->parser, "open", "close");
		xml_set_character_data_handler($this->parser, "data");
		#        register_shutdown_function(array(&$this, 'destruct'));
	}

	function destruct()
	{
		xml_parser_free($this->parser);
	}

	function parse($data)
	{
		$this->document = array();
		$this->parent = &$this->document;
		$this->parents = array();
		$this->last_opened_tag = NULL;
		xml_parse($this->parser, $data);
		return $this->document;
	}

	function open($parser, $tag, $attributes)
	{
		#echo "Opening tag $tag<br>\n";
		$this->data = "";
		$this->last_opened_tag = $tag; #tag is a string
		if(array_key_exists($tag, $this->parent)){
			#echo "There's already an instance of '$tag' at the current level ($level)<br>\n";
			if(is_array($this->parent[$tag]) and array_key_exists(0, $this->parent[$tag]))
			{ 	#if the keys are numeric
				#need to make sure they're numeric (account for attributes)
				$key = XMLRPC::count_numeric_items($this->parent[$tag]);
				#echo "There are $key instances: the keys are numeric.<br>\n";
			}
			else
			{
				#echo "There is only one instance. Shifting everything around<br>\n";
				$temp = &$this->parent[$tag];
				unset($this->parent[$tag]);
				$this->parent[$tag][0] = &$temp;

				if(array_key_exists("$tag attr", $this->parent))
				{
					#shift the attributes around too if they exist
					$temp = &$this->parent["$tag attr"];
					unset($this->parent["$tag attr"]);
					$this->parent[$tag]["0 attr"] = &$temp;
				}
				$key = 1;
			}
			$this->parent = &$this->parent[$tag];
		}
		else
		{
			$key = $tag;
		}
        if($attributes)
        { 
            $this->parent["$key attr"] = $attributes; 
        } 

        $this->parent[$key] = array(); 
        $this->parent = &$this->parent[$key];
        array_unshift($this->parents, $this->parent);
    } 

    function data($parser, $data)
    { 
        #echo "Data is '", htmlspecialchars($data), "'<br>\n"; 
        if($this->last_opened_tag != NULL)
        { 
            $this->data .= $data; 
        } 
    } 

    function close($parser, $tag)
    { 
        #echo "Close tag $tag<br>\n"; 
        if($this->last_opened_tag == $tag)
        { 
            $this->parent = $this->data; 
            $this->last_opened_tag = NULL; 
        } 
        array_shift($this->parents); 
        $this->parent = &$this->parents[0]; 
    } 
} 

class XMLRPC
{			
	private static $logger = NULL;	
	
	private static function & adjustValue(&$current_node)
	{ 
	    if(is_array($current_node))
	    { 
	        if(isset($current_node['array'])){ 
	            if(!is_array($current_node['array']['data']))
	            { 
	                #If there are no elements, return an empty array 
	                return array(); 
	            }
	            else
	            { 
	                #echo "Getting rid of array -> data -> value<br>\n"; 
	                $temp = &$current_node['array']['data']['value']; 
	                if(is_array($temp) and array_key_exists(0, $temp)){ 
	                    $count = count($temp); 
	                    for($n=0;$n<$count;$n++){ 
	                        $temp2[$n] = &self::adjustValue($temp[$n]);
	                    } 
	                    $temp = &$temp2; 
	                }
	                else
	                { 
	                    $temp2 = &self::adjustValue($temp);
	                    $temp = array(&$temp2); 
	                    #I do the temp assignment because it avoids copying, 
	                    # since I can put a reference in the array 
	                    #PHP's reference model is a bit silly, and I can't just say: 
	                    # $temp = array(&XMLRPC_adjustValue(&$temp)); 
	                } 
	            } 
	        }
	        elseif(isset($current_node['struct']))
	        { 
	            if(!is_array($current_node['struct']))
	            { 
	                #If there are no members, return an empty array 
	                return array(); 
	            }
	            else
	            { 
	                #echo "Getting rid of struct -> member<br>\n"; 
	                $temp = &$current_node['struct']['member']; 
	                if(is_array($temp) and array_key_exists(0, $temp))
	                { 
	                    $count = count($temp); 
	                    for($n=0;$n<$count;$n++)
	                    { 
	                        #echo "Passing name {$temp[$n][name]}. Value is: " . show($temp[$n][value], var_dump, true) . "<br>\n"; 
	                        $temp2[$temp[$n]['name']] = &self::adjustValue($temp[$n]['value']);
	                        #echo "adjustValue(): After assigning, the value is " . show($temp2[$temp[$n]['name']], var_dump, true) . "<br>\n"; 
	                    } 
	                }
	                else
	                { 
	                    #echo "Passing name $temp[name]<br>\n"; 
	                    $temp2[$temp['name']] = &self::adjustValue($temp['value']);
	                } 
	                $temp = &$temp2; 
	            } 
	        }
	        else
	        { 
	            $types = array('string', 'int', 'i4', 'double', 'dateTime.iso8601', 'base64', 'boolean'); 
	            $fell_through = true; 
	            foreach($types as $type)
	            { 
	                if(array_key_exists($type, $current_node))
	                { 
	                    #echo "Getting rid of '$type'<br>\n"; 
	                    $temp = &$current_node[$type]; 
	                    #echo "adjustValue(): The current node is set with a type of $type<br>\n"; 
	                    $fell_through = false; 
	                    break; 
	                } 
	            } 
	            if($fell_through)
	            { 
	                $type = 'string'; 
	                #echo "Fell through! Type is $type<br>\n"; 
	            } 
	            switch ($type)
	            { 
	                case 'int': case 'i4': $temp = (int)$temp;    break; 
	                case 'string':         $temp = (string)$temp; break; 
	                case 'double':         $temp = (double)$temp; break; 
	                case 'boolean':        $temp = (bool)$temp;   break; 
	            } 
	        } 
	    }
	    else
	    { 
	        $temp = (string)$current_node; 
	    } 
	    return $temp; 
	} 
	
	private static function convert_timestamp_to_iso8601($timestamp)
	{ 
	    #takes a unix timestamp and converts it to iso8601 required by XMLRPC 
	    #an example iso8601 datetime is "20010822T03:14:33" 
	    return date("Ymd\TH:i:s", $timestamp); 
	} 
	
	private static function convert_iso8601_to_timestamp($iso8601)
	{ 
	    return strtotime($iso8601); 
	} 
	
	public static function count_numeric_items(&$array)
	{ 
	    return is_array($array) ? count(array_filter(array_keys($array), 'is_numeric')) : 0; 
	} 
	
	private static function debug($function_name, $debug_message)
	{ 
		//global $DEBUG;
		//if(defined($DEBUG))
		//{
			if(self::$logger==NULL)
			{
				Logger::configure('log4php.xml');
				self::$logger = Logger::getLogger("xmlrpc");
			}
			$strlen = strlen($debug_message);
			if($strlen<20000)
				self::$logger->debug("$function_name: $debug_message"); 
			else
			{
				self::$logger->debug("Traced String ".$strlen." len"); 
				self::$logger->trace("$function_name: $debug_message"); 
			}
		//}
	} 
	
	/**
	 * Назначает тип переменной
	 * @param $data
	 * @param $type
	 */
		
	
	/**
	 * Преобразует массив ответа в XMLRPC-ответ
	 * @param array $data
	 * @param integer $level - Уровень рекурсии
	 * @param unknown_type $prior_key
	 */
	private static function & serialize(&$data, $level = 0, $prior_key = NULL)
	{ 
    	#assumes a hash, keys are the variable names 
    	$xml_serialized_string = ""; 
    	while(list($key, $value) = each($data))
    	{ 
    		$inline = false; 
        	$numeric_array = false; 
        	$attributes = ""; 
        	#echo "My current key is '$key', called with prior key '$prior_key'<br>"; 
        	if(!strstr($key, " attr")) #if it's not an attribute 
        	{
            	if(array_key_exists("$key attr", $data))
            	{ 
	                while(list($attr_name, $attr_value) = each($data["$key attr"]))
                	{  	#echo "Found attribute $attribute_name with value $attribute_value<br>"; 
                    	$attr_value = &htmlspecialchars($attr_value, ENT_QUOTES); 
                    	$attributes .= " $attr_name=\"$attr_value\""; 
                	} 
            	} 

            	if(is_numeric($key))
            	{  	#echo "My current key ($key) is numeric. My parent key is '$prior_key'<br>"; 
                	$key = $prior_key; 
            	}
            	else
            	{ 
                	#you can't have numeric keys at two levels in a row, so this is ok 
               		#echo "Checking to see if a numeric key exists in data."; 
                	if(is_array($value) and array_key_exists(0, $value))
                	{  	#echo " It does! Calling myself as a result of a numeric array.<br>"; 
                    	$numeric_array = true; 
                    	$xml_serialized_string .= self::serialize($value, $level, $key); 
                	} 
                	#echo "<br>"; 
            	} 

            	if(!$numeric_array)
            	{ 
                	$xml_serialized_string .= str_repeat("\t", $level) . "<$key$attributes>"; 

                	if(is_array($value))
                	{ 
                    	$xml_serialized_string .= "\r\n" . self::serialize($value, $level+1); 
                	}
                	else
                	{ 
                    	$inline = true; 
                    	$xml_serialized_string .= htmlspecialchars($value); 
                    	//self::debug("serialize $level", $value);
                	} 
                	$xml_serialized_string .= (!$inline ? str_repeat("\t", $level) : "") . "</$key>\r\n"; 
            	} 
        	}
        	else
        	{ 
            	#echo "Skipping attribute record for key $key<bR>"; 
        	} 
    	} 
    	if($level == 0)
    	{ 
        	$xml_serialized_string = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" . $xml_serialized_string; 
        	return $xml_serialized_string; 
    	}
    	else
    	{ 
       		return $xml_serialized_string; 
    	} 
	} 
	
	private static function show($data, $func = "print_r", $return_str = false)
	{ 
	    ob_start(); 
	    $func($data); 
	    $output = ob_get_contents(); 
	    ob_end_clean(); 
	    if($return_str)
	    { 
	        return  htmlspecialchars($output); 
	    }
	    else
	    { 
	        echo  htmlspecialchars($output); 
	    } 
	} 
	
	private static function & unserialize(&$xml)
	{ 
	    $xml_parser = new XML(); 
	    $data = &$xml_parser->parse($xml);
	    $xml_parser->destruct(); 
	    return $data; 
	} 

	/**
	 * Sends an XML-RPC error message
	 * @param integer $faultCode - Code of Error
	 * @param string $faultString - Message
	 * @param string $server - Server name in Header
	 */
	public static function error($faultCode, $faultString, $server = NULL)
	{ 	
		try 
		{
		    $array["methodResponse"]["fault"]["value"]["struct"]["member"] = array(); 
		    $temp = &$array["methodResponse"]["fault"]["value"]["struct"]["member"]; 
		    $temp[0]["name"] = "faultCode"; 
		    $temp[0]["value"]["int"] = $faultCode; 
		    $temp[1]["name"] = "faultString"; 
		    $temp[1]["value"]["string"] = $faultString; 
		
		    $return = self::serialize($array); 
		
		    header("Connection: close"); 
		    header("Content-Length: " . strlen($return)); 
		    header("Content-Type: text/xml"); 
		    header("Date: " . date("r")); 
		    if($server)
		    { 
		        header("Server: $server"); 
		    } 
	
		    //self::debug('XMLRPC_error', "Sent the following error response:\n\n" . $return); 
		     
		    return $return; 
		}
		catch (Exception $exc)
		{
			self::$logger->fatal("error: ".$exc->getMessage());
			return Coder::Encrypt("<?xml version=\"1.0\" encoding=\"UTF-8\" ?> 
				<methodResponse><fault><value><struct>
				<member><name>faultCode</name><value><int>2</int></value></member>
  				<member><name>faultString</name><value><string>Ошибка выполнения сценария на сервере</string></value></member>
  				</struct></value></fault></methodResponse>");
  			
		}
	}
	
	public static function GetParams($request)
	{ 
	    if(!is_array($request['methodCall']['params']))
	    { 
	        #If there are no parameters, return an empty array 
	        return array(); 
	    }
	    else
	    { 
	        #echo "Getting rid of methodCall -> params -> param<br>\n"; 
	        $temp = &$request['methodCall']['params']['param']; 
	        if(is_array($temp) and array_key_exists(0, $temp))
	        { 
	            $count = count($temp); 
	            for($n = 0; $n < $count; $n++)
	            { 
	                #echo "Serializing parameter $n<br>"; 
	                $temp2[$n] = &self::adjustValue($temp[$n]['value']);
	            } 
	        }
	        else
	        { 
	            $temp2[0] = &self::adjustValue($temp['value']); 
	        } 
	        $temp = &$temp2; 
	        return $temp; 
	    } 
	} 

	public static function GetMethodName($methodCall)
	{ 
	    #returns the method name 
	    return $methodCall['methodCall']['methodName']; 
	} 

	/**
	 * Разбирает XML-RPC request
	 * @param string $request - XMl
	 * @return array
	 */
	public static function & Parse(&$request)
	{ 
        //self::debug('XMLRPC_parse', "Received the following raw request:\n" . $request); 	    
	    $data = &self::unserialize($request);
	    //self::debug('XMLRPC_parse', "Returning the following parsed request:\n" . var_export($data,true)); 	    
	    return $data; 
	}
	
	/**
	 * Нужно при отправке Вызова процедуры чтобы указать Тип переменной
	 * @param $data
	 * @param $type - Тип переменной
	 */
	public static function & Prepare(&$data, $type = NULL)
	{ 
	    if(is_array($data)) 
	    { 
	        $num_elements = count($data); 
	        if((array_key_exists(0, $data) or !$num_elements) and $type != 'struct')
	        { 	#it's an array 
	            if(!$num_elements)
	            { 	#if the array is empty 
	                $returnvalue =  array('array' => array('data' => NULL)); 
	            }
	            else
	            { 
	                $returnvalue['array']['data']['value'] = array(); 
	                $temp = &$returnvalue['array']['data']['value']; 
	                $count = self::count_numeric_items($data); 
	                for($n=0; $n<$count; $n++)
	                { 
	                    $type = NULL; 
	                    if(array_key_exists("$n type", $data))
	                    { 
	                        $type = $data["$n type"]; 
	                    } 
	                    $temp[$n] = self::Prepare($data[$n], $type);
	                    
	                } 
	            } 
	        }
	        else
	        { 	#it's a struct 
	            if(!$num_elements)
	            { 	#if the struct is empty 
	                $returnvalue = array('struct' => NULL); 
	            }
	            else
	            { 
	                $returnvalue['struct']['member'] = array(); 
	                $temp = &$returnvalue['struct']['member']; 
	                while(list($key, $value) = each($data))
	                { 
	                    if(substr($key, -5) != ' type')
	                    { 	#if it's not a type specifier 
	                        $type = NULL; 
	                        if(array_key_exists("$key type", $data))
	                        { 
	                            $type = $data["$key type"]; 
	                        } 
	                        $temp[] = array('name' => $key, 'value' => self::Prepare($value, $type));
	                    } 
	                } 
	            } 
	        } 
	    }
	    else
	    { #it's a scalar 
	        if(!$type)
	        { 	
	            if(is_int($data))
	            { 	
	                $returnvalue['int'] = $data; 
	                return $returnvalue; 
	            }
	            elseif(is_float($data))
	            { 	
	                $returnvalue['double'] = $data; 
	                return $returnvalue; 
	            }
	            elseif(is_bool($data))
	            {
	                $returnvalue['boolean'] = ($data ? 1 : 0); 
	                return $returnvalue; 
	            }
	            elseif(preg_match('/^\d{8}T\d{2}:\d{2}:\d{2}$/', $data, $matches))
	            { 	#it's a date 
	                $returnvalue['dateTime.iso8601'] = $data; 
	                return $returnvalue; 
	            }
	            elseif(is_string($data))
	            { 
	                $returnvalue['string'] = $data;//htmlspecialchars($data);  TODO need TO test
	                return $returnvalue; 
	            } 
	        }
	        else
	        {
	            $returnvalue[$type] = htmlspecialchars($data); 
	        } 
	    }
	    return $returnvalue; 
	} 
	
	/**
	 * Делает вызов удаленной процедуры
	 * @param $site - адрес сервера
	 * @param $location - расположение
	 * @param $methodName - Имя вызываемого метода
	 * @param $params - Массив параметров
	 * @param $user_agent
	 */
	public static function Request($site, $location, $methodName, $params = NULL, $user_agent = NULL)
	{ 	
	    $site = explode(':', $site); 
	    if(isset($site[1]) and is_numeric($site[1]))
	    { 
	        $port = $site[1]; 
	    }
	    else
	    { 
	        $port = 80; 
	    } 
	    $site = $site[0]; 
	
	    $data["methodCall"]["methodName"] = $methodName; 
	    $param_count = count($params); 
	    if(!$param_count)
	    { 
	        $data["methodCall"]["params"] = NULL; 
	    }
	    else
	    { 
	        for($n = 0; $n<$param_count; $n++)
	        { 
	            $data["methodCall"]["params"]["param"][$n]["value"] = $params[$n]; 
	        } 
	    } 
	    $data = self::serialize($data); 	
	    //self::debug('XMLRPC_request', "Received the following parameter list to send:" . $params);     
	    //self::debug('XMLRPC_request', "Connecting to: $site:$port"); 
	    $conn = fsockopen ($site, $port); #open the connection 
	    if(!$conn)
	    { 	#if the connection was not opened successfully 
	        //self::debug('XMLRPC_request', "Connection failed: Couldn't make the connection to $site"); 	        
	        return array(false, array('faultCode'=>10532, 'faultString'=>"Connection failed: Couldn't make the connection to $site")); 
	    }
	    else
	    { 
	        $headers = 
	            "POST $location HTTP/1.0\r\n" . 
	            "Host: $site\r\n" . 
	            "Connection: close\r\n" . 
	            ($user_agent ? "User-Agent: $user_agent\r\n" : '') . 
	            "Content-Type: text/xml\r\n" . 
	            "Content-Length: " . strlen($data) . "\r\n\r\n"; 
	
	        fputs($conn, "$headers"); 
	        fputs($conn, $data); 
	
	        self::debug('XMLRPC_request', "Sent the following request:" . $headers . $data); 	         
	
	        #socket_set_blocking ($conn, false); 
	        $response = ""; 
	        while(!feof($conn))
	        { 
	            $response .= fgets($conn, 1024); 
	        } 
	        fclose($conn); 
	
	        #strip headers off of response 
	        $data = self::unserialize(substr($response, strpos($response, "\r\n\r\n")+4)); 	
 	        //self::debug('XMLRPC_request', "Received the following response:\n" . $response . "Which was serialized into the following data:\n" . var_export($data,true)); 
	        
	        if(isset($data['methodResponse']['fault'])){ 
	            $return =  array(false, self::adjustValue($data['methodResponse']['fault']['value']));
	            self::debug('XMLRPC_request', "Returning:\n" . var_export($return,true));              
	            return $return; 
	        }
	        else
	        { 
	            $return = array(true, self::adjustValue($data['methodResponse']['params']['param']['value']));
	           	//self::debug('XMLRPC_request', "Returning:\n\n" . var_export($return,true)); 
	            return $return; 
	        } 
	    } 
	} 
		
	/**
	 * Формирует ответную XML на удаленный вызов процедуры из вне
	 * @param $return_value - Данные для возврата
	 * @param $server
	 * @return XML-RPC responce string
	 */
	public static function Response($return_value, $server = NULL)
	{ 
		$return_value = self::Prepare($return_value);	
	    $data["methodResponse"]["params"]["param"]["value"] = &$return_value; 
	    $return = self::serialize($data);
	
	    //self::debug('XMLRPC_response', "Received the following data to return:\n" . var_export($return_value,true)); 
	     
	    header("Connection: close"); 
	    header("Content-Length: " . strlen($return)); 
	    header("Content-Type: text/xml"); 
	    header("Date: " . date("r")); 
	    if($server)
	    { 
	        header("Server: $server"); 
	    } 

	   	//self::debug('XMLRPC_response', "Sent the following response:\n\n" . $return); 
	    
	    return $return; 
	}  	 	

}

class Type
{
	const arr ="array";
	const int = "i4";
	const bool = "boolean";
	const double = "double";
	const str = "string";
	const struct = "struct";
}

?>
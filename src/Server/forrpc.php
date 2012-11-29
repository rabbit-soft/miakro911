<?php

include_once 'log4php/Logger.php';
include_once 'gamlib/pException.php';
include_once 'xmlrpc_methods.php';
include_once 'coder.php';
include_once 'config.php';

Logger::configure('log4php.xml');
$log = Logger::getLogger("main");
ini_set('display_errors', 0);//нужно для дебага
try
{
    $UID=-1;
	$request = Coder::Decrypt($HTTP_RAW_POST_DATA,$UID);
	$log->debug("Connected UID: ".$UID);
    //$log->debug("request: ".$request);
	if (!isset($UID) || $UID==-1) // проверка на пользователя
		throw new pException("Не верный пользователь",pErrCode::IncorrectUser);
    $params = xmlrpc_decode_request($request,$methodName);
    if($UID==0 && $methodName=="get.update.files")
    {
        $responce = MC::callMethod("get.update.files");
    }
    else
    {
        $responce = MC::callMethod($methodName,$params);
    }
    $result = XMLRPC_Response($responce);
    Conf::$LOG_QRS = false;
    $result =Coder::Encrypt($result);
    ob_clean(); //чтобы не было BOM
    header("Content-type: application/octet-stream; charset=utf-8");
	exit($result);
}
catch (pException $exc)
{
    $log->fatal($exc->getMessage());
    exit(Coder::Encrypt(XMLRPC_error($exc->getCode(), $exc->getMessage())));
}
catch (DecryptionException $exc)
{
    $log->fatal($exc->getMessage());
	exit("fuck you");
}
catch (Exception $exc)
{
	$log->fatal($exc->getMessage());
	exit(Coder::Encrypt(XMLRPC_error(pErrCode::ServerError, $exc->getMessage())));
}

////END MAIN

function XMLRPC_Response($data)
{
    return xmlrpc_encode_request(null,$data,array("encoding" => "utf-8","escaping"=>"markup"));//TODO кодировка рабочего сервера
}

function XMLRPC_Error($code,$msg)
{
    $err = array("faultCode"=>$code,
        "faultString"=>$msg);
    return XMLRPC_Response($err);
}

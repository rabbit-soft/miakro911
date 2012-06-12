<?php

include_once 'log4php/Logger.php';
include_once 'gamlib/pException.php';
include_once 'gamlib/libxmlrpc.php';
include_once 'xmlrpc_methods.php';
include_once 'coder.php';
include_once 'config.php';

Logger::configure('log4php.xml');
$log = Logger::getLogger("main");

ini_set('display_errors', 0);//нужно для дебага
try
{
	$request = Coder::Decrypt($HTTP_RAW_POST_DATA,$UID);
	//$log->debug("Request".$request);
	$log->debug("Connected UID: ".$UID."\n request:\n".$request);
	if ($UID == 0 or !isset($UID)) // проверка на пользователя 
		throw new pException("Не верный пользователь",pErrCode::IncorrectUser);
	
	$xmlrpc_request = XMLRPC::Parse($request); 
	$methodName = XMLRPC::GetMethodName($xmlrpc_request); 
	$params = XMLRPC::GetParams($xmlrpc_request); 
	
	//if($methodName!='user.genkey' and ServerFunc::NeedPassChange())
		//exit(Coder::Encrypt(XMLRPC::error(6, "Для продолжения работы необходимо назначить новый пароль")));
		
	$result = MC::callMethod($methodName,$params);
    $result = XMLRPC::Response($result);    $log->debug("RESPONCE ".$result);
    $result = Coder::Encrypt($result);      $log->debug("chipher ".$result);
	exit($result);
}
catch (pException $exc)
{
    $log->fatal($exc->getMessage());
    echo Coder::Encrypt(XMLRPC::error($exc->getCode(), $exc->getMessage()));
}
catch (DecryptionException $exc)
{
    $log->fatal($exc->getMessage());
	echo "fuck you";
}
catch (Exception $exc)
{
	$log->fatal($exc->getMessage());
	echo Coder::Encrypt(XMLRPC::error(pErrCode::ServerError, $exc->getMessage()));
}

?>
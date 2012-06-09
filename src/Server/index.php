<?php

include_once 'log4php/Logger.php';
include_once 'gamlib/pException.php';
require_once 'gamlib/libxmlrpc.php';
require_once 'xmlrpc_methods.php';
require_once 'coder.php';

Logger::configure('log4php.xml');
$log = Logger::getLogger("main");

ini_set('display_errors', 1);//нужно для дебага
try
{
	$request = Coder::Decrypt($HTTP_RAW_POST_DATA,$UID);
	$log->debug("Request".$request);
	$log->info("Connected UID: ".$UID);
	if ($UID == 0 or !isset($UID)) // проверка на пользователя 
		throw new pException("Не верный пользователь",pErrCode::IncorrectUser);
	
	$xmlrpc_request = XMLRPC::Parse($request); 
	$methodName = XMLRPC::GetMethodName($xmlrpc_request); 
	$params = XMLRPC::GetParams($xmlrpc_request); 
	
	//if($methodName!='user.genkey' and ServerFunc::NeedPassChange())
		//exit(Coder::Encrypt(XMLRPC::error(6, "Для продолжения работы необходимо назначить новый пароль")));
		
	$result = MC::callMethod($methodName,$params);
    $result = XMLRPC::Response($result);
    $result = Coder::Encrypt($result);
	header("Content-type: application/octet-stream");
	header("Content-length:".strlen($result));
	
	//$log->debug($result);
	//$log->debug('SIZEOF\n'.strlen($result));
	echo $result;
}
catch (pException $exc)
{
    $log->fatal($exc->getMessage());
    echo Coder::Encrypt(XMLRPC::error($exc->getCode(), $exc->getMessage()));
}
catch (DecryptionException $exc)
{
	$log->fatal($exc->getMessage());
	//echo Coder::Encrypt(XMLRPC::error(5, $exc->getMessage()));
	echo XMLRPC::error(5, $exc->getMessage());
}
catch (Exception $exc)
{
	$log->fatal($exc->getMessage());
	echo Coder::Encrypt(XMLRPC::error(pErrCode::ServerError, $exc->getMessage()));
}

?>
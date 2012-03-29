<?php
include_once 'log4php/Logger.php';
require_once 'libxmlrpc.php';
require_once 'xmlrpc_methods.php';
require_once 'coder.php';

Logger::configure('log4php.xml');
$log = Logger::getLogger("main");

ini_set('display_errors', 1);//нужно для дебага

try
{
	$request = Coder::Decrypt($HTTP_RAW_POST_DATA,$UID);
	$log->debug("Request".$request);
	$log->debug("Connected UID: ".$UID."\n request: ".$request);
	if ($UID == 0 or !isset($UID)) // проверка на пользователя 
		 exit(Coder::Encrypt(XMLRPC::error(4, "Не верный пользователь")));
	
	$xmlrpc_request = XMLRPC::parse($request); 
	$methodName = XMLRPC::getMethodName($xmlrpc_request); 
	$params = XMLRPC::getParams($xmlrpc_request); 
	
	if($methodName!='user.genkey' and ServerFunc::NeedPassChange())
		exit(Coder::Encrypt(XMLRPC::error(6, "Для продолжения работы необходимо назначить новый пароль")));
		
	$result = Coder::Encrypt(MC::callMethod($methodName,$params));
	header("Content-type: application/octet-stream");
	header("Content-length:".strlen($result));
	
	//$log->debug($result);
	//$log->debug('SIZEOF\n'.strlen($result));
	echo $result;
}
catch (Exception $exc)
{
	$log->fatal($exc->getMessage());
	echo Coder::Encrypt(XMLRPC::error(2, $exc->getMessage()));
}
catch (DecryptionException $exc)
{
	$log->fatal($exc->getMessage());
	echo Coder::Encrypt(XMLRPC::error(5, $exc->getMessage()));
}
/*
include_once 'log4php/Logger.php';

Logger::configure('log4php.xml');
$log = Logger::getLogger("main");

$log->trace("My first message.");   // Not logged because TRACE < WARN
$log->debug("My second message.");  // Not logged because DEBUG < WARN
$log->info("My third message.");    // Not logged because INFO < WARN
$log->warn("My fourth message.");   // Logged because WARN >= WARN
$log->error("My fifth message.");   // Logged because ERROR >= WARN
$log->fatal("My sixth message.");   // Logged because FATAL >= WARN
echo "ok";*/
?>
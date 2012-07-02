<?php
include_once 'log4php/Logger.php';
include_once "gamlib/libxmlrpc.php";
include_once "xmlrpc_methods.php";
include_once "config.php";
include_once "coder.php";

ini_set('display_errors', 1);//нужно для дебага
Logger::configure('log4php.xml');
$log = Logger::getLogger("test");
$UID=1;
//XMLRPC::debug("fuck", "you");
$request ='<?xml version="1.0" encoding="utf-8"?>
<methodCall>
  <methodName>get.update.files</methodName>
  <params />
</methodCall>';
$xmlrpc_request = XMLRPC::Parse($request);
$methodName = XMLRPC::GetMethodName($xmlrpc_request);
$params = XMLRPC::GetParams($xmlrpc_request);

//$params = array ('"rkbtyn"','jcnfg','nfv','1');
  /*0 => 'AAAAAAdng4FPGTkkxGCNqyh1T9WbnU5/7gUrJ14AAAAAAAAAAAAAAAAAAAAA',
  1 => '1',
  2 => '10',
  3 => '0',
  4 => '2012-04-02',
  5 => '2012-04-02',
  6 => '5555'
); */
$result = MC::callMethod($methodName,$params);
$result = XMLRPC::Response($result);
$result = Coder::Encrypt($result);
echo $result;

//vecho($arr);
?>
<?php
include_once 'log4php/Logger.php';
include_once "gamlib/libxmlrpc.php";
include_once "xmlrpc_methods.php";
include_once "config.php";

Logger::configure('log4php.xml');
$log = Logger::getLogger("test");
$UID=5;
ini_set('display_errors', 1);
//XMLRPC::debug("fuck", "you");
$request ="<methodCall>
  <methodName>client.money.add</methodName>
  <params>
    <param>
      <value>
        <string>6</string>
      </value>
    </param>
    <param>
      <value>
        <string>10000</string>
      </value>
    </param>
  </params>
</methodCall>";
$xmlrpc_request = XMLRPC::Parse($request);
$methodName = XMLRPC::GetMethodName($xmlrpc_request);
$params = XMLRPC::GetParams($xmlrpc_request);

$params = array ('"rkbtyn"','jcnfg','nfv','1');
  /*0 => 'AAAAAAdng4FPGTkkxGCNqyh1T9WbnU5/7gUrJ14AAAAAAAAAAAAAAAAAAAAA',
  1 => '1',
  2 => '10',
  3 => '0',
  4 => '2012-04-02',
  5 => '2012-04-02',
  6 => '5555'
); */
echo MC::callMethod("clients.get");

//vecho($arr);
?>
<?php

include_once 'php/ServFunc.php';
include_once 'php/ServFunc.php';
include_once 'php/RabUsers.php';

session_start();

ini_set("display_errors",0);
Logger::configure(Conf::log4php_conf("admin"));
$log = Logger::getLogger("main");

if(!isset($_SESSION["u_id"]) || RabUsers::GetType($_SESSION["u_id"])!=UserType::ADMIN)
{
    header("Location: ./index.php");
    exit();
}


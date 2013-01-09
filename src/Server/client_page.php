<?php

include_once 'php/ServFunc.php';
include_once 'php/ServFunc.php';
include_once 'php/RabUsers.php';

session_start();

ini_set("display_errors",0);
Logger::configure(Conf::log4php_conf("client"));
$log = Logger::getLogger("main");

if(!isset($_SESSION["u_id"]) || RabUsers::GetType($_SESSION["u_id"])!=UserType::USER)
{
    header("Location: ./index.php");
    exit();
}

echo '<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Статистика фермы</title>
    </head>
    <body bgcolor="CCFFCC">';
echo '<p align="center">Доступные БазыДанных</p><form name="frmTest" action="forrpc.php" method=POST>
		<p align="center"><select name="checkeddb" style="width : 200" onChange="frmTest.submit()";>';
$dbs = ServFunc::GetWebRep_DBs($_SESSION["farmname"]);
//if(isset($_POST["checkeddb"]))
//echo '<option value="" SELECTED>'.$_POST["checkeddb"]."</option>";
foreach ($dbs as $row)
{
    echo '<option value="'.$row.'"';
    if($row == $_POST["checkeddb"])
        echo " SELECTED";
    echo '>'.$row.'</option>';
}
echo '</select></p></form>';

if(isset($_POST["checkeddb"]))
    $farm = $_POST["checkeddb"];
else $farm = $dbs[0];
$table = ServFunc::GetWebRep_Global($_SESSION["farmname"],$farm);
echo '<table border="1" align="center" bordercolor=black cellspacing="0"><tr bgcolor="CCCC33">
		<td>Дата</td>
		<td>Случки</td>
		<td>Окролы</td>
		<td>Прохолосты</td>
		<td>Рождено</td>
		<td>Забито</td>
		<td>Падеж</td>
		<td>Всего кроликов</td>
	</tr>';
$gstr = "";
$gdt ="";
$grab = 0;
foreach($table as $row)
{
    if($gdt == "")
        $gdt = $row["dt"];
    $lstr = '<tr>';
    if(substr($gdt, 3,2) != substr($row["dt"], 3,2))
    {
        $lstr .= "<td><b>".$row["dt"]."</b></td>";
        $gdt = $row["dt"];
    }
    else $lstr .= "<td>".$row["dt"]."</td>";

    $lstr .= "<td>".$row["fucks"]."</td>";
    $lstr .= "<td>".$row["okrols"]."</td>";
    $lstr .= "<td>".$row["proholosts"]."</td>";
    $lstr .= "<td>".$row["born"]."</td>";
    $lstr .= "<td>".$row["killed"]."</td>";
    $lstr .= "<td>".$row["deads"]."</td>";

    $lstr .= '<td align="center"';
    if($grab >$row["rabbits"]*1)
        $lstr .= ' bgcolor="FF6666"';
    else if($grab <$row["rabbits"]*1)
        $lstr .= ' bgcolor="66FF99"';
    $lstr .= '>'.$row["rabbits"].'</td>';

    $grab = $row["rabbits"]*1;
    $lstr .= '</tr>';
    $gstr = $lstr.$gstr;
}
echo $gstr;

echo '</table></body></html>';
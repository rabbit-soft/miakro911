<?php
function show_loginpage()
{
	echo file_get_contents("HTMLS/login_top");
	if(isset($_SESSION["error"]))
		echo "<p align=\"center\" style=\"color:red\" >".$_SESSION["error"]."</p>";
	echo file_get_contents("HTMLS/login_bottom");
	unset($_SESSION["error"]);
	exit();
}

session_start();
include 'DBworker.php';
if(!isset($_SESSION["farmname"]))
{
	if(isset($_POST["login"]) or isset($_POST["pass"]))
	{
		if($_POST["login"]=="" or $_POST["pass"]== "")
		{
			$_SESSION["error"] = "Имя пользователя и Пароль не должны быть пустыми";
			show_loginpage();
		}
		$farm = DBworker::GetUserFarm($_POST["login"], $_POST["pass"]);
		if($farm == "")
		{	
			$_SESSION["error"] = "Не верный логин или пароль";	
			show_loginpage();	
		} 	
		$_SESSION["farmname"] = $farm;	
	}
	else show_loginpage();
}

///если пользователь все таки ввел пароль и он подошел, то далее рисуем таблицу

echo file_get_contents("HTMLS/reports_top");
echo '<p align="center">Доступные БазыДанных</p><form name="frmTest" action="index.php" method=POST>
		<p align="center"><select name="checkeddb" style="width : 200" onChange="frmTest.submit()";>';
$dbs = DBworker::GetWebRep_DBs($_SESSION["farmname"]);
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
$table = DBworker::GetWebRep_Global($_SESSION["farmname"],$farm); 
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
echo "</table>";

echo file_get_contents("HTMLS/reports_bottom");
?>
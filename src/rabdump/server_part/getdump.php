<?php
	$filepath="uploads/".$_POST["farm"]."/dump/".$_POST["file"];
	if(!file_exists($filepath))
	{
		echo "[File Not Exists]";
		exit();
	}
	$filesize = filesize($filepath);
	if(!isset($_POST["offset"]))
		{$offset = 0;}
	else {$offset = $_POST["offset"];}
	header("Content-type: application/octet-stream");
	header("Content-disposition: attachment;filename=".$_POST["file"]);
	header("Content-length:".($filesize-$offset));
	header("Content-MD5:".md5_file($filepath));
	
	echo file_get_contents($filepath,null,null,$offset)
	//yb;t bvbnfwbz vtlktyyjuj cjtlbytybz
	/*$len = 50000;
	while( $filesize-$offset > $len )
	{
		echo file_get_contents($filepath,null,null,$offset,$len);
		$offset += $len;
		sleep(1);
	}
	$len = $filesize-$offset;
	echo file_get_contents($filepath,null,null,$offset,$len);*/
?>
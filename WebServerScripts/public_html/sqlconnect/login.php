<?php

    $servername = "localhost";
	$username =  "id12719620_adminaroffice";
	$password = "1234Five";
	$dbName = "id12719620_dbaroffice";
	
	//Make Connection
	$con = mysqli_connect($servername, $username, $password, $dbName);

	if (mysqli_connect_error())
	{
		echo "1"; //error code #1 - connection failed
		exit();
	}
	
	$username = $_POST["name"];
	$password = $_POST["password"];
	
	//already exist check
	
	$namecheckquery = "SELECT username , salt , hash , score FROM players WHERE username = '". $username . "';";
	$namecheck = mysqli_query($con,$namecheckquery)or die("2");//name check query fail
	
	if (mysqli_num_rows($namecheck)==0)
	{
		echo"User Doen't Exist! Please Signup First!!"; //error code 5
		exit();
	}
	//get login info 
	$existinginfo = mysqli_fetch_assoc($namecheck);
	
	
	$salt=$existinginfo["salt"];
	$hash=$existinginfo["hash"];
	
	$loginhash = crypt($password, $salt);
	
	if ($hash != $loginhash )
	{
		
		echo "Password is not correct!";
		exit();
		
	}
	echo "0";
?>
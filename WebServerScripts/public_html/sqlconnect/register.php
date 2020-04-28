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
	
	$name = $_POST["name"];
	$username = $_POST["Username"];
	$password = $_POST["password"];
	
	//already exist check
	
	$namecheckquery = "SELECT username FROM players WHERE username = '". $username . "';";
	$namecheck = mysqli_query($con,$namecheckquery)or die("2");//name check query fail
	
	if (mysqli_num_rows($namecheck)>0)
	{
		echo"Username not available!"; //error code 3 
	}
	else{
    	$salt = "\$5\$rounds=5000\$" . "SteamedHams" . $username . "\$";
    	$hash = crypt($password,$salt);
    	$insertuserquery = "INSERT INTO players (username,hash,salt,FullName) VALUES ('" . $username . "','" . $hash . "','" . $salt . "','" . $name . "');";
    	mysqli_query($con, $insertuserquery) or die("Server Error!");
	}
	echo "0";
	
?>
<?php
    error_reporting(0);
	$servername = "localhost";
	$username =  "id12719620_adminaroffice";
	$password = "1234Five";
	$dbName = "id12719620_dbaroffice";
	
	//Make Connection
	$conn = new mysqli($servername, $username, $password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$domain = mysqli_real_escape_string($conn, $_POST['domain']);
	if($domain != null)
	{
    	$sql = "SELECT * FROM questionset WHERE domain = '$domain'";
    	$result = mysqli_query($conn ,$sql);
    	if(mysqli_num_rows($result) > 0){
    	    echo mysqli_num_rows($result);
    		//show data for each row
    		while($row = mysqli_fetch_assoc($result)){
    			echo ";".$row['quesid'].",".$row['questext'].",".$row['noOfOpt'].",".$row['option1'].",".$row['option2']. ",".$row['option3'].",".$row['option4'].",".$row['correctOptionNo'].",".$row['domain'];
    		}
    	}
	}
	else{
	    header('HTTP/1.0 403 Forbidden');
        $contents = file_get_contents('./403.html', TRUE);
        exit($contents);
	}
?>
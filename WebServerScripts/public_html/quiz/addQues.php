<?php
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
	
	$question = mysqli_real_escape_string($conn, $_POST['question']);
    $domain = mysqli_real_escape_string($conn, $_POST['domain']);
    $option1 = mysqli_real_escape_string($conn, $_POST['option1']);
	$option2 = mysqli_real_escape_string($conn, $_POST['option2']);
	$option3 = mysqli_real_escape_string($conn, $_POST['option3']);
	$option4 = mysqli_real_escape_string($conn, $_POST['option4']);
	$correctOption = mysqli_real_escape_string($conn, $_POST['correctOption']);
	
	$sql = "SELECT * FROM questionset WHERE questext = '$question'" ;
	$result = mysqli_query($conn ,$sql);
	if(mysqli_num_rows($result) > 0){
        echo "1";
	}
	else{
	    $insertuserquery = "INSERT INTO questionset (quesid,questext,noOfOpt,option1,option2,option3,option4,correctOptionNo,domain) VALUES (null,'" . $question . "','4','" . $option1 . "','" . $option2 . "','" . $option3 . "','" . $option4 . "',$correctOption,'" . $domain . "');";
    	mysqli_query($conn, $insertuserquery) or die("-1");
    	echo "0";
	}
?>
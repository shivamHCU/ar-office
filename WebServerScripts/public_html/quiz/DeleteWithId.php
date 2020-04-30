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
	
    $quesid = mysqli_real_escape_string($conn, $_POST['quesid']);
	if($quesid != null)
	{
    	// sql to delete a record
    	$sql = "DELETE FROM questionset WHERE quesid = '$quesid' ";
        if (mysqli_query($conn, $sql)) {
            echo "0";
        }
        mysqli_close($conn);
	}
	else{
	    mysqli_close($conn);
	    header('HTTP/1.0 403 Forbidden');
        $contents = file_get_contents('./403.html', TRUE);
        exit($contents);
	}
?>
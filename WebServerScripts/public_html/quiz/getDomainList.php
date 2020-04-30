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
	
    $username = mysqli_real_escape_string($conn, $_POST['username']);
	if($username != null)
	{
    	$sql = "SELECT DISTINCT(domain) FROM questionset WHERE domain LIKE '$username%' ";
    	$result = mysqli_query($conn ,$sql);
    	if(mysqli_num_rows($result) > 0){
    	    while($row = mysqli_fetch_assoc($result)){
    		    echo ";".$row['domain'];
    	    }
    	}
    	else{
    	    echo "0";
    	}
	}
	else{
	    header('HTTP/1.0 403 Forbidden');
        $contents = file_get_contents('./403.html', TRUE);
        exit($contents);
	}
?>
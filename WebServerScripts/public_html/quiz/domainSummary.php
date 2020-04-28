<?php
    error_reporting(0);
    $servername = "localhost";
	$username =  "id12719620_adminaroffice";
	$password = "1234Five";
	$dbName = "id12719620_dbaroffice";
	
	function endsWith($string, $endString) 
    { 
        $len = strlen($endString); 
        if ($len == 0) { 
            return true; 
        } 
        return (substr($string, -$len) === $endString); 
    } 
    
	
	//Make Connection
	$conn = new mysqli($servername, $username, $password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	
	$domain = mysqli_real_escape_string($conn, $_POST['domain']);
	if(endsWith($domain,"ALL")){
	    $sql = "SELECT DISTINCT(domain) FROM questionset";
    	$result = mysqli_query($conn ,$sql);
    	if(mysqli_num_rows($result) > 0){
    	    while($row = mysqli_fetch_assoc($result)){
			    echo $row['domain'].";";
		    }
    	}
    	else{
    	    echo "You have not added any domain till now create a new domain by adding new question(s)";
    	}
	}	
	
	else if($domain != null)
	{
    	$sql = "SELECT * FROM questionset WHERE domain = '$domain'";
    	$result = mysqli_query($conn ,$sql);
    	if(mysqli_num_rows($result) > 0){
    	    echo mysqli_num_rows($result);
    	}
    	else{
    	    echo '0';
    	}
	}
	else{
	    header('HTTP/1.0 403 Forbidden');
        $contents = file_get_contents('./403.html', TRUE);
        exit($contents);
	}
?>
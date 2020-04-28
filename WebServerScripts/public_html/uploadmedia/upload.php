<?php
if(isset($_POST["submit"])) {
    $target_dir = "uploads/";
    $target_file = $target_dir . basename($_FILES["fileToUpload"]["name"]);
    $uploadOk = 1;
    $imageFileType = strtolower(pathinfo($target_file,PATHINFO_EXTENSION));
    // Check if image file is a actual image or fake image
    // Check if file already exists
    if (file_exists($target_file)) {
        $reutenurl = "Sorry, file already exists.";
        $uploadOk = 0;
    }
    // Check file size
    if ($_FILES["fileToUpload"]["size"] > 100000000) {
        $reutenurl = "Sorry, your file is too large.";
        $uploadOk = 0;
    }
    // Allow certain file formats
    if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
    && $imageFileType != "gif" && $imageFileType != "mp4" ) {
        $reutenurl = "Sorry, only JPG, JPEG, PNG &  files are allowed.";
        $uploadOk = 0;
    }
    // Check if $uploadOk is set to 0 by an error
    if ($uploadOk == 0) {
        $reutenurl = "Sorry, your file was not uploaded.";
    // if everything is ok, try to upload file
    } else {
        if (move_uploaded_file($_FILES["fileToUpload"]["tmp_name"], $target_file)) {
            $reutenurl = "https://shivamgangwar.000webhostapp.com/uploadmedia/uploads/". basename( $_FILES["fileToUpload"]["name"]);
        } else {
            $reutenurl +=  "Sorry, there was an error uploading your file.";
        }
    }
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
  <title>AR-OFFICE|MEDIA UPLAOD</title>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
  <link rel="stylesheet" type="text/css" href="mystyle.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
  <script src="myscripts.js"></script>
</head>
<body>
<!--
<body>
<form action="upload.php" method="post" enctype="multipart/form-data">
    Select image to upload:
    <input type="file" name="fileToUpload" id="fileToUpload">
    <input type="submit" value="Upload Image" name="submit">
</form>
</body>
-->

<form action="" method="post" enctype="multipart/form-data">
<div class="container">
<div class="row it">
<div class="col-sm-offset-1 col-sm-10" id="one">
<p>
Please upload files only in 'jpg', 'jpeg','png', 'mp4' and 'mkv' format.
</p><br>
<div class="row">
  <div class="col-sm-offset-4 col-sm-4 form-group">
    <h3 class="text-center">AR-OFFICE</h3>
  </div><!--form-group-->
</div><!--row-->
<div id="uploader">
<div class="row uploadDoc">
  <div class="col-sm-3">
    <div class="docErr">Please upload valid file</div><!--error-->
    <div class="fileUpload btn btn-orange">
      <img src="https://image.flaticon.com/icons/svg/136/136549.svg" class="icon">
      <span class="upl" id="upload">Upload Media</span>
        <input type="file" class="upload up" name="fileToUpload" id="fileToUpload" onchange="readURL(this);" /> 
    </div><!-- btn-orange -->
  </div><!-- col-3 -->
  <div class="col-sm-8">
    <input type="text" class="form-control" name="" placeholder="Retuned url.. (copy this url and paste in app.)" value = "<?php echo (isset($reutenurl))?$reutenurl:'';?>" >
  </div><!--col-8-->
  <div class="col-sm-1"><a class="btn-check"><i class="fa fa-times"></i></a></div><!-- col-1 -->
</div><!--row-->
</div><!--uploader-->
<div class="text-center">
<input type="submit" style="background-color: #4CAF50;border: none;color: white;padding: 16px 32px;text-decoration:non;
  margin: 4px 2px;cursor: pointer;" value="Upload Media" name="submit">

</div>
</div><!--one-->
</div><!-- row -->
</div><!-- container -->
</form>
</body>
</html>





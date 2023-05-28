<?php
     include('db.php');

     $userLogin = $_POST["Login"];
     $levelID = $_POST["LevelId"];
     $jsonFile = $_POST["JsonFile"];

     echo SaveLevelData($levelID, $userLogin, $jsonFile);
?>
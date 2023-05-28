<?php
     include('db.php');

     $userLogin = $_POST["Login"];
     $levelID = $_POST["LevelId"];
     $jsonFile = $_POST["JsonFile"];

     echo ApplyLevelData($levelID, $userLogin, $jsonFile);
?>
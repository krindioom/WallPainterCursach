<?php
    include('db.php');

    $userLogin = $_POST["Login"];
    $userPassword = $_POST["Password"];

    echo registerUser($userLogin, $userPassword);
?>
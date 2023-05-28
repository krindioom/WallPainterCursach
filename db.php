<?php
require './libs/rb-mysql.php';

R::setup( 'mysql:host=localhost;dbname=WallPainterDB', 'root', '' );

if(!R::testConnection())
{
    echo 'не удалось подключиться к бд';
    exit();
}

function registerUser($login, $password)
{
    $playerTable = R::dispense('player');
    $playerTable -> name = $login;
    $playerTable -> password = $password;
    $match = R::findOne( 'player', ' name = ? ', [ $login ] );

    if(isset($match))
    {
        return $login;
    }

    R::store($playerTable);

    echo "adsfgfhjhgfds $login";
    return "пользователь $login успешно добавлен";
}

function loginUser($login, $password)
{
    $match = R::findOne('player', 'name = ? AND password = ?', [$login, $password]);

    if(isset($match))
    {
        return $match;
    }

    return "неверный логин или пароль";
}

function ApplyLevelData($levelId, $login, $json)
{
    $note = R::findOne('levelsplayers', "player = ? AND level_id = ?", [$login, $levelId]);
    if(!$note)
    {
        $levelPrefsTable = R::dispense('levelsplayers');
        $levelPrefsTable->player = $login;
        $levelPrefsTable->levelId = $levelId;
        $levelPrefsTable->levelPrefs = $json;

        R::store($levelPrefsTable);
        return;
    }

    return $note->level_prefs;
}

function SaveLevelData($levelId, $login, $json)
{
    $note = R::findOne('levelsplayers', "player = ? AND level_id = ?", [$login, $levelId]);
    if($note)
    {
        // $levelPrefsTable = R::dispense('levelsplayers');
        // $levelPrefsTable->player = $login;
        // $levelPrefsTable->levelId = $levelId;
        $note->levelPrefs = $json;

        R::store($note);
        return "записано";
    }

    return "нихера не сохранилось";
}

?>
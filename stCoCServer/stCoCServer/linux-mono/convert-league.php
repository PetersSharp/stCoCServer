#!/usr/bin/php
<?php

    $db = new SQLite3("../coc.db") or die('Unable to open database');
    $res = $db->query("SELECT * FROM AllLeague") or die("Query list failed");
    while ($row = $res->fetchArray())
    {
        // echo ":: ".$row['id']." (".$row['name'].")(".$row['ico'].")\n";
        system("mv ".$row['ico'].".png ".$row['id'].".png");
    }

?>


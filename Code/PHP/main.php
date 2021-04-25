<?php
    require('Sample.php');

    
    $baseurl = "http://advancedapi.bazhuayu.com/"; //base_url  api base url, like http://advancedapi.octoparse.com/ (for China: http://advancedapi.bazhuayu.com/)
    $username = "your username"; //your username
    $password = "your password"; //your password
    $token_entity = GetToken($baseurl, $username, $password);
    $token_entity = json_decode($token_entity);
    $accessToken = $token_entity->access_token;
    $refreshToken = $token_entity->refresh_token;
    $groups = GetTaskGroups($baseurl, $accessToken);
    $groups = json_decode($groups, true);
    for ($i=0; $i<count($groups['data']); $i++) {
        $taskGroupId = $groups['data'][$i]['taskGroupId'];
        $taskGroupName = $groups['data'][$i]['taskGroupName'];
        $tasks = GetTasksByGroup($baseurl, $accessToken, $taskGroupId);
        $tasks = json_decode($tasks, true);
        for ($j=0; $j<count($tasks['data']); $j++) {
            $taskId = $tasks['data'][$j]['taskId'];
            $taskName = $tasks['data'][$j]['taskName'];
            echo "taskGroupId: ".$taskGroupId."; taskGroupName: ".$taskGroupName."; taskId: ".$taskId."; taskName: ".$taskName."<br>";
        }
    }
?>

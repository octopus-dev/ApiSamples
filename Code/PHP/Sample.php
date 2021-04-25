<?php
	require('util.php');

	/**
 	* Obtain a new token
 	* @param string $username  //username
 	* @param string $password  //password
 	* @return string Token object
 	*/
	function GetToken($baseurl, $username, $password)
	{
		if ($baseurl != null && $username != null && $password != null)
		{
			$post_data = array(
				'username' => $username,
				'password' => $password,
				'grant_type' => "password"
			);
			$url = $baseurl."token";
			$result = send_post($url, $post_data);
			return $result;
		}
	}

	/**
 	* Refresh Token
 	* @param string $refreshToken  //Token to refresh Access Token
 	* @return string Token object
 	*/
	function RefreshToken($baseurl, $refreshToken){
		if ($baseurl != null && $refreshToken != null)
		{
			$post_data = array(
				'refresh_token' => $refreshToken,
				'grant_type' => "refresh_token"
			);
			$url = $baseurl."token";
			$result = send_post($url, $post_data);
			return $result;
		}
	}

	/**
 	* List all task groups
 	* @param string $accessToken  //Access Token
 	* @return string Task group list
 	*/
	function GetTaskGroups($baseurl, $accessToken){
		if ($baseurl != null && $accessToken != null)
		{
			$url = $baseurl."api/TaskGroup";
			$result = request_get($url, $accessToken);
			return $result;
		}
	}

	/**
 	* List all tasks in a group
 	* @param string $accessToken  //Access Token
	* @param string $groupId  //Task group ID
 	* @return string Task list
 	*/
	function GetTasksByGroup($baseurl, $accessToken, $groupId){
		if ($baseurl != null && $accessToken != null && $groupId != null)
		{
			$url = $baseurl."api/Task?taskGroupId=".$groupId;
			$result = request_get($url, $accessToken);
			return $result;
		}
	}

	/**
 	* Remove data by task
 	* @param string $accessToken  //Access Token
	* @param string $taskId  //Task Id
 	*/
	function RemoveTaskData($baseurl, $accessToken, $taskId){
		if ($baseurl != null && $accessToken != null && $taskId != null)
		{
			$url = $baseurl."api/task/RemoveDataByTaskId?taskId=".$taskId;
			$result = request_post($url, $accessToken,"");
			return $result;

		}
	}

	/**
 	* Export non-exported data
 	* @param string $accessToken  //Access Token
	* @param string $taskId  //Task Id
	* @param string $size  //The amount of data (range from 1 to 1000)
 	* @return string TaskDataExportResult object
 	*/
	 function ExportNotExportedData($baseurl, $accessToken, $taskId, $size){
		if ($baseurl != null && $accessToken != null && $taskId != null && $size > 0)
		{

			$url = $baseurl."api/notexportdata/gettop?taskId=".$taskId."&size=".$size;
			$result = request_get($url, $accessToken);
			return $result;

		}
	}

	/**
 	* Mark data exported
 	* @param string $accessToken  //Access Token
	* @param string $taskId  //Task ID
 	*/
	 function MarkDataExported($baseurl, $accessToken, $taskId){
		if ($baseurl != null && $accessToken != null && $taskId != null)
		{

			$url = $baseurl."api/notexportdata/update?taskId=".$taskId;
			$result = request_post($url, $accessToken,"");
			return $result;

		}
	}

	/**
 	* Get data by offset
 	* @param string $accessToken  //Access Token
	* @param string $taskId  //Task ID
	* @param string $offset  //Offset, if offset is less than or equal to 0, data will be returned starting from the first row
	* @param string $size  //The amount of data that will be returned (range from 1 to 1000)
 	* @return string TaskDataOffsetResult object
 	*/
	function GetTaskDataByOffset($baseurl, $accessToken, $taskId, $offset, $size){
		if ($baseurl != null && $accessToken != null && $taskId != null)
		{

			$url = $baseurl."api/alldata/GetDataOfTaskByOffset?taskId=".$taskId."&offset=".$offset."&size=".$size;
			$result = request_get($url, $accessToken);
			return $result;

		}
	}

?>
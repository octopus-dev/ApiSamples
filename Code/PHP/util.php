<?php
/**
 * send post
 * @param string $url 
 * @param array $post_data post
 * @return string
 */
function send_post($url, $post_data) {
	$postdata = http_build_query($post_data);
	$options = array(
		'http' => array(
			'method' => 'POST',
			'header' => 'Content-type:application/x-www-form-urlencoded',
			'content' => $postdata,
			'timeout' => 15 * 60 
		)
	);
	$context = stream_context_create($options);
	$result = file_get_contents($url, false, $context);
	return $result;
}

/**
 * Curl get
 * @param string $url 
 * @param string $token 
 * @return string
 */
function request_get($url, $token) {
  $ch = curl_init($url); // Initialise cURL
  $authorization = "Authorization: Bearer ".$token; // Prepare the authorisation token
  curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json' , $authorization )); // Inject the token into the header
  curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
  $result = curl_exec($ch);
  curl_close($ch);
  return $result; // Return the received data
}

/**
 * Curl post
 * @param string $url
 * @param string $token 
 * @param array $post_data post
 * @return string
 */
function request_post($url, $token, $post_data) {
	$postdata = http_build_query($post_data);
	$ch = curl_init($url); 
	$authorization = "Authorization: Bearer ".$token; // Prepare the authorisation token
	curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json' , $authorization )); // Inject the token into the header
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($ch, CURLOPT_POST, 1); // Specify the request method as POST
	curl_setopt($ch, CURLOPT_POSTFIELDS, 'mypost=' . $postdata);
	curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 1); // This will follow any redirects
	$result = curl_exec($ch); // Execute the cURL statement
	curl_close($ch); // Close the cURL connection
	return $result; // Return the received data
}

?>
package APIDemo;

/**   
 * @Project: 		Skieer BI
 * @Package: 		skieer.test 
 * @Description:		TODO
 * @author 			
 * @version 			V1.0.0
 * @date 			3:06:33 afternoon on September 6, 2016 
 * @copyright Copyright 2014(c) Octopus Data Inc.
 */

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;
import java.util.List;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * @ClassName: APIDemo
 * @Description: TODO
 * @author 
 */
public class APIDemo {
	public static void main(String args[]) {
		
		String userName = "";	  //Octoparse Username
		String passWord = "";		//Octoparse Password	
		
		//Access token
		String tokenUrl = "http://dataapi.octoparse.com/token";	
		String getToken = GetToken(userName, passWord, tokenUrl);
		System.out.println("Token is:" + getToken); 
		System.out.println("----------------------------------");

		//Regular processing access token
		String tokenRegex=":\"(.*)\",\"t";
		Pattern p=Pattern.compile(tokenRegex);
		Matcher m = p.matcher(getToken);
		String token="";
		while(m.find()){  
			token="bearer "+m.group(1);
		}
		
		//Get the task group list by task group interface demo
		String taskGroupUrl="http://dataapi.octoparse.com/api/taskgroup";
		String getTaskGroup=LoadTaskGroup(token, taskGroupUrl);
		System.out.println("TaskGroup is :"+getTaskGroup);
		System.out.println("----------------------------------");
		
		//Get the task list by task group id and token, taskGroupID task group id for the users themselves
		String taskGroupId="";  //Task group id, Get set up
		String taskIdUrl="http://dataapi.octoparse.com/api/task?taskgroupid="+taskGroupId;
		String getTask=LoadTaskGroup(token, taskIdUrl);
		System.out.println("TaskId is :"+getTask);
		System.out.println("----------------------------------");
		
		//Get AllData, taskID tasks for the user ID, parameter set for themselves
		String taskId="";   //task id,Get set up
		int pageindex=1;    	//Your Settings
		int pagesize=3;		//Your Settings
		String precedAllDataUrl="http://dataapi.octoparse.com/api/alldata";
		String allDataparam="taskid="+taskId+"&pageindex="+pageindex+
				"&pagesize="+pagesize;
		String getAllData=sendGet(precedAllDataUrl, allDataparam, token);
		System.out.println("All Data is :"+getAllData);
		System.out.println("----------------------------------");
		
		//Access to notexportdata
		int size=2;   //Your Settings
		String notExportDataUrl="http://dataapi.octoparse.com/api/notexportdata";
		String notExportparam="taskid="+taskId+"&size="+size;
		String getNotExportData=sendGet(notExportDataUrl, notExportparam, token);
		System.out.println("Not Export Data is"+getNotExportData);
		
	}
	
	public static String LoadTaskGroup(String token,String taskgroupUrl) {
		String result = "";
		BufferedReader in = null;
		try {
			URL realUrl = new URL(taskgroupUrl);
			// The connection between the opening and the URL
			URLConnection connection = realUrl.openConnection();
			// Set the request of the general properties
			connection.setRequestProperty("accept", "application/json");
			connection.setRequestProperty("connection", "Keep-Alive");
			connection.setRequestProperty("user-agent",
					"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			connection.setRequestProperty("Authorization", token);
			// To establish the actual connection
			connection.connect();
			// Get all the response header fields
			Map<String, List<String>> map = connection.getHeaderFields();
			// Iterate through all the response header fields
			for (String key : map.keySet()) {
				//System.out.println(key + "--->" + map.get(key));
			}
			// Define BufferedReader input stream to read the response of the URL
			in = new BufferedReader(new InputStreamReader(
					connection.getInputStream(),"UTF-8"));
					//connection.getInputStream()));
			String line;
			while ((line = in.readLine()) != null) {
				result += line;
			}
			//return result;
		} catch (Exception e) {
			System.out.println("Send a GET request an exception!" + e);
			e.printStackTrace();
		}
		// To use a finally block to close the input stream
		finally {
			try {
				if (in != null) {
					in.close();
				}
			} catch (Exception e2) {
				e2.printStackTrace();
			}
			return result;
		}
	}

	public static String GetToken(String userName, String password,
			String tokenUrl) {
		String token = null;
		if (null != userName && null != password && null != tokenUrl) {
			String postdata = String.format(
					"username=%s&password=%s&grant_type=password", userName,
					password);	
			String responseText = sendPost(tokenUrl,postdata);
			//System.out.println(responseText);
			if (responseText.contains("access_token")) {
				token = responseText;// JObject.Parse(responseText)["access_token"].ToString();
			}
		}
		return token;
	}

	public static String sendGet(String url, String param,String token) {
		String result = "";
		BufferedReader in = null;
		try {
			String urlNameString = url + "?" + param;
			//String bearerAccessToken="bearer "+token;
			//System.out.println(bearerAccessToken);
			URL realUrl = new URL(urlNameString);
			// The connection between the opening and the URL
			URLConnection connection = realUrl.openConnection();
			// Set the request of the general properties
			connection.setRequestProperty("accept", "application/json");
			connection.setRequestProperty("Authorization", token);
			connection.setRequestProperty("connection", "Keep-Alive");
			connection.setRequestProperty("user-agent",
					"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			// To establish the actual connection
			connection.connect();
			//Get all the response header fields
			Map<String, List<String>> map = connection.getHeaderFields();
			// Iterate through all the response header fields
			for (String key : map.keySet()) {
				//System.out.println(key + "--->" + map.get(key));
			}
			// Define BufferedReader input stream to read the response of the URL
			in = new BufferedReader(new InputStreamReader(
					connection.getInputStream(),"UTF-8"));
			String line;
			while ((line = in.readLine()) != null) {
				result += line;
			}
		} catch (Exception e) {
			System.out.println("Send a GET request an exception!" + e);
			e.printStackTrace();
		}
		// To use a finally block to close the input stream
		finally {
			try {
				if (in != null) {
					in.close();
				}
			} catch (Exception e2) {
				e2.printStackTrace();
			}
		}
		return result;
	}

	/**
	 * To specify the URL to send the request of the POST method
	 * 
	 * @param url
	 *            Send the request URL
	 * @param param
	 *            Request parameters, request parameters should be name1 = value1 & name2 = value2 form.
	 * @return Represents a remote resource response results
	 */
	public static String sendPost(String url, String param) {
		PrintWriter out = null;
		BufferedReader in = null;
		String result = "";
		try {
			URL realUrl = new URL(url);
			// The connection between the opening and the URL
			HttpURLConnection conn = (HttpURLConnection) realUrl
					.openConnection();
			// Set the request of the general properties
			conn.setRequestProperty("accept", "*/*");
			conn.setRequestProperty("Content-Type",
					"application/x-www-form-urlencoded");
			conn.setRequestProperty("connection", "Keep-Alive");
			conn.setRequestProperty("user-agent",
					"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			conn.setRequestMethod("POST");
			conn.setDoOutput(true);// Use the URL connection for output 
			conn.setDoInput(true);//Use the URL connection for input

			byte[] requestStringBytes = param.getBytes("utf-8");

			// To build the output stream and write data
			OutputStream outputStream = conn.getOutputStream();
			outputStream.write(requestStringBytes);
			outputStream.close();
			// Get the response status
			int responseCode = conn.getResponseCode();
			if (HttpURLConnection.HTTP_OK == responseCode) {// The connection is successful
				// When the correct response to process the data
				StringBuffer sb = new StringBuffer();
				String readLine;
				BufferedReader responseReader;
				//Process the response flow, must be consistent with the server response stream output coding
				responseReader = new BufferedReader(new InputStreamReader(
						conn.getInputStream(), "utf-8"));
				while ((readLine = responseReader.readLine()) != null) {
					sb.append(readLine).append("\n");
				}
				responseReader.close();
				result = sb.toString();

			}
		} catch (Exception e) {
			System.out.println("Send a POST request is abnormal!" + e);
			e.printStackTrace();
		}


		// Use a finally block to close the output stream and the input stream
		finally {
			try {
				if (out != null) {
					out.close();
				}
				if (in != null) {
					in.close();
				}
			} catch (IOException ex) {
				ex.printStackTrace();
			}
		}
		return result;
	}
}

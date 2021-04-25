package API;



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

import org.apache.commons.configuration.ConfigurationException;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;

import ReadConfig.ReadBzyXml;


public class APIDemoAdvanced
{

	public static void main(String args[]) throws Exception
	{
		//读取配置文件，增值接口的账号必须拥有权限
		ReadBzyXml config = new ReadBzyXml(); 
		String userName = config.getKeyName("userName"); 
		String passWord = config.getKeyName("passWord"); 

		//获取任务token
		String tokenUrl = config.getKeyName("advancedAPIToken");
		String getTokenResult = GetToken(userName, passWord, tokenUrl);
		System.out.println("八爪鱼Advanced接口Token:\r\n" + getTokenResult);
		System.out.println("----------------------------------");
		JSONObject getTokenJson=(JSONObject)JSON.parse(getTokenResult);
		String token=getTokenJson.getString("access_token");
		System.out.println("获取得Token为：\r\n"+token);
		System.out.println("----------------------------------");
		
		//刷新token，和重新获取token同理，如果需要请打开下方注释
		String refresh_token=getTokenJson.getString("refresh_token");
		System.out.println("刷新token需要的refresh_token为："+refresh_token);
		System.out.println("----------------------------------");
		/*
		String refreshURL=config.getKeyName("advancedAPIToken");
		String refreshParm=String.format("refresh_token=%s&grant_type=refresh_token", refresh_token);
		String refreshResult=sendPost(refreshURL, refreshParm);
		System.out.println("八爪鱼Advanced刷新后接口Token:\r\n"+refreshResult); */
		
		//启动任务接口，可以控制任务启动云采集
		String startTaskURL=String.format(config.getKeyName("advancedStartTask")+"=%s", config.getKeyName("taskIdAdvancedForTaskStatus"));
		String startTaskResult=sendPostForTask(startTaskURL, token);
		System.out.println("任务启动Result:\r\n"+startTaskResult);
		
		
		try
		{
			Thread.sleep(1000); 
		} catch (InterruptedException ex)
		{
			Thread.currentThread().interrupt();
		}

		
		//停止任务接口,控制任务停止
		String stopTaskURL=String.format(config.getKeyName("advancedStopTask")+"=%s",config.getKeyName("taskIdAdvancedForTaskStatus"));
		String stopTaskResult=sendPostForTask(stopTaskURL,token);
		System.out.println("任务停止Result:\r\n"+stopTaskResult);
		
		//根据TaskList获取任务状态
	    String getTaskStatusURL=config.getKeyName("advancedGetTaskStatusByIdList");
	    String demoListJson=String.format("{\r\n" + 
	    		"  \"taskIdList\": [\r\n" + 
	    		"    \"%s\",\r\n" + 
	    		"    \"%s\"\r\n" + 
	    		"  ]\r\n" + 
	    		"}", config.getKeyName("taskIdAdvancedForTaskStatus"),config.getKeyName("taskIdAdvancedForUpdateTask"));
	    String taskStatusResult=sendPostTaskStatus(getTaskStatusURL, demoListJson, token);
	    System.out.println("根据TaskList返回的Result：\r\n"+taskStatusResult);
	    
	    //根据taskId和流程名称返回参数
	    String getTaskPropertyURL=String.format(config.getKeyName("advancedGetTaskRulePropertyByName")+"?taskId=%s&name=%s"
	    		, config.getKeyName("taskIdAdvancedForUpdateTask"),config.getKeyName("property"));
	    String taskPropertyResult=sendPostForTask(getTaskPropertyURL, token);
	    System.out.println("任务流程配置参数：\r\n"+taskPropertyResult);
	    
	    //根据任务Id与名称，修改流程步骤参数值
	    String updateTaskURL=config.getKeyName("advancedUpdateTaskRule");
	    String updateTaskParm=String.format("{\r\n" + 
	    		"  \"taskId\": \"%s\",\r\n" + 
	    		"  \"name\": \"%s\",\r\n" + 
	    		"  \"value\": \"%s\"\r\n" + 
	    		"}", config.getKeyName("taskIdAdvancedForUpdateTask"),config.getKeyName("property"),
	    		"['www.baidu.com','www.bazhuayu.com']");
	    String updateTaskParmResult=sendPostTaskStatus(updateTaskURL, updateTaskParm, token);
	    System.out.println("修改任务流程参数值Result：\r\n"+updateTaskParmResult);
	    
	    //根据任务Id与名称，添加流程参数值
	    String addUrlOrTextToTaskURL=config.getKeyName("advancedAddUrlOrTextToTask");
	    String addTaskParm=String.format("{\r\n" + 
	    		"  \"taskId\": \"%s\",\r\n" + 
	    		"  \"name\": \"%s\",\r\n" + 
	    		"  \"value\": \"%s\"\r\n" + 
	    		"}", config.getKeyName("taskIdAdvancedForUpdateTask"),config.getKeyName("property"),
	    		"['www.sina.com']");
	   //System.out.println(addTaskParm);
	   String addTaskResult=sendPostTaskStatus(addUrlOrTextToTaskURL, addTaskParm, token);
	   System.out.println("添加任务参数项Result:\r\n"+addTaskResult);
	    
	   
	
		}


	public static String GetToken(String userName, String password, String tokenUrl)
	{
		String token = null;
		if (null != userName && null != password && null != tokenUrl)
		{
			String postdata = String.format("username=%s&password=%s&grant_type=password", userName, password);
			String responseText = sendPost(tokenUrl, postdata);
			if (responseText.contains("access_token"))
			{
				token = responseText;
			}
		}
		return token;
	}

	public static String sendGet(String url, String param, String token)
	{
		String result = "";
		BufferedReader in = null;
		try
		{
			String urlNameString = url + "?" + param;
			URL realUrl = new URL(urlNameString);
			URLConnection connection = realUrl.openConnection();
			connection.setRequestProperty("Accept", "application/json");
			connection.setRequestProperty("Authorization", token);
			connection.setRequestProperty("connection", "Keep-Alive");
			connection.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			connection.connect();
			Map<String, List<String>> map = connection.getHeaderFields();
			for (String key : map.keySet())
			{
				// System.out.println(key + "--->" + map.get(key));
			}
			in = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8"));
			String line;
			while ((line = in.readLine()) != null)
			{
				result += line;
			}
		} catch (Exception e)
		{
			System.out.println("Get请求异常：" + e);
			e.printStackTrace();
		}
		finally
		{
			try
			{
				if (in != null)
				{
					in.close();
				}
			} catch (Exception e2)
			{
				e2.printStackTrace();
			}
		}
		return result;
	}
	public static String sendPost(String url, String contentparam)
	{
		PrintWriter out = null;
		BufferedReader in = null;
		String result = "";
		try
		{
			URL realUrl = new URL(url);
			HttpURLConnection conn = (HttpURLConnection) realUrl.openConnection();
			conn.setRequestProperty("Accept", "application/json");
			conn.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			conn.setRequestProperty("connection", "Keep-Alive");
			conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			conn.setRequestMethod("POST");
			conn.setDoOutput(true);
			conn.setDoInput(true);

			byte[] requestStringBytes = contentparam.getBytes("utf-8");

			OutputStream outputStream = conn.getOutputStream();
			outputStream.write(requestStringBytes);
			outputStream.close();
			//״̬
			int responseCode = conn.getResponseCode();
			if (HttpURLConnection.HTTP_OK == responseCode)
			{
				StringBuffer sb = new StringBuffer();
				String readLine;
				BufferedReader responseReader;
				responseReader = new BufferedReader(new InputStreamReader(conn.getInputStream(), "utf-8"));
				while ((readLine = responseReader.readLine()) != null)
				{
					sb.append(readLine).append("\n");
				}
				responseReader.close();
				result = sb.toString();

			}
		} catch (Exception e)
		{
			System.out.println("POST异常：" + e);
			e.printStackTrace();
		}
		finally
		{
			try
			{
				if (out != null)
				{
					out.close();
				}
				if (in != null)
				{
					in.close();
				}
			} catch (IOException ex)
			{
				ex.printStackTrace();
			}
		}
		return result;
	}

	public static String sendPostForTask(String url, String tokenparam)
	{
		PrintWriter out = null;
		BufferedReader in = null;
		String result = "";
		String token=String.format("bearer %s", tokenparam);
		try
		{
			URL realUrl = new URL(url);
			HttpURLConnection conn = (HttpURLConnection) realUrl.openConnection();
			conn.setRequestProperty("Accept", "application/json");
			conn.setRequestProperty("Authorization", token);
			conn.setRequestProperty("connection", "Keep-Alive");
			conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			conn.setRequestMethod("POST");
			conn.setDoOutput(true);
			conn.setDoInput(true);

			byte[] requestStringBytes;
			OutputStream outputStream = conn.getOutputStream();
			outputStream.close();//״̬
			int responseCode = conn.getResponseCode();

			conn.connect();
			in = new BufferedReader(new InputStreamReader(conn.getInputStream(), "UTF-8"));
			String line;

			if (HttpURLConnection.HTTP_OK == responseCode)
			{
				StringBuffer sb = new StringBuffer();
				String readLine;
				BufferedReader responseReader;
				responseReader = new BufferedReader(new InputStreamReader(conn.getInputStream(), "utf-8"));
				while ((readLine = responseReader.readLine()) != null)
				{
					sb.append(readLine).append("\n");
				}
				responseReader.close();
				result = sb.toString();

			}

		} catch (Exception e)
		{
			System.out.println("POST请求异常：" + e);
			e.printStackTrace();
		}
		finally
		{
			try
			{
				if (out != null)
				{
					out.close();
				}
				if (in != null)
				{
					in.close();
				}
			} catch (IOException ex)
			{
				ex.printStackTrace();
			}
		}
		return result;
		
	}

	public static String sendPostTaskStatus(String url, String contentparam, String token)
	{
		PrintWriter out = null;
		BufferedReader in = null;
		String result = "";
		token=String.format("bearer %s", token);
		try
		{
			URL realUrl = new URL(url);
			HttpURLConnection conn = (HttpURLConnection) realUrl.openConnection();
			conn.setRequestProperty("Accept", "application/json");
			conn.setRequestProperty("Authorization", token);
			conn.setRequestProperty("Content-Type", "application/json");
			conn.setRequestProperty("connection", "Keep-Alive");
			conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			conn.setRequestMethod("POST");
			conn.setDoOutput(true);
			conn.setDoInput(true);

			byte[] requestStringBytes = contentparam.getBytes("utf-8");

			OutputStream outputStream = conn.getOutputStream();
			outputStream.write(requestStringBytes);
			outputStream.close();
			//״̬
			int responseCode = conn.getResponseCode();
			if (HttpURLConnection.HTTP_OK == responseCode)
			{
				StringBuffer sb = new StringBuffer();
				String readLine;
				BufferedReader responseReader;
				responseReader = new BufferedReader(new InputStreamReader(conn.getInputStream(), "utf-8"));
				while ((readLine = responseReader.readLine()) != null)
				{
					sb.append(readLine).append("\n");
				}
				responseReader.close();
				result = sb.toString();

			}
		} catch (Exception e)
		{
			System.out.println("POST请求异常：" + e);
			e.printStackTrace();
		}
		finally
		{
			try
			{
				if (out != null)
				{
					out.close();
				}
				if (in != null)
				{
					in.close();
				}
			} catch (IOException ex)
			{
				ex.printStackTrace();
			}
		}
		return result;
	}
	public static String updateGetFor(String url, String param,String token) {
		PrintWriter out = null;
		BufferedReader in = null;
		String result = "";
		try {
			URL realUrl = new URL(url);
			HttpURLConnection conn = (HttpURLConnection) realUrl.openConnection();

			conn.setRequestProperty("accept", "application/json");
			conn.setRequestProperty("Content-Type",
					"application/json");
			conn.setRequestProperty("Authorization", token);
			conn.setRequestProperty("connection", "Keep-Alive");
			conn.setRequestProperty("user-agent",
					"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			conn.setRequestMethod("POST");
			conn.setRequestProperty("Authorization", token);
			conn.setDoOutput(true);
			conn.setDoInput(true);

			byte[] requestStringBytes = param.getBytes("utf-8");

	
			OutputStream outputStream = conn.getOutputStream();
			outputStream.write(requestStringBytes);
			outputStream.close();
			//״̬
			int responseCode = conn.getResponseCode();
			if (HttpURLConnection.HTTP_OK == responseCode) {		
				StringBuffer sb = new StringBuffer();
				String readLine;
				BufferedReader responseReader;
				responseReader = new BufferedReader(new InputStreamReader(
						conn.getInputStream(), "utf-8"));
				while ((readLine = responseReader.readLine()) != null) {
					sb.append(readLine).append("\n");
				}
				responseReader.close();
				result = sb.toString();

			}
		} catch (Exception e) {
			System.out.println("POST请求异常：" + e);
			e.printStackTrace();
		}
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

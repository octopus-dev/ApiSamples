package API;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
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

public class APIDemo
{
	public static void main(String args[]) throws Exception
	{
		
		// 读取XML文件，仅供配置使用
		ReadBzyXml config = new ReadBzyXml();
		String userName = config.getKeyName("userName");
		String passWord = config.getKeyName("passWord");
  
		// 通过token接口，获取访问验证token，token有效期24小时
		String tokenURL = config.getKeyName("tokenURL");
		String jsonObject = GetToken(userName, passWord, tokenURL);
		JSONObject jsonString=(JSONObject)JSON.parse(jsonObject);
		String access_token=jsonString.getString("access_token");
		System.out.println("访问验证Token身份:\r\n"+access_token);
		System.out.println("----------------------------------");
		
		//通过token接口，使用refresh_token刷新access_token,如果需要重刷token请打开下方注释
		String refresh_token=jsonString.getString("refresh_token");
		System.out.println("用于刷新的refresh_token:\r\n"+refresh_token);
		System.out.println("----------------------------------");
		/*
		String param=String.format("refresh_token=%s&grant_type=refresh_token", refresh_token);
		String responseText=sendPost(tokenURL, param);
		System.out.println("刷新后的验证Token身份:\r\n"+responseText); */
	
		// 通过提供token获取任务组taskGroupId列表
		String taskGroupURL = config.getKeyName("taskGroupURL");	
		String getTaskGroup = LoadTask(access_token, taskGroupURL);
		System.out.println("任务组TaskGroupId列表:\r\n"+getTaskGroup);
		System.out.println("----------------------------------");
	
		//通过提供token、taskGroupId获取任务task列表,此处的taskGroupId为通过taskGroup接口获取后填入配置文件的，填入成功后，请打开下方注释
		String taskIdUrl =String.format("%s=%s", config.getKeyName("taskGroupIdURL"),config.getKeyName("taskGroupId"));
		/*
		String getTask = LoadTask(access_token, taskIdUrl);
		System.out.println("任务TaskId列表:\r\n" + getTask);
		System.out.println(); */
		
		//根据起始偏移量获取任务数据
		String offset=config.getKeyName("offSet");
		String getDataByOffsetParm=String.format("taskid=%s&offset=%s&size=%s", config.getKeyName("taskIdForOffset"),offset,config.getKeyName("size"));
		String getDataByOffsetResult=sendGet(config.getKeyName("getDataOfTaskByOffsetURL"),getDataByOffsetParm,access_token);
		System.out.println("根据起始偏移量offset=0获取的数据：\r\n"+getDataByOffsetResult);
		System.out.println("----------------------------------");
		
		//根据返回的offset以此类推获取以下数据,其他以此类推
		JSONObject offsetResultJson=(JSONObject)JSON.parse(getDataByOffsetResult);
		String dataString=offsetResultJson.getString("data");
		JSONObject dataJson=(JSONObject)JSON.parse(dataString);
		offset=dataJson.getString("offset");
		System.out.println("获取返回结果中的offset:"+offset);
		System.out.println("----------------------------------");
		getDataByOffsetParm=String.format("taskid=%s&offset=%s&size=%s", config.getKeyName("taskIdForOffset"),offset,config.getKeyName("size"));
		getDataByOffsetResult=sendGet(config.getKeyName("getDataOfTaskByOffsetURL"),getDataByOffsetParm,access_token);
		System.out.println("根据起始偏移量offset="+offset+"获取的数据：\r\n"+getDataByOffsetResult);
		System.out.println("----------------------------------");
		
		//导出一批任务数据
		String getTopParm=String.format("taskId=%s&size=%s", config.getKeyName("taskIdForGetTop"),config.getKeyName("size"));
		String getTopResult=sendGet(config.getKeyName("getTopURL"),getTopParm,access_token);
		System.out.println("导出一批任务数据：\r\n"+getTopResult);
		System.out.println("----------------------------------");
		
		//标记任务为已导出
		String updateURL=String.format(config.getKeyName("updateURL")+"%s",config.getKeyName("taskIdForGetTop"));
		String updateResult=sendPost(updateURL, access_token);
		System.out.println("将任务数据中处于正在导出状态的数据改为已导出：\r\n"+updateResult);
		
		//清除任务数据
		String removeDataURL=String.format(config.getKeyName("removeDataByTaskIdURL")+"%s", config.getKeyName("taskIdRemoveData"));
		String removeDataResult=sendPost(removeDataURL,access_token);
		System.out.println("清除数据接口结果：\r\n"+removeDataResult);
	}

	public static String LoadTask(String token, String taskgroupUrl)
	{
		token=String.format("bearer %s", token);
		String result = "";
		BufferedReader in = null;
		try
		{
			URL realUrl = new URL(taskgroupUrl);
			URLConnection connection = realUrl.openConnection();

			connection.setRequestProperty("accept", "application/json");
			connection.setRequestProperty("connection", "Keep-Alive");
			connection.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			connection.setRequestProperty("Authorization", token);
			connection.connect();

			Map<String, List<String>> map = connection.getHeaderFields();

			for (String key : map.keySet())
			{
				// System.out.println(key + "--->" + map.get(key));
			}

			in = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8"));
			// connection.getInputStream()));
			String line;
			while ((line = in.readLine()) != null)
			{
				result += line;
			}

		} catch (Exception e)
		{
			System.out.println("发送GET请求出现异常！" + e);
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
			return result;
		}
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
		System.out.println("八爪鱼返回的JSON:\r\n"+token);
		return token;
	}

	public static String sendGet(String url, String param, String token)
	{
		token=String.format("bearer %s", token);
		String result = "";
		BufferedReader in = null;
		try
		{
			String urlNameString = url + "?" + param;
			URL realUrl = new URL(urlNameString);
			URLConnection connection = realUrl.openConnection();
			connection.setRequestProperty("accept", "application/json");
			connection.setRequestProperty("Authorization", token);
			connection.setRequestProperty("connection", "Keep-Alive");
			connection.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			connection.connect();

			
			in = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8"));
			String line;
			while ((line = in.readLine()) != null)
			{
				result += line;
			}
		} catch (Exception e)
		{
			System.out.println("发送GET请求出现异常！" + e);
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


	public static String sendPost(String url, String param)
	{
		PrintWriter out = null;
		BufferedReader in = null;
		String result = "";
		try
		{
			URL realUrl = new URL(url);
			HttpURLConnection conn = (HttpURLConnection) realUrl.openConnection();
			conn.setRequestProperty("accept", "application/json");
			conn.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			conn.setRequestProperty("Authorization", "bearer "+param);
			conn.setRequestProperty("connection", "Keep-Alive");
			conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			conn.setRequestMethod("POST");
			conn.setDoOutput(true);
			conn.setDoInput(true);
			
			byte[] requestStringBytes = param.getBytes("utf-8");

			OutputStream outputStream = conn.getOutputStream();
			outputStream.write(requestStringBytes);
			outputStream.close();

			int responseCode = conn.getResponseCode();
			if (HttpURLConnection.HTTP_OK == responseCode)
			{

				StringBuffer stringb = new StringBuffer();
				String readLine;
				BufferedReader responseReader;
				responseReader = new BufferedReader(new InputStreamReader(conn.getInputStream(), "utf-8"));
				while ((readLine = responseReader.readLine()) != null)
				{
					stringb.append(readLine).append("\n");
				}
				responseReader.close();
				result = stringb.toString();

			}
		} catch (Exception e)
		{
			System.out.println("发送 POST 请求出现异常！" + e);
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

}

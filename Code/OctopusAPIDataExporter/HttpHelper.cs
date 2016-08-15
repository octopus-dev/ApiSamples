using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace OctopusAPIDataExporter
{
    class HttpHelper
    {
        public static string GetWithHeaders(string URL, Dictionary<string, string> headers)
        {
            try
            {
                // Get the response instance.
                WebRequest webReq = WebRequest.Create(URL);
                webReq.Method = "GET";
                foreach (var item in headers)
                {
                    webReq.Headers.Add(item.Key, item.Value);
                }
                StreamReader sr = new StreamReader(webReq.GetResponse().GetResponseStream());
                string srcString = sr.ReadToEnd();
                webReq.Abort();
                webReq = null;
                return srcString;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        ///<summary>
        ///采用POST方法
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        ///<returns></returns>
        public static string Post(string URL, string strPostdata, Dictionary<string, string> _headers = null)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                if (null != _headers)
                    foreach (var item in _headers)
                    {
                        webClient.Headers.Add(item.Key, item.Value);
                    }
                byte[] postData, responseData;
                string srcString;
                if (null == strPostdata)
                    strPostdata = "";
                postData = Encoding.UTF8.GetBytes(strPostdata);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
                responseData = webClient.UploadData(URL, "POST", postData);//得到返回字符流  
                srcString = Encoding.UTF8.GetString(responseData);//解码  
                webClient.Dispose();
                webClient = null;
                return srcString;
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}

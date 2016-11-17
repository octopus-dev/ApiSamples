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

        public static string Post(string URL, string strPostdata, Dictionary<string, string> _headers = null)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                if (null != _headers)
                    foreach (var item in _headers)
                    {
                        webClient.Headers.Add(item.Key, item.Value);
                    }
                byte[] postData, responseData;
                string srcString;
                if (null == strPostdata)
                    strPostdata = "";
                postData = Encoding.UTF8.GetBytes(strPostdata);
                responseData = webClient.UploadData(URL, "POST", postData);  
                srcString = Encoding.UTF8.GetString(responseData);
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

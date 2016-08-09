using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OctopusAPIDataExporter
{
    public struct Task
    {
        public string taskID;
        public string taskName;
    };
    public class TaskGroup
    {
        public string taskGroupID;
        public string taskGroupName;
        public List<Task> tasks;
    };
    public enum SaveFileFormat { JSON, XML };
    public enum ExportDataType { AllData, NotExportedData };
    public class APIUser
    {
        public string userName = null;
        public string password = null;
        public string tokenUrl = null;
        public SaveFileFormat saveFileFormat = SaveFileFormat.JSON;
        public ExportDataType exportDataType = ExportDataType.AllData;
        public string token = "";
        public List<TaskGroup> taskGroups;
        public APIUser()
        {
        }
        public APIUser(string usr, string pwd, string tokenurl)
        {
            userName = usr;
            password = pwd;
            tokenUrl = tokenurl;
        }

        /// <summary>
        /// 从Token的Url获取Token
        /// </summary>
        /// <returns>Token String</returns>
        public string GetToken()
        {
            if (null != userName && null != password && null != tokenUrl)
            {
                string postdata = string.Format("username={0}&password={1}&grant_type=password", userName, password);
                string responseText = HttpHelper.Post(tokenUrl, postdata);
                if (responseText.Contains("access_token"))
                {
                    token = JObject.Parse(responseText)["access_token"].ToString();
                }
            }
            return token;
        }

        /// <summary>
        /// 示例代码：用HTTP的POST方法提交username和password，通过tokenUrl接口来获取token
        /// </summary>
        /// <param name="userName">八爪鱼用户名</param>
        /// <param name="password">登录密码</param>
        /// <param name="tokenUrl">token接口地址，一般为http://ipaddress:9000/token</param>
        /// <returns></returns>
        public string GetToken(string userName, string password, string tokenUrl)
        {
            if (null != userName && null != password && null != tokenUrl)
            {
                string postdata = string.Format("username={0}&password={1}&grant_type=password", userName, password);
                string responseText = HttpHelper.Post(tokenUrl, postdata);
                if (responseText.Contains("access_token"))
                {
                    token = JObject.Parse(responseText)["access_token"].ToString();
                }
            }
            return token;
        }

    }
}

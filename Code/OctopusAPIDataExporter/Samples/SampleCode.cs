using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;

namespace OctopusAPIDataExporter.Samples
{
    public class SamplesCode
    {
        ///<summary>
        ///用POST方法提交username和password获取token
        ///</summary>
        ///<param name="userName">八爪鱼用户名</param>
        ///<param name="password">登录密码</param>
        ///<returns></returns>
        public string GetToken(string userName, string password)
        {
            string token = string.Empty;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                string postdata = string.Format("username={0}&password={1}&grant_type=password", userName, password);
                string responseText = HttpHelper.Post("http://dataapi.bazhuayu.com/token", postdata);
                if (responseText.Contains("access_token"))
                {
                    token = JObject.Parse(responseText)["access_token"].ToString();
                }
            }
            return token;
        }

        /// <summary>
        /// 通过token获取对应用户已分组的所有任务
        /// </summary>
        /// <param name="token">access token</param>
        /// <returns>已分组的所有任务</returns>
        public List<TaskGroup> GetTaskWithGroups(string token)
        {
            List<TaskGroup> taskGroups = new List<TaskGroup>();
            if (null != token)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", string.Format("bearer {0}", token));
                string TaskGroupsStr = HttpHelper.Get("http://dataapi.bazhuayu.com/api/taskgroup", headers);
                if (!TaskGroupsStr.Contains("success"))
                {
                    JArray jsonTaskGroupsJarray = JObject.Parse(TaskGroupsStr)["data"] as JArray;
                    foreach (JToken jtokenTaskGroup in jsonTaskGroupsJarray)
                    {
                        System.Threading.Thread.Sleep(5000); // sleep 5s just in case of exceeded quota
                        taskGroups.Add(new TaskGroup()
                        {
                            GroupId = jtokenTaskGroup["taskGroupId"].ToString(),
                            GroupName = jtokenTaskGroup["taskGroupName"].ToString(),
                            Tasks = GetTasks(token, jtokenTaskGroup["taskGroupId"].ToString())//将任务组中的任务数据同时获取到，此操作需要时间
                        });
                    }
                }
            }
            return taskGroups;
        }

        /// <summary>
        /// 获取某组的所有任务
        /// </summary>
        /// <param name="token">access token</param>
        /// <param name="taskGroupId">组Id</param>
        /// <returns></returns>
        public List<Task> GetTasks(string token, string taskGroupId)
        {
            List<Task> tasks = new List<Task>();
            if (null != token)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", string.Format("bearer {0}", token));
                string URL = string.Format("{0}?taskgroupid={1}", "http://dataapi.bazhuayu.com/api/task", taskGroupId);
                string taskStr = HttpHelper.Get(URL, headers);
                if (taskStr.Contains("success"))
                {
                    JArray jsonTasksJarray = JObject.Parse(taskStr)["data"] as JArray;
                    foreach (JToken jtokenTask in jsonTasksJarray)
                    {
                        tasks.Add(new Task()
                        {
                            TaskId = jtokenTask["taskId"].ToString(),
                            TaskName = jtokenTask["taskName"].ToString()
                        });
                    }
                }
            }
            return tasks;
        }

        /// <summary>
        /// 根据TaskId获取该任务采集的数据
        /// </summary>
        /// <param name="token">用户Token</param>
        /// <param name="taskId">任务ID</param>
        /// <param name="pageSize">数据条目个数</param>
        /// <returns>数据</returns>
        public DataResultModel GetDataByTask(string token, string taskId, int pageSize)
        {
            DataResultModel taskData = null;
            if (null != token)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", string.Format("bearer {0}", token));
                int pageIndex = 1;
                string URL = string.Format("{0}?taskid={1}&pageindex={2}&pagesize={3}", "http://dataapi.bazhuayu.com/api/alldata", taskId, pageIndex++, pageSize);
                var jsonData = HttpHelper.Get(URL, headers);
                if (jsonData.Contains("success"))
                {
                    taskData = JsonConvert.DeserializeObject<DataResultModel>(JObject.Parse(jsonData)["Data"].ToString());
                    // for example, save the data
                    SaveData(taskData.DataList);
                    int maxIndex = taskData.Total / pageSize + 1;
                    for (; pageIndex < maxIndex; pageIndex++)
                    {
                        System.Threading.Thread.Sleep(5000); // sleep 5s just in case of exceeded quota
                        URL = string.Format("{0}?taskid={1}&pageindex={2}&pagesize={3}", "http://dataapi.bazhuayu.com/api/alldata", taskId, pageIndex, pageSize);
                        jsonData = HttpHelper.Get(URL, headers);
                        if (jsonData.Contains("success"))
                        {
                            taskData = JsonConvert.DeserializeObject<DataResultModel>(JObject.Parse(jsonData)["Data"].ToString());
                            // for example, save the data
                            SaveData(taskData.DataList);
                        }
                    }
                }
            }
            return taskData;
        }

        public void SaveData(DataTable dt)
        {
            // save dt
        }
    }


    public class Task
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
    };
    public class TaskGroup
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public List<Task> Tasks { get; set; }
    };

    public class DataResultModel
    {
        /// <summary>
        /// 任务总数据条数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 此批数据的条数
        /// </summary>
        public int CurrentTotal { get; set; }
        /// <summary>
        /// 此批数据
        /// </summary>
        public DataTable DataList { get; set; }
    }
}

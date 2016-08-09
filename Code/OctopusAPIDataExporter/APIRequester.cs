using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace OctopusAPIDataExporter
{
    public class APIRequester
    {
        public APIUser user = null;
        public string taskGroupUrl = null;
        public string taskUrl = null;
        public string allDataUrl = null;
        public string notExportedDataUrl = null;
        public ControlChanger DelegGroupProgressTextChange;//控制任务组进度信息
        public ControlChanger DelegTaskProgressTextChange;//控制任务进度信息
        public ControlChanger DelegAddGroupIntoListView;//任务组列表控制
        public ControlChanger DelegAddProgressInfoIntoListView;//任务组获取进度信息列表控制
        public ProgressChanger DelegProgressChange;//控制获取数据进度条
        public ProgressChanger DelegGroupProgressChange;//控制任务组进度条

        //为用户和各种Url赋值
        public void AssignUserAndUrls(APIUser _user)
        {
            user = _user;
            AssignUrlsFromTokenUrl(_user.tokenUrl);
        }
        //根据TokenUrl对其他Url赋值
        public void AssignUrlsFromTokenUrl(string tokenurl = null)
        {
            if (null == tokenurl)
                tokenurl = user.tokenUrl;
            string rootUrl = tokenurl.Substring(0, tokenurl.LastIndexOf('/'));
            taskGroupUrl = rootUrl + "/api/taskgroup";
            taskUrl = rootUrl + "/api/task";
            allDataUrl = rootUrl + "/api/alldata";
            notExportedDataUrl = rootUrl + "/api/notexportdata";
        }

        #region 示例代码
        /// <summary>
        /// 通过taskgroup接口来获得某个用户的所有taskgroup
        /// </summary>
        /// <param name="token">某个用户的access token</param>
        /// <param name="taskGroupUrl">taskgroup接口地址，一般为http://ipadress:9000/api/taskgroup</param>
        public void GetTaskGroups(string token, string taskGroupUrl)
        {
            if (null != token && null != taskGroupUrl)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>(1);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                string TaskGroupsStr = HttpHelper.GetWithHeaders(taskGroupUrl, headers);
                if (!TaskGroupsStr.Contains("\"data\":"))
                {
                    //DelegGroupProgressTextChange(string.Format("获取任务组失败！"));
                    return;
                }
                JObject jsonTaskGroupsAll = JObject.Parse(TaskGroupsStr);
                JArray jsonTaskGroupsJarray = jsonTaskGroupsAll["data"] as JArray;
                user.taskGroups = new List<TaskGroup>(jsonTaskGroupsJarray.Count);
                foreach (JToken jtokenTaskGroup in jsonTaskGroupsJarray)
                {
                    user.taskGroups.Add(new TaskGroup()
                    {
                        taskGroupID = jtokenTaskGroup["taskGroupId"].ToString(),
                        taskGroupName = jtokenTaskGroup["taskGroupName"].ToString(),
                        //tasks = GetTasks(token,jtokenTaskGroup["taskGroupId"].ToString(),taskUrl)
                    });
                }
            }
        }

        /// <summary>
        /// 通过task接口来获得某个任务组的所有tasks，请求格式http://ipadress:9000/api/task?taskgroupid=123
        /// </summary>
        /// <param name="token">access token</param>
        /// <param name="taskGroupID">任务组标识</param>
        /// <param name="taskUrl">task接口地址，一般为http://ipadress:9000/api/task</param>
        /// <returns>传入任务组ID的任务组下的所有任务</returns>
        public List<Task> GetTasks(string token, string taskGroupID, string taskUrl)
        {
            List<Task> tasks = null;
            if (null != user && null != taskUrl)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>(1);
                headers.Add("Authorization", string.Format("bearer {0}", token));
                string URL = string.Format("{0}?taskgroupid={1}", taskUrl, taskGroupID);
                string taskStr = HttpHelper.GetWithHeaders(URL, headers);
                if (taskStr.Contains("\"data\":"))
                {
                    JObject jsonTasks = JObject.Parse(taskStr);
                    JArray jsonTasksJarray = jsonTasks["data"] as JArray;
                    tasks = new List<Task>(jsonTasksJarray.Count);
                    foreach (JToken jtokenTask in jsonTasksJarray)
                    {
                        tasks.Add(new Task()
                        {
                            taskID = jtokenTask["taskId"].ToString(),
                            taskName = jtokenTask["taskName"].ToString()
                        });
                    }
                }
                else
                {
                    //elegAddProgressInfoIntoListView(string.Format("获取Tasks失败！返回文本：{1}", taskStr));
                }
            }
            return tasks;
        }

        /// <summary>
        /// 根据TaskID获取该任务采集的数据
        /// </summary>
        /// <param name="token">用户Token</param>
        /// <param name="dataUrl">数据接口地址</param>
        /// <param name="taskID">任务ID</param>
        /// <param name="pageIndex">初始数据条目位置</param>
        /// <param name="pageSize">数据条目个数</param>
        /// <returns>任务采集的数据</returns>
        public string GetDataByTask(string token, string dataUrl, string taskID, int pageIndex, int pageSize)
        {
            string taskData = "";
            if (null != token && null != dataUrl)
            {
                string URL = "";
                Dictionary<string, string> headers = new Dictionary<string, string>(1);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                URL = string.Format("{0}?taskid={1}&pageindex={2}&pagesize={3}", dataUrl, taskID, pageIndex, pageSize);
                taskData = HttpHelper.GetWithHeaders(URL, headers);
            }
            return taskData;
        }

        public void GetDataByGroupAndSave(string token, int groupIndex,string savePath)
        {
            StreamWriter taskdataFileWriter;
            new DirectoryInfo(string.Format("{1}/GroupID_{0}/", user.taskGroups[groupIndex].taskGroupID, savePath)).Create();
            string taskData = "", dataFilePath = "";
            int count = 0;
            if (null != user.taskGroups[groupIndex].tasks)
            {
                foreach (Task task in user.taskGroups[groupIndex].tasks)
                {
                    count++;
                    taskData = GetDataByTask(token,0,task.taskID,1,10);
                    if (taskData.Contains("\"data\":"))
                    {
                        dataFilePath = string.Format("{4}/GroupID_{0}/{1}_TaskID {2}{3}", user.taskGroups[groupIndex].taskGroupID, task.taskName, task.taskID, ".json", savePath);
                    }
                    else
                    {
                        dataFilePath = string.Format("{4}/GroupID_{0}/_失败！_{1}_TaskID {2}{3}", user.taskGroups[groupIndex].taskGroupID, task.taskName, task.taskID, ".json", savePath);
                    }
                    taskdataFileWriter = File.CreateText(dataFilePath);
                    taskdataFileWriter.Write(taskData);
                    taskdataFileWriter.Close();
                }
            }
        }

        public string GetDataByTask(string token, int dataType, string taskID, int pageIndex, int pageSize)
        {
            string taskData = "";
            if (null != user)
            {
                string URL = "";
                switch (dataType)
                {
                    case 0: URL = allDataUrl; break;
                    case 1: URL = notExportedDataUrl; break;
                    default: break;
                }
                Dictionary<string, string> headers = new Dictionary<string, string>(2);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                URL = string.Format("{0}?taskid={1}&pageindex={2}&pagesize={3}", URL, taskID, pageIndex, pageSize);
                taskData = HttpHelper.GetWithHeaders(URL, headers);
            }
            return taskData;
        }
        #endregion

        //获取用户任务组
        public void GetTaskGroups()
        {
            if (null != user && null != taskGroupUrl)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>(1);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                string TaskGroupsStr = HttpHelper.GetWithHeaders(taskGroupUrl, headers);
                if (!TaskGroupsStr.Contains("\"data\":"))
                {
                    DelegGroupProgressTextChange(string.Format("获取任务组失败！"));
                    return;
                }
                JObject jsonTaskGroupsAll = JObject.Parse(TaskGroupsStr);
                JArray jsonTaskGroupsJarray = jsonTaskGroupsAll["data"] as JArray;
                int current = 0;

                DelegGroupProgressTextChange(string.Format("共查询到{0}个TaskGroup", jsonTaskGroupsJarray.Count));
                user.taskGroups = new List<TaskGroup>(jsonTaskGroupsJarray.Count);

                foreach (JToken jtokenTaskGroup in jsonTaskGroupsJarray)
                {
                    //异步线程并设定回调函数
                    //AsyncCallback callback = new AsyncCallback(this.AsyncCallbackImpl);
                    //tasksgetter.BeginInvoke(jtokenTaskGroup["taskGroupId"].ToString(), current, callback, tasksgetter);

                    user.taskGroups.Add(new TaskGroup()
                    {
                        taskGroupID = jtokenTaskGroup["taskGroupId"].ToString(),
                        taskGroupName = jtokenTaskGroup["taskGroupName"].ToString(),
                        tasks = GetTasks(jtokenTaskGroup["taskGroupId"].ToString(), current)//将任务组中的任务数据同时获取到，此操作需要时间
                    });

                    DelegAddGroupIntoListView(string.Format("{0}[ID:{1}]", user.taskGroups[current].taskGroupName, user.taskGroups[current].taskGroupID));
                    current++;
                    DelegGroupProgressChange(current, jsonTaskGroupsJarray.Count);
                }

            }

        }

        //根据任务组ID获取任务
        public List<Task> GetTasks(string taskGroupID, int currentGroup)
        {
            List<Task> tasks = null;
            if (null != user && null != taskUrl)
            {
                DelegAddProgressInfoIntoListView(string.Format("获取第{0}个TaskGroup中的Task...", currentGroup + 1));
                Dictionary<string, string> headers = new Dictionary<string, string>(2);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                //headers.Add("Accept", "application/json");
                string URL = string.Format("{0}?taskgroupid={1}", taskUrl, taskGroupID);
                string taskStr = HttpHelper.GetWithHeaders(URL, headers);
                if (taskStr.Contains("\"data\":"))
                {
                    JObject jsonTasks = JObject.Parse(taskStr);
                    JArray jsonTasksJarray = jsonTasks["data"] as JArray;
                    tasks = new List<Task>(jsonTasksJarray.Count);

                    DelegAddProgressInfoIntoListView(string.Format("成功，共{1}个Task！", currentGroup + 1, jsonTasksJarray.Count));
                    int current = 0;
                    foreach (JToken jtokenTask in jsonTasksJarray)
                    {
                        tasks.Add(new Task()
                        {
                            taskID = jtokenTask["taskId"].ToString(),
                            taskName = jtokenTask["taskName"].ToString()
                        });
                        current++;
                    }
                }
                else
                {
                    DelegAddProgressInfoIntoListView(string.Format("失败！返回文本：{1}", currentGroup + 1, taskStr));
                }
            }
            return tasks;
        }

        //获取任务数据时常用的数据结构汇总
        public class TaskDataConfig
        {
            public int dataType = 0;//0代表所有数据，1代表未导出数据
            public string taskID = "";
            public int groupIndex = 0;
            public int pageIndex = 1;
            public int pageSize = 1;
            public string savePath;
        }

        //获取当前用户的所有组的所有任务及其数据
        public void GetDataByGroupAndSave(TaskDataConfig config)
        {
            StreamWriter taskdataFileWriter;
            new DirectoryInfo(string.Format("{1}/GroupID_{0}/", user.taskGroups[config.groupIndex].taskGroupID, config.savePath)).Create();
            string taskData = "", dataFilePath = "";
            int count = 0;
            if (null != user.taskGroups[config.groupIndex].tasks)
            {
                foreach (Task task in user.taskGroups[config.groupIndex].tasks)
                {
                    DelegGroupProgressTextChange(string.Format("正在导出TaskGroup中的Task数据：{0}[ID:{1}]", user.taskGroups[config.groupIndex].taskGroupName, user.taskGroups[config.groupIndex].taskGroupID));
                    count++;
                    config.taskID = task.taskID;
                    taskData = GetDataByTask(config);
                    if (taskData.Contains("\"data\":"))
                    {
                        dataFilePath = string.Format("{4}/GroupID_{0}/{1}_TaskID {2}{3}", user.taskGroups[config.groupIndex].taskGroupID, task.taskName, task.taskID, ".json", config.savePath);
                    }
                    else
                    {
                        dataFilePath = string.Format("{4}/GroupID_{0}/_失败！_{1}_TaskID {2}{3}", user.taskGroups[config.groupIndex].taskGroupID, task.taskName, task.taskID, ".json", config.savePath);
                        DelegTaskProgressTextChange(string.Format("获取任务数据失败！"));
                    }
                    taskdataFileWriter = File.CreateText(dataFilePath);
                    taskdataFileWriter.Write(taskData);
                    DelegTaskProgressTextChange(string.Format("json文件保存：{0}", dataFilePath));
                    taskdataFileWriter.Close();
                    DelegProgressChange(count, user.taskGroups[config.groupIndex].tasks.Count);
                }
            }
            else
            {

            }
        }

        //获取一个任务的数据
        public string GetDataByTask(TaskDataConfig config)
        {
            string taskData = "";
            if (null != user && null != allDataUrl)
            {
                string URL = "";
                switch (config.dataType)
                {
                    case 0: URL = allDataUrl; break;
                    case 1: URL = notExportedDataUrl; break;
                    default: break;
                }
                Dictionary<string, string> headers = new Dictionary<string, string>(2);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                //headers.Add("Accept", "application/json");
                URL = string.Format("{0}?taskid={1}&pageindex={2}&pagesize={3}", URL, config.taskID, config.pageIndex, config.pageSize);
                DelegTaskProgressTextChange(string.Format("{0}", URL));
                taskData = HttpHelper.GetWithHeaders(URL, headers);
            }
            return taskData;
        }

    }
}

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
        public ControlChanger DelegGroupProgressTextChange;
        public ControlChanger DelegTaskProgressTextChange;
        public ControlChanger DelegAddGroupIntoListView;
        public ControlChanger DelegAddProgressInfoIntoListView;
        public ProgressChanger DelegProgressChange;
        public ProgressChanger DelegGroupProgressChange;

        public void AssignUserAndUrls(APIUser _user)
        {
            user = _user;
            AssignUrlsFromTokenUrl(_user.tokenUrl);
        }
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

        #region Example Code
        /// <summary>
        /// using taskgroup interface to all the taskgroups of a user
        /// </summary>
        /// <param name="token">access token of a user</param>
        /// <param name="taskGroupUrl">taskgroup interface address, like: http://ipadress:9000/api/taskgroup </param>
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
        /// using task interface all the tasks of a taskgroup
        /// </summary>
        /// <param name="token">access token</param>
        /// <param name="taskGroupID">taskGroupID</param>
        /// <param name="taskUrl">task interface address, like: http://ipadress:9000/api/task?taskgroupid=123 </param>
        /// <returns>all the task of the given task group</returns>
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
        /// using data export interface to get data of a task
        /// </summary>
        /// <param name="token">access token of a user</param>
        /// <param name="dataUrl">data export interface address, like: http://ipadress:9000/api/alldata?taskid=123&pageIndex=0&pageSize=100 </param>
        /// <param name="taskID">task ID</param>
        /// <param name="pageIndex">page index of the paged data by pageSize for all data</param>
        /// <param name="pageSize">the size of each paged data</param>
        /// <returns>a page of data</returns>
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
                        dataFilePath = string.Format("{4}/GroupID_{0}/_failed！_{1}_TaskID {2}{3}", user.taskGroups[groupIndex].taskGroupID, task.taskName, task.taskID, ".json", savePath);
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

        
        public void GetTaskGroups()
        {
            if (null != user && null != taskGroupUrl)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>(1);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                string TaskGroupsStr = HttpHelper.GetWithHeaders(taskGroupUrl, headers);
                if (!TaskGroupsStr.Contains("\"data\":"))
                {
                    DelegGroupProgressTextChange(string.Format("Failed to get task group(s)！"));
                    return;
                }
                JObject jsonTaskGroupsAll = JObject.Parse(TaskGroupsStr);
                JArray jsonTaskGroupsJarray = jsonTaskGroupsAll["data"] as JArray;
                int current = 0;

                DelegGroupProgressTextChange(string.Format("{0} task group(s) found", jsonTaskGroupsJarray.Count));
                user.taskGroups = new List<TaskGroup>(jsonTaskGroupsJarray.Count);

                foreach (JToken jtokenTaskGroup in jsonTaskGroupsJarray)
                {
                    user.taskGroups.Add(new TaskGroup()
                    {
                        taskGroupID = jtokenTaskGroup["taskGroupId"].ToString(),
                        taskGroupName = jtokenTaskGroup["taskGroupName"].ToString(),
                        tasks = GetTasks(jtokenTaskGroup["taskGroupId"].ToString(), current)//get task info and data, may be somehow slow.
                    });

                    DelegAddGroupIntoListView(string.Format("{0}[ID:{1}]", user.taskGroups[current].taskGroupName, user.taskGroups[current].taskGroupID));
                    current++;
                    DelegGroupProgressChange(current, jsonTaskGroupsJarray.Count);
                }

            }

        }

        public List<Task> GetTasks(string taskGroupID, int currentGroup)
        {
            List<Task> tasks = null;
            if (null != user && null != taskUrl)
            {
                DelegAddProgressInfoIntoListView(string.Format("Requiring task(s) for {0}th task group...", currentGroup + 1));
                Dictionary<string, string> headers = new Dictionary<string, string>(2);
                headers.Add("Authorization", string.Format("bearer {0}", user.token));
                string URL = string.Format("{0}?taskgroupid={1}", taskUrl, taskGroupID);
                string taskStr = HttpHelper.GetWithHeaders(URL, headers);
                if (taskStr.Contains("\"data\":"))
                {
                    JObject jsonTasks = JObject.Parse(taskStr);
                    JArray jsonTasksJarray = jsonTasks["data"] as JArray;
                    tasks = new List<Task>(jsonTasksJarray.Count);

                    DelegAddProgressInfoIntoListView(string.Format("Success to get {1} task(s)！", currentGroup + 1, jsonTasksJarray.Count));
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
                    DelegAddProgressInfoIntoListView(string.Format("Failed!Responsed Message：{1}", currentGroup + 1, taskStr));
                }
            }
            return tasks;
        }

        public class TaskDataConfig
        {
            public int dataType = 0;//0 stand for all data，1 for unexported data
            public string taskID = "";
            public int groupIndex = 0;
            public int pageIndex = 1;
            public int pageSize = 1;
            public string savePath;
        }

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
                    DelegGroupProgressTextChange(string.Format("Exporting data for task(s) in group: {0}[ID:{1}]", user.taskGroups[config.groupIndex].taskGroupName, user.taskGroups[config.groupIndex].taskGroupID));
                    count++;
                    config.taskID = task.taskID;
                    taskData = GetDataByTask(config);
                    if (taskData.Contains("\"data\":"))
                    {
                        dataFilePath = string.Format("{4}/GroupID_{0}/{1}_TaskID {2}{3}", user.taskGroups[config.groupIndex].taskGroupID, task.taskName, task.taskID, ".json", config.savePath);
                    }
                    else
                    {
                        dataFilePath = string.Format("{4}/GroupID_{0}/_Failed！_{1}_TaskID {2}{3}", user.taskGroups[config.groupIndex].taskGroupID, task.taskName, task.taskID, ".json", config.savePath);
                        DelegTaskProgressTextChange(string.Format("Failed to get data for task!"));
                    }
                    taskdataFileWriter = File.CreateText(dataFilePath);
                    taskdataFileWriter.Write(taskData);
                    DelegTaskProgressTextChange(string.Format("Saving JSON file: {0}", dataFilePath));
                    taskdataFileWriter.Close();
                    DelegProgressChange(count, user.taskGroups[config.groupIndex].tasks.Count);
                }
            }
            else
            {

            }
        }

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
                URL = string.Format("{0}?taskid={1}&pageindex={2}&pagesize={3}", URL, config.taskID, config.pageIndex, config.pageSize);
                DelegTaskProgressTextChange(string.Format("{0}", URL));
                taskData = HttpHelper.GetWithHeaders(URL, headers);
            }
            return taskData;
        }

    }
}

using Newtonsoft.Json;
using Octopus.ApiSamples.Models;
using Octopus.ApiSamples.Utils;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace Octopus.ApiSamples
{
    public class OctopusSample
    {
        //增值Api用户请使用地址: http://advancedapi.bazhuayu.com
        private static readonly string _baseUrl = "http://dataapi.bazhuayu.com";
        private readonly RestClient _client = new RestClient(_baseUrl);

        /// <summary>
        /// 获取全新的Token
        /// </summary>
        /// <param name="username">八爪鱼账号</param>
        /// <param name="password">密码</param>
        /// <returns>Token对象</returns>
        public Token GetToken(string username, string password)
        {
            Requires.NotNullOrEmpty(username, nameof(username));
            Requires.NotNullOrEmpty(password, nameof(password));

            var request = new RestRequest("token", Method.POST)
                .AddParameter("username", username)
                .AddParameter("password", password)
                .AddParameter("grant_type", "password");

            var response = _client.Execute(request);
            ThrowExceptionIfResponseError(response);

            return JsonConvert.DeserializeObject<Token>(response.Content);
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="refreshToken">用于刷新的token值</param>
        /// <returns>Token对象</returns>
        public Token RefreshToken(string refreshToken)
        {
            Requires.NotNullOrEmpty(refreshToken, nameof(refreshToken));

            var request = new RestRequest("token", Method.POST)
                .AddParameter("refresh_token", refreshToken)
                .AddParameter("grant_type", "refresh_token");

            var response = _client.Execute(request);
            ThrowExceptionIfResponseError(response);

            return JsonConvert.DeserializeObject<Token>(response.Content);
        }

        /// <summary>
        /// 获取任务组中的任务
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="groupId">任务组Id</param>
        /// <returns>任务列表</returns>
        public List<Task> GetTasksByGroup(string accessToken, string groupId)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));

            var request = new RestRequest("api/task", Method.GET)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskGroupId", groupId);

            var response = _client.Execute(request);
            var result = GetResultFromResponse<List<Task>>(response);
            ThrowExceptionIfResultError(result);

            return result.Data;
        }

        /// <summary>
        /// 获取所有任务组
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <returns>任务组列表</returns>
        public List<TaskGroup> GetTaskGroups(string accessToken)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));

            var request = new RestRequest("api/taskGroup", Method.GET)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken));

            var response = _client.Execute(request);
            var result = GetResultFromResponse<List<TaskGroup>>(response);
            ThrowExceptionIfResultError(result);

            return result.Data;
        }

        /// <summary>
        /// 根据起始偏移量获取任务数据
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        /// <param name="offset">数据偏移量，当offset小于等于0时，则从起始位置读取任务数据</param>
        /// <param name="size">要获取的数据量(范围:[1,1000])</param>
        /// <returns>TaskDataOffsetResult对象</returns>
        public TaskDataOffsetResult GetTaskDataByOffset(string accessToken, string taskId, long offset, int size)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));
            Requires.MustGreaterThan(offset, 0, nameof(offset));
            Requires.MustGreaterThan(size, 0, nameof(size));

            var request = new RestRequest("api/allData/GetDataOfTaskByOffset", Method.GET)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskId", taskId)
                .AddQueryParameter("offset", offset.ToString())
                .AddQueryParameter("size", size.ToString());

            var response = _client.Execute(request);
            var result = GetResultFromResponse<TaskDataOffsetResult>(response);
            ThrowExceptionIfResultError(result);

            return result.Data;
        }

        /// <summary>
        /// 清空任务数据
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        public void RemoveTaskData(string accessToken, string taskId)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));

            var request = new RestRequest("api/task/removeDataByTaskId", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskId", taskId);

            var response = _client.Execute(request);
            var result = GetResultFromResponse(response);

            ThrowExceptionIfResultError(result);
        }

        /// <summary>
        /// 导出任务的未导出数据
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        /// <param name="size">数据条数(范围:[1,1000])</param>
        /// <returns>TaskDataExportResult对象</returns>
        public TaskDataExportResult ExportNotExportedData(string accessToken, string taskId, int size)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));
            Requires.MustGreaterThan(size, 0, nameof(size));

            var request = new RestRequest("api/notExportData/getTop", Method.GET)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskId", taskId)
                .AddQueryParameter("size", size.ToString());

            var response = _client.Execute(request);
            var result = GetResultFromResponse<TaskDataExportResult>(response);
            ThrowExceptionIfResultError(result);

            return result.Data;
        }

        /// <summary>
        /// 标记数据为已导出状态
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        public void MarkDataExported(string accessToken, string taskId)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));

            var request = new RestRequest("api/notExportData/update", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskId", taskId);

            var response = _client.Execute(request);
            var result = GetResultFromResponse(response);

            ThrowExceptionIfResultError(result);
        }

        #region Advance apis
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        /// <returns>启动任务状态结果</returns>
        public StartTaskResult StartTask(string accessToken, string taskId)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));

            var request = new RestRequest("api/task/startTask", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskId", taskId);

            var response = _client.Execute(request);
            var result = GetResultFromResponse<StartTaskResult>(response);
            ThrowExceptionIfResultError(result);

            return result.Data;
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        public void StopTask(string accessToken, string taskId)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));

            var request = new RestRequest("api/task/stopTask", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskId", taskId);

            var response = _client.Execute(request);
            var result = GetResultFromResponse(response);

            ThrowExceptionIfResultError(result);
        }

        /// <summary>
        /// 批量获取任务状态
        /// </summary>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskIds">任务Id列表</param>
        /// <returns>TaskStatusModel列表</returns>
        public List<TaskStatusModel> GetTaskStatusList(string accessToken, IEnumerable<string> taskIds)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNull(taskIds, nameof(taskIds));

            var request = new RestRequest("api/task/getTaskStatusByIdList", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddJsonBody(new { taskIdList = taskIds });

            var response = _client.Execute(request);
            var result = GetResultFromResponse<List<TaskStatusModel>>(response);
            ThrowExceptionIfResultError(result);

            return result.Data;
        }

        /// <summary>
        /// 获取任务规则流程步骤的属性值
        /// </summary>
        /// <typeparam name="T">任务规则流程步骤属性值的类型</typeparam>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        /// <param name="name">属性名</param>
        /// <returns>任务规则流程步骤的属性值</returns>
        public T GetTaskRulePropertyByName<T>(string accessToken, string taskId, string name)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));
            Requires.NotNullOrEmpty(name, nameof(name));

            var request = new RestRequest("api/task/GetTaskRulePropertyByName", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddQueryParameter("taskId", taskId)
                .AddQueryParameter("name", name);

            var response = _client.Execute(request);
            var result = GetResultFromResponse<T>(response);
            ThrowExceptionIfResultError(result);

            return result.Data;
        }

        /// <summary>
        /// 更新任务规则流程属性值
        /// </summary>
        /// <typeparam name="T">任务规则流程属性值的类型</typeparam>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public void UpdateTaskRuleProperty<T>(string accessToken, string taskId, string name, T value)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));
            Requires.NotNullOrEmpty(name, nameof(name));
            Requires.NotNull(value, nameof(value));

            var request = new RestRequest("api/task/updateTaskRule", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddJsonBody(new
                {
                    taskId = taskId,
                    name = name,
                    value = value
                });

            var response = _client.Execute(request);
            var result = GetResultFromResponse(response);

            ThrowExceptionIfResultError(result);
        }

        /// <summary>
        /// 任务规则循环列表中添加新项
        /// </summary>
        /// <typeparam name="T">新项类型(string或IEnumerable<string>)</typeparam>
        /// <param name="accessToken">授权Token</param>
        /// <param name="taskId">任务Id</param>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public void AddUrlOrTextToTask<T>(string accessToken, string taskId, string name, T value)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));
            Requires.NotNullOrEmpty(name, nameof(name));
            Requires.NotNull(value, nameof(value));

            var request = new RestRequest("api/task/addUrlOrTextToTask", Method.POST)
                .AddHeader("Authorization", string.Format("bearer {0}", accessToken))
                .AddJsonBody(new
                {
                    taskId = taskId,
                    name = name,
                    value = value
                });

            var response = _client.Execute(request);
            var result = GetResultFromResponse(response);

            ThrowExceptionIfResultError(result);
        }
        #endregion

        #region Private methods
        private CallResult GetResultFromResponse(IRestResponse response)
        {
            ThrowExceptionIfResponseError(response);

            var result = JsonConvert.DeserializeObject<CallResult>(response.Content);
            return result;
        }

        private CallResult<T> GetResultFromResponse<T>(IRestResponse response)
        {
            ThrowExceptionIfResponseError(response);

            var result = JsonConvert.DeserializeObject<CallResult<T>>(response.Content);
            return result;
        }

        private static void ThrowExceptionIfResponseError(IRestResponse response)
        {
            if (response.ErrorException != null)
            {
                //比如一些网络异常
                throw response.ErrorException;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiCallException(HttpStatusErrors.GetError(response.StatusCode));
            }
        }

        private void ThrowExceptionIfResultError(CallResult result)
        {
            if (!result.IsSucceed)
            {
                throw new ApiCallException(result.Error, result.ErrorDescription);
            }
        }
        #endregion
    }
}

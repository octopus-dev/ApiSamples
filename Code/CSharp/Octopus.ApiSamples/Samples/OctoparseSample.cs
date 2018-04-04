using Newtonsoft.Json;
using Octopus.ApiSamples.Models;
using Octopus.ApiSamples.Utils;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace Octopus.ApiSamples
{
    public class OctoparseSample
    {

        //For advanced api: http://advancedapi.octoparse.com
        private readonly string _baseUrl = "http://dataapi.octoparse.com";
        private readonly RestClient _client;
        public OctoparseSample(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new RestClient(_baseUrl);
        }

        /// <summary>
        /// Obtain a new token
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>Token object</returns>
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
        /// Refresh Token
        /// </summary>
        /// <param name="refreshToken">Token to refresh Access Token</param>
        /// <returns>Token object</returns>
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
        /// List all tasks in a group
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="groupId">Task group ID</param>
        /// <returns>Task list</returns>
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
        /// List all task groups
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <returns>task group list</returns>
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
        /// Get data by offset
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task ID</param>
        /// <param name="offset">Offset, if offset is less than or equal to 0, data will be returned starting from the first row</param>
        /// <param name="size">The amount of data that will be returned (range from 1 to 1000) </param>
        /// <returns>TaskDataOffsetResult object</returns>
        public TaskDataOffsetResult GetTaskDataByOffset(string accessToken, string taskId, long offset, int size)
        {
            Requires.NotNullOrEmpty(accessToken, nameof(accessToken));
            Requires.NotNullOrEmpty(taskId, nameof(taskId));
            Requires.MustGreaterThan(size, 0, nameof(size));

            var request = new RestRequest("api/allData/getDataOfTaskByOffset", Method.GET)
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
        /// Clear task data
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
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
        /// Export non-exported data
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
        /// <param name="size">The amount of data (range from 1 to 1000) </param>
        /// <returns>TaskDataExportResult object</returns>
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
        /// Update data status
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
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
        /// Start running task
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
        /// <returns>Start task status result</returns>
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
        /// Stop running task
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
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
        /// Get task status
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskIds">Task Id list</param>
        /// <returns>TaskStatusModel list</returns>
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
        /// Get task rule property
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
        /// <param name="name">Property name</param>
        /// <returns>Property value</returns>
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
        /// Update task rule property
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
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
        /// Add URL/text to a loop in task rule
        /// </summary>
        /// <typeparam name="T">Property type (string or IEnumerable<string>)</typeparam>
        /// <param name="accessToken">Access Token</param>
        /// <param name="taskId">Task Id</param>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
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
                //such as some network error..
                throw response.ErrorException;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiCallException(HttpStatusErrors.GetError(response.StatusCode), response.Content);
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

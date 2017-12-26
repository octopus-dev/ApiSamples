using Newtonsoft.Json;
using OctopusApiSamples.Models;
using OctopusApiSamples.Utils;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace OctopusApiSamples
{
    public class OctoparseSample
    {
        private static readonly string _baseUrl = "";
        private readonly RestClient _client = new RestClient(_baseUrl);

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

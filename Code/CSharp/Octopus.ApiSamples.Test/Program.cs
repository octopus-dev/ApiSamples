using Octopus.ApiSamples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Octopus.ApiSamples.Test
{
    class Program
    {
        public static Token Token;
        public static string TaskId = "b14da7c6-d7e7-4f07-b0bc-175f59e86bc9";
        public static string TaskIdOperation = "fdd0875e-545b-4c62-9bc1-e14e8bbb0c73";
        
        static void Main(string[] args)
        {
            try
            {
                string address = "http://{0}.bazhuayu.com";
                bool isAdvanced = true;
#if INTER
                address = "http://{0}.octoparse.com";
                TaskId = "dacd06a1-216d-4553-b73f-3b53416cc7b5";
                TaskIdOperation = "82e80964-e104-48fa-b3cc-ed49fe68ed44";
#endif
                string apiName = "dataapi";
                if (isAdvanced)
                {
                    apiName = "advancedapi";
                }
                address = string.Format(address, apiName);
                if (args.Length > 0)
                {
                    address = args[0];
                }
                if (args.Length > 1)
                {
                    isAdvanced = bool.Parse(args[1]);
                }
                OctoparseSample octoparseSample = new OctoparseSample(address);
                Token = octoparseSample.GetToken("vaecole", "qwer1010");
                Debug.Assert(Token?.AccessToken != null);
                Console.WriteLine(Token.AccessToken + Token.ExpiresIn);

                try
                {
                    Token = octoparseSample.RefreshToken(Token.RefreshToken);
                    Debug.Assert(Token?.AccessToken != null);
                    Console.WriteLine("Token refreshed!");
                    Console.WriteLine(Token.AccessToken + Token.ExpiresIn);
                }
                catch (ApiCallException ex)
                {
                    Console.WriteLine(ex.Error + ex.ErrorDescription);
                }

                Console.WriteLine("GetTaskDataByOffset:");
                var wrappedData = octoparseSample.GetTaskDataByOffset(Token.AccessToken, TaskId, 0, 10);
                Debug.Assert(wrappedData != null);
                Console.WriteLine(wrappedData.DataList.Rows.Count + "/" + wrappedData.Total);
                Console.WriteLine(wrappedData.DataList.Rows[0][0].ToString());
                Console.WriteLine("OK.");

                Console.WriteLine("RemoveTaskData:");
                octoparseSample.RemoveTaskData(Token.AccessToken, TaskIdOperation);
                Console.WriteLine("OK.");

                Console.WriteLine("GetTaskGroups:");
                var groups = octoparseSample.GetTaskGroups(Token.AccessToken);
                Debug.Assert(groups != null);
                Console.WriteLine(groups.Count);
                Console.WriteLine(groups.First().Name);
                Console.WriteLine("OK.");

                Console.WriteLine("GetTasks:");
                var tasks = octoparseSample.GetTasksByGroup(Token.AccessToken, groups.FirstOrDefault().Id);
                Debug.Assert(tasks != null);
                Console.WriteLine(tasks.Count);
                Console.WriteLine(tasks.First().Name);
                Console.WriteLine("OK.");

                Console.WriteLine("ExportNotExportedData:");
                var exportData1 = octoparseSample.ExportNotExportedData(Token.AccessToken, TaskId, 10);
                Debug.Assert(exportData1 != null);
                if (exportData1.DataList != null)
                {
                    Console.WriteLine(exportData1.DataList.Rows.Count + "/" + exportData1.Total);
                    Console.WriteLine(exportData1.DataList.Rows[0][0].ToString());
                }
                Console.WriteLine("OK.");

                Console.WriteLine("MarkDataExported:");
                octoparseSample.MarkDataExported(Token.AccessToken, TaskId);
                Console.WriteLine("OK.");

                #region Advanced
                if (isAdvanced)
                {
                    Console.WriteLine("StartTask:");
                    var startRes = octoparseSample.StartTask(Token.AccessToken, TaskIdOperation);
                    Console.WriteLine(startRes);

                    System.Threading.Thread.Sleep(2000);

                    Console.WriteLine("Task Status:");
                    var statuses = octoparseSample.GetTaskStatusList(Token.AccessToken, new string[] { TaskIdOperation });
                    Console.WriteLine(statuses.FirstOrDefault().Status);

                    Console.WriteLine("StopTask:");
                    octoparseSample.StopTask(Token.AccessToken, TaskIdOperation);
                    Console.WriteLine("OK.");
                    statuses = octoparseSample.GetTaskStatusList(Token.AccessToken, new string[] { TaskIdOperation });
                    Console.WriteLine(statuses.FirstOrDefault().Status);

                    Console.WriteLine("UpdateTaskRuleProperty: navigateAction1.Url");
                    octoparseSample.UpdateTaskRuleProperty<string>(Token.AccessToken, TaskIdOperation, "navigateAction1.Url", "http://acm.sjtu.edu.cn/OnlineJudge/problems?page=" + new Random().Next(1, 100));
                    Console.WriteLine("OK.");

                    Console.WriteLine("GetTaskRulePropertyByName: navigateAction1.Url");
                    var properties = octoparseSample.GetTaskRulePropertyByName<string[]>(Token.AccessToken, TaskIdOperation, "navigateAction1.Url");
                    Console.WriteLine(properties.FirstOrDefault());

                    Console.WriteLine("UpdateTaskRuleProperty: loopAction2.TextList");
                    octoparseSample.UpdateTaskRuleProperty<string[]>(Token.AccessToken, TaskIdOperation, "loopAction2.TextList", new string[] { "testText1", "testText2" });
                    Console.WriteLine("OK.");

                    Console.WriteLine("GetTaskRulePropertyByName: loopAction2.TextList");
                    properties = octoparseSample.GetTaskRulePropertyByName<string[]>(Token.AccessToken, TaskIdOperation, "loopAction2.TextList");
                    Console.WriteLine(properties.FirstOrDefault());
                }
            }
            catch (ApiCallException ex)
            {
                Console.WriteLine(ex.Error + ex.ErrorDescription);
            }
            Console.WriteLine("Test finished!");
            Console.ReadKey();
            #endregion
        }
    }
}

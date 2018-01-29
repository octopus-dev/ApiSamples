using Octopus.ApiSamples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Octopus.ApiSamples.Test
{
    class Program
    {
        public static Token Token;
        public static string TaskId = "7ea57485-9e6e-4c33-9774-a606ddf3fc54";
        public static string TaskIdOperation = "82e80964-e104-48fa-b3cc-ed49fe68ed44";
        static void Main(string[] args)
        {
            OctoparseSample octoparseSample = new OctoparseSample("http://advancedapi.octoparse.com");
            Token = octoparseSample.GetToken("vaecole", "qwer1010");
            Debug.Assert(Token?.AccessToken != null);
            Console.WriteLine(Token.AccessToken + Token.ExpiresIn);

            Token = octoparseSample.RefreshToken(Token.RefreshToken);
            Debug.Assert(Token?.AccessToken != null);
            Console.WriteLine("Token refreshed!");
            Console.WriteLine(Token.AccessToken + Token.ExpiresIn);

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
            Console.WriteLine(exportData1.DataList.Rows.Count + "/" + exportData1.Total);
            Console.WriteLine(exportData1.DataList.Rows[0][0].ToString());
            Console.WriteLine("OK.");

            Console.WriteLine("ExportNotExportedData:");
            octoparseSample.MarkDataExported(Token.AccessToken, TaskId);
            Console.WriteLine("OK.");

            #region Advanced
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
            #endregion
        }
    }
}

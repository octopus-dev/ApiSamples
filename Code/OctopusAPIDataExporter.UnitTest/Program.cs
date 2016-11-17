using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctopusAPIDataExporter;

namespace OctopusAPIDataExporter.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            APIRequester _apirequester = new APIRequester();
            string userName, password, token = "";
            while (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("User name:");
                userName = Console.ReadLine();
                Console.WriteLine("Password:");
                password = Console.ReadLine();
                _apirequester.AssignUserAndUrls(new APIUser(userName, password, "http://dataapi.bazhuayu.com/token"));
                Console.WriteLine("Getting Token:");
                token = _apirequester.user.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Wrong user name or password!");
                }
                else
                {
                    Console.WriteLine(token);
                }
            }
            Console.WriteLine("{0}", token);
            Console.WriteLine("Getting task group(s)");
            _apirequester.GetTaskGroups(token, _apirequester.taskGroupUrl);

            foreach (TaskGroup tg in _apirequester.user.taskGroups)
            {
                Console.WriteLine("->Task group: {0}[{1}]", tg.taskGroupName, tg.taskGroupID);
                Console.WriteLine("->Requiring task(s)");
                List<Task> tasks = _apirequester.GetTasks(token, tg.taskGroupID, _apirequester.taskUrl);
                if (null != tasks)
                    foreach (var item in tasks)
                    {
                        Console.WriteLine("-->Requiring data: {0}[{1}]", item.taskName, item.taskID);
                        Console.WriteLine("--->Required data: {0}", _apirequester.GetDataByTask(token, 0, item.taskID, 1, 10));
                        System.Threading.Thread.Sleep(1000);
                    }
            }
            Console.ReadKey();
        }
    }
}

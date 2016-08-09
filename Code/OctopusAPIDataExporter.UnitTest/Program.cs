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
            _apirequester.AssignUserAndUrls(new APIUser("test", "123456", "http://127.0.0.1:9000/token"));
            Console.WriteLine("获取Token:");
            string token = _apirequester.user.GetToken();
            Console.WriteLine("{0}", token);
            Console.WriteLine("获取任务组");
            _apirequester.GetTaskGroups(token, _apirequester.taskGroupUrl);

            foreach (TaskGroup tg in _apirequester.user.taskGroups)
            {
                Console.WriteLine("->任务组：{0}[{1}]", tg.taskGroupName, tg.taskGroupID);
                Console.WriteLine("->获取任务");
                List<Task> tasks = _apirequester.GetTasks(token, tg.taskGroupID, _apirequester.taskUrl);
                if (null != tasks)
                    foreach (var item in tasks)
                    {
                        Console.WriteLine("-->获取任务数据:{0}[{1}]", item.taskName, item.taskID);
                        Console.WriteLine("获得任务数据:{0}", _apirequester.GetDataByTask(token, 0, item.taskID, 1, 10));
                        System.Threading.Thread.Sleep(1000);//服务器IP访问频率控制，这里暂停1秒
                    }
            }

            Console.ReadKey();
        }
    }
}

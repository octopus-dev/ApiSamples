using System;

namespace Octopus.ApiSamples
{
    static class Program
    {
        static void Main(string[] args)
        {
            var sample = new OctopusSample();

            try
            {
                var token = sample.GetToken("your username", "your password");
                var groups = sample.GetTaskGroups(token.AccessToken);

                foreach (var group in groups)
                {
                    //just in case of quota exceeded..
                    System.Threading.Tasks.Task.Delay(1000).Wait();

                    var tasks = sample.GetTasksByGroup(token.AccessToken, group.Id);
                    foreach (var task in tasks)
                    {
                        Console.WriteLine("Task {0} in group {1}", task.Name, group.Name);
                    }
                }
            }
            catch (ApiCallException aex)
            {
                Console.WriteLine("Api error: {0}, detail: {1}", aex.Error, aex.ErrorDescription);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: {0}", ex.Message);
            }
        }
    }
}

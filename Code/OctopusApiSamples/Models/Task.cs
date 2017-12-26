using Newtonsoft.Json;

namespace OctopusApiSamples.Models
{
    public class Task
    {
        [JsonProperty("TaskId")]
        public string Id { get; set; }

        [JsonProperty("TaskName")]
        public string Name { get; set; }
    }
}

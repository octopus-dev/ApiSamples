using Newtonsoft.Json;

namespace OctopusApiSamples.Models
{
    public class TaskGroup
    {
        [JsonProperty("TaskGroupId")]
        public string Id { get; set; }

        [JsonProperty("TaskGroupName")]
        public string Name { get; set; }
    }
}

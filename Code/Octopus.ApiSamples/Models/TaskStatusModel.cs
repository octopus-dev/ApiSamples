namespace Octopus.ApiSamples.Models
{
    public enum TaskStatus
    {
        Executing = 0,
        Stopped = 1,
        Finished = 2,
        Waiting = 3,
        Unexecuted = 5
    }

    public class TaskStatusModel
    {
        public string TaskId { get; set; }

        public string TaskName { get; set; }

        public TaskStatus Status { get; set; }
    }
}

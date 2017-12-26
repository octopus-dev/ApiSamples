namespace Octopus.ApiSamples.Models
{
    public enum StartTaskResult
    {
        Succeed = 1,
        Executing = 2,
        TaskConfigError = 3,
        PermissionDenied = 4,
        Other = 100
    }
}

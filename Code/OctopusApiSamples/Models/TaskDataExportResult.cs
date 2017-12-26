using System.Data;

namespace OctopusApiSamples.Models
{
    public class TaskDataExportResult
    {
        public int Total { get; set; }

        public int CurrentTotal { get; set; }

        public DataTable DataList { get; set; }
    }
}

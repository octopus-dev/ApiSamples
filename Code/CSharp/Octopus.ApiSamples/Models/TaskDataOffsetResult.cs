using System.Data;

namespace Octopus.ApiSamples.Models
{
    public class TaskDataOffsetResult
    {
        public long Offset { get; set; }

        public int Total { get; set; }

        public int RestTotal { get; set; }

        public DataTable DataList { get; set; }
    }
}

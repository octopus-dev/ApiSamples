using Newtonsoft.Json;
using System;

namespace Octopus.ApiSamples.Models
{
    public class CallResult
    {
        public string Error { get; set; }

        [JsonProperty("Error_Description")]
        public string ErrorDescription { get; set; }

        public bool IsSucceed
        {
            get
            {
                return string.Equals(Error, "success", StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    public class CallResult<T> : CallResult
    {
        public T Data { get; set; }
    }  
}

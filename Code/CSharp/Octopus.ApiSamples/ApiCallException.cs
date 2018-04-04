using System;

namespace Octopus.ApiSamples
{
    public class ApiCallException : Exception
    {
        public string Error { get; private set; }

        public string ErrorDescription { get; private set; }

        public ApiCallException(string error)
        {
            Error = error;
        }

        public ApiCallException(string error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}

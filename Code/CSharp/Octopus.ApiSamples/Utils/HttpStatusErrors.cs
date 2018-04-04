using System.Collections.Generic;
using System.Net;

namespace Octopus.ApiSamples.Utils
{
    public static class HttpStatusErrors
    {
        private static IDictionary<HttpStatusCode, string> _errors = new Dictionary<HttpStatusCode, string>()
        {
            { HttpStatusCode.BadRequest, "client_error" },
            { HttpStatusCode.Unauthorized, "unauthorized" },
            { HttpStatusCode.MethodNotAllowed, "method_not_allow" },
            { HttpStatusCode.NotFound, "not_found" },
            { (HttpStatusCode)429, "quota_exceeded" },
            { HttpStatusCode.InternalServerError, "server_error" },
            { HttpStatusCode.ServiceUnavailable, "service_unavailable" }
        };

        public static string GetError(HttpStatusCode code)
        {
            if (_errors.ContainsKey(code))
            {
                return _errors[code];
            }

            return "unknown_error";
        }
    }
}

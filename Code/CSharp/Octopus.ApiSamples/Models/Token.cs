using Newtonsoft.Json;

namespace Octopus.ApiSamples.Models
{
    public class Token
    {
        [JsonProperty("Access_Token")]
        public string AccessToken { get; set; }

        [JsonProperty("Token_Type")]
        public string TokenType { get; set; }

        [JsonProperty("Refresh_Token")]
        public string RefreshToken { get; set; }

        //unit: second
        [JsonProperty("Expires_In")]
        public int ExpiresIn { get; set; }
    }
}

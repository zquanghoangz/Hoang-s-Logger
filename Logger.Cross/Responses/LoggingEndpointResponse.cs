using Newtonsoft.Json;

namespace Logger.Cross.Responses
{
    public class LoggingEndpointResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }
}
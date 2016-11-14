using System;
using Newtonsoft.Json;

namespace Logger.Cross.Responses
{
    public class AuthorizationResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
    }
}
using Newtonsoft.Json;

namespace SkillfullAPI.Models.LightcastApiModels
{
    public class LightcastAuthTokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}

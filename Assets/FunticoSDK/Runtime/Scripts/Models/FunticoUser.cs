using Newtonsoft.Json;

namespace FunticoSDK.Runtime.Scripts.Models
{
    public class FunticoUser
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("User_id")]
        public string UserId { get; set; }
    }
}
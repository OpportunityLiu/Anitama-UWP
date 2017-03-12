using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api.Auth
{
    class WechatAuthData
    {
        [JsonProperty("uid")]
        public string UId { get;private set; }
        [JsonProperty("isPay")]
        public string IsPay { get; private set; }
        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; private set; }
        [JsonProperty("nickname")]
        public string Nickname { get; private set; }
        [JsonProperty("accessToken")]
        public string Token { get; private set; }
        [JsonProperty("expireAt")]
        public DateTimeOffset ExpireAt { get; private set; }
    }
}

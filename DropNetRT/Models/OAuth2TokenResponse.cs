using System;
using System.Runtime.Serialization;

namespace DropNetRT.Models
{
    [DataContract]
    public class OAuth2TokenResponse
    {
        [DataMember(Name = "access_token")]
        public string Token { get; set; }
    }
}

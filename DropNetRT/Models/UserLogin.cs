using System.Runtime.Serialization;
namespace DropNetRT.Models
{
    [DataContract]
    public class UserLogin
    {
        public string Token { get; set; }
        public string Secret { get; set; }
    }
}

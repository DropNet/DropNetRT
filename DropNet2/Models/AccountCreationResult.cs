using System.Runtime.Serialization;

namespace DropNet2.Models
{
    public class AccountCreationResult
    {
        public enum ErrorTypes
        {
            Unknown,
            EmailInUse
        }
        
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorType")]
        public ErrorTypes ErrorType { get; set; }
    }
}

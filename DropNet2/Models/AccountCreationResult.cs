namespace DropNet2.Models
{
    public class AccountCreationResult
    {
        public enum ErrorTypes
        {
            Unknown,
            EmailInUse
        }

        public bool success { get; set; }
        public ErrorTypes errorType { get; set; }
    }
}

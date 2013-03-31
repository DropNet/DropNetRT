using System.Runtime.Serialization;

namespace DropNet2.Models
{
    [DataContract]
    public class AccountInfo
    {
        [DataMember(Name = "referral_link")]
        public string ReferralLink { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name ="email")]
        public string Email { get; set; }

        [DataMember(Name ="display_name")]
        public string DisplayName { get; set; }

        [DataMember(Name = "quota_info")]
        public QuotaInfo QuotaInfo { get; set; }

        [DataMember(Name = "uid")]
        public long Uid { get; set; }
    }

   
    public class QuotaInfo
    {
        [DataMember(Name = "shared")]
        public long Shared { get; set; }

        [DataMember(Name = "quota")]
        public long Quota { get; set; }
        
        [DataMember(Name = "normal")]
        public long Normal { get; set; }
    }
}

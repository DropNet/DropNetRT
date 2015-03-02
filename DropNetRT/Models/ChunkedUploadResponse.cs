using System;
using System.Runtime.Serialization;

namespace DropNetRT.Models
{
    [DataContract]
    public class ChunkedUploadResponse
    {
        [DataMember(Name = "upload_id")]
        public string UploadId { get; set; }

        [DataMember(Name = "offset")]
        public long Offset { get; set; }

        [DataMember(Name = "expires")]
        public DateTimeOffset Expires { get; set; }
    }
}
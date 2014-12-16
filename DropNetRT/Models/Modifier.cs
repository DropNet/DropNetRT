using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DropNetRT.Models
{
    public class Modifier
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("uid")]
        public string Uid { get; set; }
    }
}

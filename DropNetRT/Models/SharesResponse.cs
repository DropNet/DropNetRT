﻿using System;

namespace DropNetRT.Models
{
    public class ShareResponse
    {
        public string Url { get; set; }
        public string Expires { get; set; }
        public DateTime ExpiresDate
        {
            get { return DateTime.Parse(Expires); }
        }
    }
}

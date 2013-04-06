﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DropNet2.Models
{
    [DataContract]
    public class MetaData
    {
        [DataMember(Name = "hash")]
        public string Hash { get; set; }

        [DataMember(Name = "thumb_exists")]
        public bool ThumbExists { get; set; }

        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        [DataMember(Name = "modified")]
        public string Modified { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "is_dir")]
        public bool IsDirectory { get; set; }

        [DataMember(Name = "is_deleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "size")]
        public string Size { get; set; }

        [DataMember(Name = "root")]
        public string Root { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "revision")]
        public int Revision { get; set; }

        [DataMember(Name = "rev")]
        public string Rev { get; set; }

        [DataMember(Name = "contents")]
        public List<MetaData> Contents { get; set; }

        [IgnoreDataMember]
        public DateTime ModifiedDate
        {
            get
            {
                //cast to datetime and return
                return DateTime.Parse(Modified); //RFC1123 format date codes are returned by API
            }
        }

        [IgnoreDataMember]
        public DateTime UTCDateModified
        {
            get
            {
                string str = Modified;
                if (str.EndsWith(" +0000")) str = str.Substring(0, str.Length - 6);
                if (!str.EndsWith(" UTC")) str += " UTC";
                return DateTime.ParseExact(str, "ddd, d MMM yyyy HH:mm:ss UTC", System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                Modified = value.ToString("ddd, d MMM yyyy HH:mm:ss UTC");
            }
        }


        [IgnoreDataMember]
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(Path))
                {
                    return string.Empty;
                }

                if (Path.LastIndexOf("/") == -1)
                {
                    return string.Empty;
                }

                return string.IsNullOrEmpty(Path) ? "root" : Path.Substring(Path.LastIndexOf("/") + 1);
            }
        }

        [IgnoreDataMember]
        public string Extension
        {
            get
            {
                if (string.IsNullOrEmpty(Path))
                {
                    return string.Empty;
                }

                if (Path.LastIndexOf(".") == -1)
                {
                    return string.Empty;
                }

                return IsDirectory ? string.Empty : Path.Substring(Path.LastIndexOf("."));
            }
        }
    }

}

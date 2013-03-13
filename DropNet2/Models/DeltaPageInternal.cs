using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DropNet2.Models
{
    internal class DeltaPageInternal
    {
        public string Cursor { get; set; }
        public bool Has_More { get; set; }
        public bool Reset { get; set; }
        public List<List<string>> Entries { get; set; }
    }
}

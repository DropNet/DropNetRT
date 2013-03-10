﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DropNet2.HttpHelpers
{
    class QueryParameterComparer : IComparer<HttpParameter>
    {
        // Methods
        public int Compare(HttpParameter x, HttpParameter y)
        {
            return ((x.Name == y.Name) ? string.Compare(x.Value.ToString(), y.Value.ToString()) : string.Compare(x.Name, y.Name));
        }
    }
}

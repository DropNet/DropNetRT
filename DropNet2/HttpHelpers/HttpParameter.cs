using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DropNet2.HttpHelpers
{
    public class HttpParameter
    {
        public HttpParameter(string name, object value)
        {
            // TODO: Complete member initialization
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value of the parameter
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Return a human-readable representation of this parameter
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace DropNet2.HttpHelpers
{
    public class HttpRequest : HttpRequestMessage
    {
        public List<HttpParameter> Parameters { get; set; }

        public HttpRequest(HttpMethod httpMethod, string requestUrl)
            : base(httpMethod, requestUrl)
        {
            Parameters = new List<HttpParameter>();
        }

        public void AddParameter(string name, object value)
        {
            Parameters.Add(new HttpParameter(name, value));
        }

        public void AddBody(object obj)
        {
            Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}

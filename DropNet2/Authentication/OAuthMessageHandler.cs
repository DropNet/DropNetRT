using DropNet2.HttpHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DropNet2.Extensions;

namespace DropNet2.Authentication
{
    public class OAuthMessageHandler : DelegatingHandler
    {
        internal string ApiKey { get; set; }
        internal string ApiSecret { get; set; }
        internal string UserToken { get; set; }
        internal string UserSecret { get; set; }

        private const string SignatureMethod = "PLAINTEXT";
        private static readonly Random Random = new Random();

        public OAuthMessageHandler(HttpMessageHandler innerHandler, string apiKey, string apiSecret)
            : base(innerHandler)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        public OAuthMessageHandler(HttpMessageHandler innerHandler, string apiKey, string apiSecret, string userToken, string userSecret)
            : base(innerHandler)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            UserToken = userToken;
            UserSecret = userSecret;
        }

        /// <summary>
        /// Builds up the oauth parameters and updated the request Uri
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public HttpRequest Authenticate(HttpRequest request)
        {
            request.AddParameter("oauth_version", "1.0");
            request.AddParameter("oauth_nonce", GenerateNonce());
            request.AddParameter("oauth_timestamp", GenerateTimeStamp());
            request.AddParameter("oauth_signature_method", SignatureMethod);
            request.AddParameter("oauth_consumer_key", ApiKey);
            if (!string.IsNullOrEmpty(UserToken))
            {
                request.AddParameter("oauth_token", UserToken);
            }
            request.Parameters.Sort(new QueryParameterComparer());
            request.AddParameter("oauth_signature", GenerateSignature(request));

            //Now build the real Url
            var data = EncodeParameters(request);
            var url = string.Format("{0}?{1}", request.RequestUri, data);

            request.RequestUri = new Uri(url);

            //Does it need to do this?
            return request;
        }

        private object EncodeParameters(HttpRequest request)
        {
            var querystring = new StringBuilder();
            foreach (var p in request.Parameters)
            {
                if (querystring.Length > 1)
                {
                    querystring.Append("&");
                }
                querystring.AppendFormat("{0}={1}", p.Name.UrlEncode(), (p.Value.ToString()).UrlEncode());
            }

            return querystring.ToString();
        }

        private string GenerateNonce()
        {
            return Random.Next(0x1e208, 0x98967f).ToString();
        }

        private string GenerateSignature(HttpRequest request)
        {
            return ApiSecret + "&" + UserSecret;
        }

        private string GenerateTimeStamp()
        {
            TimeSpan span = DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(span.TotalSeconds).ToString();
        }
    }
}

using DropNet2.Authentication;
using DropNet2.HttpHelpers;
using DropNet2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DropNet2
{
    public partial class DropNetClient
    {
        private const string ApiBaseUrl = "https://api.dropbox.com";
        private const string ApiContentBaseUrl = "https://api-content.dropbox.com";

        public UserLogin UserLogin { get; set; }

        /// <summary>
        /// To use Dropbox API in sandbox mode (app folder access) set to true
        /// </summary>
        public bool UseSandbox { get; set; }

        private const string SandboxRoot = "sandbox";
        private const string DropboxRoot = "dropbox";

        private readonly string _apiKey;
        private readonly string _apisecret;

        private HttpMessageHandler _httpHandler;
        private OAuthMessageHandler _oauthHandler;
        private HttpClient _httpClient;

        /// <summary>
        /// Gets the directory root for the requests (full or sandbox mode)
        /// </summary>
        string Root
        {
            get { return UseSandbox ? SandboxRoot : DropboxRoot; }
        }

        /// <summary>
        /// Default Constructor for the DropboxClient
        /// </summary>
        /// <param name="apiKey">The Api Key to use for the Dropbox Requests</param>
        /// <param name="appSecret">The Api Secret to use for the Dropbox Requests</param>
        public DropNetClient(string apiKey, string appSecret)
        {
            _apiKey = apiKey;
            _apisecret = appSecret;

            LoadClient();
        }

        /// <summary>
        /// Creates an instance of the DropNetClient given an API Key/Secret and a User Token/Secret
        /// </summary>
        /// <param name="apiKey">The Api Key to use for the Dropbox Requests</param>
        /// <param name="apiSecret">The Api Secret to use for the Dropbox Requests</param>
        /// <param name="userToken">The User authentication token</param>
        /// <param name="userSecret">The Users matching secret</param>
        public DropNetClient(string apiKey, string apiSecret, string userToken, string userSecret)
        {
            _apiKey = apiKey;
            _apisecret = apiSecret;

            UserLogin = new UserLogin { Token = userToken, Secret = userSecret };

            LoadClient();
        }

        private void LoadClient()
        {
            //Default to full access
            UseSandbox = false;

            _httpHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            if (UserLogin != null)
            {
                _oauthHandler = new OAuthMessageHandler(_httpHandler, _apiKey, _apisecret, UserLogin.Token, UserLogin.Secret);
            }
            else
            {
                _oauthHandler = new OAuthMessageHandler(_httpHandler, _apiKey, _apisecret);
            }
            _httpClient = new HttpClient(_oauthHandler);
        }

        public void SetUserToken(string userToken, string userSecret)
        {
            SetUserToken(new UserLogin { Token = userToken, Secret = userSecret });
        }

        public void SetUserToken(UserLogin userLogin)
        {
            UserLogin = userLogin;

            _oauthHandler.UserToken = userLogin.Token;
            _oauthHandler.UserSecret = userLogin.Secret;
        }

        private string MakeRequestString(string action, ApiType apiType)
        {
            //TODO - check for /
            return string.Format("{0}/{1}", apiType == ApiType.Base ? ApiBaseUrl : ApiContentBaseUrl, action);
        }
    
        enum ApiType
        {
            Base,
            Content
        }

        public string BuildAuthorizeUrl(UserLogin userLogin, string callback = null)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException("userLogin");
            }

            //Go 1-Liner!
            return string.Format("https://www.dropbox.com/1/oauth/authorize?oauth_token={0}{1}", userLogin.Token,
                (string.IsNullOrEmpty(callback) ? string.Empty : "&oauth_callback=" + callback));
        }

        private UserLogin GetUserLoginFromParams(string urlParams)
        {
            var userLogin = new UserLogin();

            //TODO - Make this not suck
            var parameters = urlParams.Split('&');

            foreach (var parameter in parameters)
            {
                if (parameter.Split('=')[0] == "oauth_token_secret")
                {
                    userLogin.Secret = parameter.Split('=')[1];
                }
                else if (parameter.Split('=')[0] == "oauth_token")
                {
                    userLogin.Token = parameter.Split('=')[1];
                }
            }

            return userLogin;
        }

        public async Task<T> SendAsync<T>(HttpRequest request) where T : class
        {
            //Authenticate with oauth
            _oauthHandler.Authenticate(request);

            var response = await _httpClient.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

    }
}

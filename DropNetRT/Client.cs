using System.Threading;
using DropNetRT.Authentication;
using DropNetRT.Exceptions;
using DropNetRT.HttpHelpers;
using DropNetRT.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DropNetRT
{
    public partial class DropNetClient
    {
        private const string ApiBaseUrl = "https://api.dropbox.com";
        private const string ApiContentBaseUrl = "https://api-content.dropbox.com";
        private const string ApiNotifyBaseUrl = "https://api-notify.dropbox.com";

        /// <summary>
        /// Do not set this property directly, instead use SetUserToken
        /// </summary>
        public UserLogin UserLogin { get; set; }

        /// <summary>
        /// To use Dropbox API in sandbox mode (app folder access) set to true
        /// </summary>
        public bool UseSandbox { get; set; }

        public TimeSpan Timeout
        {
            get { return _httpClient.Timeout; }
            set { _httpClient.Timeout = value; }
        }

        private const string SandboxRoot = "sandbox";
        private const string DropboxRoot = "dropbox";

        private readonly string _apiKey;
        private readonly string _apisecret;

        private HttpMessageHandler _httpHandler;
        private OAuthMessageHandler _oauthHandler;
        private HttpClient _httpClient;

        private const string _lineBreak = "\r\n";
        private const string _formBoundary = "-----------------------------28947758029299";

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

        /// <summary>
        /// Internal method to load up the HttpClient stuff
        /// </summary>
        private void LoadClient()
        {
            //Default to full access
            UseSandbox = false;

            _httpHandler = new HttpClientHandler();

            if (UserLogin != null)
            {
                _oauthHandler = new OAuthMessageHandler(_httpHandler, _apiKey, _apisecret, UserLogin.Token, UserLogin.Secret);
            }
            else
            {
                _oauthHandler = new OAuthMessageHandler(_httpHandler, _apiKey, _apisecret);
            }
            _httpClient = new HttpClient(_oauthHandler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "DropNetRT");
            if (_httpClient.DefaultRequestHeaders.Any(h => h.Key == "Connection"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Connection");
            }
        }
    
        enum ApiType
        {
            Base,
            Content,
            Notify
        }


        /// <summary>
        /// Wrapper around the HttpClient.SendAsync function with error handling
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<T> SendAsync<T>(HttpRequest request, CancellationToken cancellationToken) where T : class
        {
            string responseBody = await SendAsync(request, cancellationToken);

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private async Task<string> SendAsync(HttpRequest request, CancellationToken cancellationToken)
        {
            //Authenticate with oauth
            _oauthHandler.Authenticate(request);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DropboxException(ex);
            }

            //TODO - More Error Handling
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new DropboxException(response);
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}

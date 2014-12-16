using System.Threading;
using DropNetRT.HttpHelpers;
using DropNetRT.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace DropNetRT
{
    public partial class DropNetClient
    {

        /// <summary>
        /// Auth Step 1. Gets a Request Token which is required for the login request
        /// </summary>
        /// <returns></returns>
        public async Task<UserLogin> GetRequestToken()
        {
            var requestUrl = MakeRequestString("1/oauth/request_token", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            _oauthHandler.Authenticate(request);

            var response = await _httpClient.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();

            UserLogin = GetUserLoginFromParams(responseBody);

            SetUserToken(UserLogin);

            return UserLogin;
        }

        /// <summary>
        /// Auth Step 3. Once a Request Token has been authorized convert it to an access token for API usage.
        /// </summary>
        /// <returns></returns>
        public async Task<UserLogin> GetAccessToken()
        {
            var requestUrl = MakeRequestString("1/oauth/access_token", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            _oauthHandler.Authenticate(request);

            var response = await _httpClient.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();

            UserLogin = GetUserLoginFromParams(responseBody);

            SetUserToken(UserLogin);

            return UserLogin;
        }

        /// <summary>
        /// Gets the account info of the current logged in user
        /// </summary>
        /// <returns></returns>
        public Task<AccountInfo> AccountInfo()
        {
            return AccountInfo(CancellationToken.None);
        }

        /// <summary>
        /// Gets the account info of the current logged in user
        /// </summary>
        /// <returns></returns>
        public async Task<AccountInfo> AccountInfo(CancellationToken cancellationToken)
        {
            var requestUrl = MakeRequestString("1/account/info", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<AccountInfo>(request, cancellationToken);

            return response;
        }

        /// <summary>
        /// Gets the oauth2 token for current logged in user
        /// </summary>
        /// <returns></returns>
        public Task<string> GetOAuth2Token()
        {
            return GetOAuth2Token(CancellationToken.None);
        }

        /// <summary>
        /// Gets the oauth2 token for current logged in user
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOAuth2Token(CancellationToken cancellationToken)
        {
            var requestUrl = MakeRequestString("1/oauth2/token_from_oauth1", ApiType.Base);

            var request = new HttpRequest (HttpMethod.Post, requestUrl);

            var response = await SendAsync<OAuth2TokenResponse>(request, cancellationToken);

            return response.Token;
        }

        /// <summary>
        /// Disables the access token used to authenticate the call.
        /// This method works for OAuth 1 and OAuth 2 tokens.
        /// </summary>
        public async Task DisableAccessToken(CancellationToken cancellationToken)
        {
            var requestUrl = MakeRequestString("1/disable_access_token", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Post, requestUrl);

            await SendAsync(request, cancellationToken);
        }
    }
}

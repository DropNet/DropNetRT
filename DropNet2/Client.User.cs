using DropNet2.HttpHelpers;
using DropNet2.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace DropNet2
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
        public async Task<AccountInfo> AccountInfo()
        {
            var requestUrl = MakeRequestString("1/account/info", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<AccountInfo>(request);

            return response;
        }

    }
}

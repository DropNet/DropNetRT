using DropNet2.HttpHelpers;
using DropNet2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DropNet2
{
    public partial class DropNetClient
    {

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


        public async Task<AccountInfo> AccountInfo()
        {
            var requestUrl = MakeRequestString("1/account/info", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<AccountInfo>(request);

            return response;
        }

    }
}

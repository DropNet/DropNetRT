using DropNetRT.HttpHelpers;
using DropNetRT.Extensions;
using DropNetRT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace DropNetRT
{
    public partial class DropNetClient
    {
        /// <summary>
        /// Converts the ThumbnailSize enum to the string equivalent for the API
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string ThumbnailSizeString(ThumbnailSize size)
        {
            switch (size)
            {
                case ThumbnailSize.Small:
                    return "small";
                case ThumbnailSize.Medium:
                    return "medium";
                case ThumbnailSize.Large:
                    return "large";
                case ThumbnailSize.ExtraLarge:
                    return "l";
                case ThumbnailSize.ExtraLarge2:
                    return "xl";
            }
            return "s";
        }
        
        /// <summary>
        /// Sets a UserLogin (Token and Secret) for the current client
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="userSecret"></param>
        public void SetUserToken(string userToken, string userSecret)
        {
            SetUserToken(new UserLogin { Token = userToken, Secret = userSecret });
        }

        /// <summary>
        /// Sets a UserLogin (Token and Secret) for the current client
        /// </summary>
        /// <param name="userLogin"></param>
        public void SetUserToken(UserLogin userLogin)
        {
            UserLogin = userLogin;

            if (_oauthHandler != null) //not sure when this would ever be null
            {
                if (userLogin == null) //logout
                {
                    _oauthHandler.UserToken = string.Empty;
                    _oauthHandler.UserSecret = string.Empty;
                }
                else
                {
                    _oauthHandler.UserToken = userLogin.Token;
                    _oauthHandler.UserSecret = userLogin.Secret;
                }
            }
        }

        /// <summary>
        /// Internal method to build the full request Url
        /// </summary>
        /// <param name="action"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        private string MakeRequestString(string action, ApiType apiType)
        {
            //TODO - check for /
            return string.Format("{0}/{1}", GetBaseUrl(apiType), action);
        }

        private static string GetBaseUrl(ApiType apiType)
        {
            switch (apiType)
            {
                case ApiType.Base:
                    return ApiBaseUrl;
                case ApiType.Content:
                    return ApiContentBaseUrl;
                case ApiType.Notify:
                    return ApiNotifyBaseUrl;
                default:
                    throw new ArgumentOutOfRangeException("apiType");
            }
        }


        /// <summary>
        /// Gets an authorize url for the current UserLogin (after getting a Request Token)
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Reads the auth response parameters and creates a UserLogin object from it
        /// </summary>
        /// <param name="urlParams"></param>
        /// <returns></returns>
        protected UserLogin GetUserLoginFromParams(string urlParams)
        {
            var userLogin = new UserLogin();

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

        /// <summary>
        /// Builds a DeltaEntry object from a list of JRaw responses
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        private DeltaEntry JRawListToDeltaEntry(List<JRaw> stringList)
        {
            var deltaEntry = new DeltaEntry
            {
                Path = JToken.Parse(stringList[0].ToString()).Value<string>()
            };
            if (!String.IsNullOrEmpty(stringList[1].ToString()))
            {
                deltaEntry.MetaData = JsonConvert.DeserializeObject<Metadata>(stringList[1].ToString());
            }
            return deltaEntry;
        }


        private HttpRequest MakeGetFileRequest(string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/files/{0}/{1}", Root, path.CleanPath()), ApiType.Content);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            _oauthHandler.Authenticate(request);

            return request;
        }

        private HttpRequest MakeThumbnailRequest(string path, ThumbnailSize size)
        {
            var requestUrl = MakeRequestString(string.Format("1/thumbnails/{0}/{1}", Root, path.CleanPath()), ApiType.Content);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.AddParameter("size", ThumbnailSizeString(size));

            _oauthHandler.Authenticate(request);

            return request;
        }

        private HttpRequest MakeUploadRequest(string path, string filename)
        {
            var requestUrl = MakeRequestString(string.Format("1/files/{0}/{1}", Root, path.CleanPath()), ApiType.Content);

            var request = new HttpRequest(HttpMethod.Post, requestUrl);

            _oauthHandler.Authenticate(request);

            return request;
        }

        private HttpRequest MakeUploadPutRequest(string path, string filename)
        {
            var requestUrl = MakeRequestString(string.Format("1/files_put/{0}/{1}/{2}", Root, path.CleanPath(), filename), ApiType.Content);

            var request = new HttpRequest(HttpMethod.Put, requestUrl);

            _oauthHandler.Authenticate(request);

            return request;
        }
    }
}

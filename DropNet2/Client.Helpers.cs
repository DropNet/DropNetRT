using DropNet2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DropNet2
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
                _oauthHandler.UserToken = userLogin.Token;
                _oauthHandler.UserSecret = userLogin.Secret;
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
            return string.Format("{0}/{1}", apiType == ApiType.Base ? ApiBaseUrl : ApiContentBaseUrl, action);
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
        private UserLogin GetUserLoginFromParams(string urlParams)
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
        /// Builds a DeltaEntry object from a list of string responses
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        private DeltaEntry StringListToDeltaEntry(List<string> stringList)
        {
            var deltaEntry = new DeltaEntry
            {
                Path = stringList[0]
            };
            if (!String.IsNullOrEmpty(stringList[1]))
            {
                deltaEntry.MetaData = JsonConvert.DeserializeObject<MetaData>(stringList[1]);
            }
            return deltaEntry;
        } 

    }
}

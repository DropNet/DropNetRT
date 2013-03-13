using DropNet2.HttpHelpers;
using DropNet2.Models;
using Newtonsoft.Json;
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


        private const string FormBoundary = "-----------------------------28947758029299";
        private static string GetMultipartFileHeader(string fileName)
        {
            return string.Format("--{0}{4}Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"{4}Content-Type: {3}{4}{4}",
                FormBoundary, fileName, fileName, "application/octet-stream", _lineBreak);
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

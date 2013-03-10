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

        public async Task<MetaData> GetMetaData(string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/metadata/{0}{1}", Root, path), ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<MetaData>(request);

            return response;
        }

        public async Task<ShareResponse> GetShare(string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/shares/{0}{1}", Root, path), ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<ShareResponse>(request);

            return response;
        }

    }
}

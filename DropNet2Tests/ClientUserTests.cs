using System.Threading.Tasks;
using DropNet2;
using NUnit.Framework;

namespace DropNet2Tests
{
    [TestFixture]
    public class ClientUserTests
    {   

        [Test]
        public async Task When_Token_Requested_Then_User_Token_Is_Returned()
        {   
            var client = new DropNetClient(AppKey, AppSecret);
            var userToken = await client.GetRequestToken();
            Assert.NotNull(userToken);
        }

        [Test]
        public async Task Given_UserToken_When_Build_Auth_Url_Then_The_Authentication_Url_Is_Returned()
        {
            var client = new DropNetClient(AppKey, AppSecret);

            var userToken = await client.GetRequestToken();
            string url = client.BuildAuthorizeUrl(userToken, "http://cloudyboxapp.com");
            Assert.IsNotEmpty(url);
        }

        [Test]
        public async Task Given_A_Clent_Get_User_Account_Infromation()
        {
            var client = new DropNetClient(AppKey, AppSecret, UserToken, UserSecret);
            var accountInfromation = await client.AccountInfo();

            Assert.NotNull(accountInfromation);
            Assert.NotNull(accountInfromation.QuotaInfo);
        }

        [Test]
        [Explicit("This test require user input.")]
        public async Task Get_Access_Token_Test()
        {
            var client = new DropNetClient(AppKey, AppSecret);

            var userToken = await client.GetRequestToken();
            
            //Open the url in browser and login
            string url = client.BuildAuthorizeUrl(userToken, "http://cloudyboxapp.com");
            var user = await client.GetAccessToken();

            Assert.NotNull(user);
        }

        private const string UserSecret = "ep0bg2jkoogl5o3";
        private const string UserToken = "pgeb5zzmgeh37xt";

        private const string AppKey = "g1znvbjklqyeve8";
        private const string AppSecret = "13fvio3d1bzhkfv";
    }
}

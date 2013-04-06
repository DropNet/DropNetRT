using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using DropNet2;
using DropNet2.Exceptions;
using NUnit.Framework;

namespace DropNet2Tests
{
    [TestFixture]
    public class ClientFileTests
    {

        [SetUp]
        public void Setup()
        {
            _client = new DropNetClient(AppKey, AppSecret, UserToken, UserSecret);
        }

        [Test]
        public async Task Given_A_Root_Path_Get_Metadata()
        {
            var data = await _client.GetMetaData("/");
            Assert.NotNull(data);
            Assert.IsTrue(data.IsDirectory);
        }

        [Test]
        public async Task Given_A_Folder_Path_Get_Metadata()
        {
            var data = await _client.GetMetaData("/photos");
            Assert.NotNull(data);
            Assert.IsTrue(data.IsDirectory, "Give path is not a directory");
            bool nameMatches = string.Compare(data.Name, "photos", StringComparison.InvariantCultureIgnoreCase) == 0;

            Assert.IsTrue(nameMatches);
        }

        [Test]
        public async Task Given_A_File_Path_Get_Metadata()
        {
            var data = await _client.GetMetaData("/getting started.pdf");
            Assert.NotNull(data);
            Assert.IsFalse(data.IsDirectory, "Give path is not a file");
            bool nameMatches = string.Compare(data.Name, "getting started.pdf", StringComparison.InvariantCultureIgnoreCase) == 0;

            Assert.IsTrue(nameMatches);
        }

        [Test]
        public async Task Given_A_Non_Existing_File_Path_Throws_DropNet_Exception_With_NotFound_Response()
        {
            try
            {
                await _client.GetMetaData("/DoesNotExist.pdf");
            }
            catch (DropboxException exception)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
            }
        }

        [Test]
        public async Task Given_Path_Get_A_Share_Link()
        {
            var share = await _client.GetShare("/photos");
            
            Assert.NotNull(share);
            Assert.IsNotNull(share.Url);

            Debug.WriteLine(share.Url);
        }

        [Test]
        public async Task Given_Search_String_Find_The_Matching_Items()
        {
            var items = await _client.Search("pdf");

            Assert.IsNotNull(items);
            Assert.AreEqual(1, items.Count);
        }

        [Test]
        public async Task Given_Search_String_If_No_Items_Found_Return_Empty_List()
        {
            var items = await _client.Search("Does not exist");

            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count);
        }

        [Test]
        public async Task Give_Search_String_And_Path_Find_The_Matching_Items()
        {
            var items = await _client.Search("jpg", "/photos");

            Assert.IsNotNull(items);
            Assert.AreEqual(3, items.Count);
        }
  
        private DropNetClient _client;

        private const string UserSecret = "h18w80z4c7h5gmn";
        private const string UserToken = "dkms8xc4x3mtm19";

        private const string AppKey = "7zqu95o832tr0kb";
        private const string AppSecret = "j1k6gklasd5knv5";
    }
}
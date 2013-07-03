using DropNet2.Exceptions;
using DropNet2.Extensions;
using DropNet2.HttpHelpers;
using DropNet2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DropNet2
{
    public partial class DropNetClient
    {

        /// <summary>
        /// Gets MetaData for a file or folder given the path
        /// </summary>
        /// <param name="path">Path to file or folder</param>
        /// <returns><see cref="MetaData"/> for a file or folder</returns>
        public async Task<MetaData> GetMetaData(string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/metadata/{0}/{1}", Root, path.CleanPath()), ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<MetaData>(request);

            return response;
        }

        /// <summary>
        /// Gets a share link from a give path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ShareResponse> GetShare(string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/shares/{0}/{1}", Root, path.CleanPath()), ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<ShareResponse>(request);

            return response;
        }

        /// <summary>
        /// Gets a share link from a give path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        public async Task<ShareResponse> GetShare(string path, bool shortUrl)
        {
            var requestUrl = MakeRequestString(string.Format("1/shares/{0}/{1}", Root, path.CleanPath()), ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.Parameters.Add(new HttpParameter("short_url", shortUrl));

            var response = await SendAsync<ShareResponse>(request);

            return response;
        }

        /// <summary>
        /// Searches for a given text in the entire dropbox/sandbox folder
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<MetaData>> Search(string searchString)
        {
            return await Search(searchString, string.Empty);
        }

        /// <summary>
        /// Searches for a given text in a specified folder
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<List<MetaData>> Search(string searchString, string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/search/{0}/{1}", Root, path.CleanPath()), ApiType.Base);
            
            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.Parameters.Add(new HttpParameter("query", searchString));

            var response = await SendAsync<List<MetaData>>(request);

            return response;
        }

        /// <summary>
        /// Gets a file from the given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<byte[]> GetFile(string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/files/{0}/{1}", Root, path.CleanPath()), ApiType.Content);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            _oauthHandler.Authenticate(request);

            var response = await _httpClient.SendAsync(request);

            //TODO - Error Handling

            return await response.Content.ReadAsByteArrayAsync();
        }


        /// <summary>
        /// Uploads a file to a Dropbox folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public async Task<MetaData> Upload(string path, string filename, byte[] fileData)
        {
            var requestUrl = MakeRequestString(string.Format("1/files/{0}/{1}", Root, path.CleanPath()), ApiType.Content);

            var request = new HttpRequest(HttpMethod.Post, requestUrl);

            _oauthHandler.Authenticate(request);

            var content = new MultipartFormDataContent(_formBoundary);

            foreach (var parm in request.Parameters)
            {
                content.Add(new StringContent(parm.Value.ToString()), parm.Name);
            }

            var fileContent = new ByteArrayContent(fileData);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("file")
            {
                FileName = filename,
                Name = "file"
            };
            fileContent.Headers.Add("Content-Type", "application/octet-stream");
            content.Add(fileContent);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(requestUrl, content);
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

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MetaData>(responseBody);
        }


        /// <summary>
        /// Uploads a file to a Dropbox folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<MetaData> Upload(string path, string filename, Stream fileStream)
        {
            var rawBytes = ReadFully(fileStream);
            return await Upload(path, filename, rawBytes);
        }

        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// Deletes the file or folder from dropbox with the given path
        /// </summary>
        /// <param name="path">The Path of the file or folder to delete.</param>
        /// <returns></returns>
        public async Task<MetaData> Delete(string path)
        {
            var requestUrl = MakeRequestString("1/fileops/delete", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.AddParameter("path", path);
            request.AddParameter("root", Root);

            var response = await SendAsync<MetaData>(request);

            return response;
        }

        /// <summary>
        /// Copies a file or folder on Dropbox
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        public async Task<MetaData> Copy(string fromPath, string toPath)
        {
            var requestUrl = MakeRequestString("1/fileops/copy", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.AddParameter("from_path", fromPath);
            request.AddParameter("to_path", toPath);
            request.AddParameter("root", Root);

            var response = await SendAsync<MetaData>(request);

            return response;
        }

        /// <summary>
        /// Moves a file or folder on Dropbox
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        public async Task<MetaData> Move(string fromPath, string toPath)
        {
            var requestUrl = MakeRequestString("1/fileops/move", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.AddParameter("from_path", fromPath);
            request.AddParameter("to_path", toPath);
            request.AddParameter("root", Root);

            var response = await SendAsync<MetaData>(request);

            return response;
        }

        /// <summary>
        /// Created a new folder with the given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<MetaData> CreateFolder(string path)
        {
            var requestUrl = MakeRequestString("1/fileops/create_folder", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.AddParameter("path", path);
            request.AddParameter("root", Root);

            var response = await SendAsync<MetaData>(request);

            return response;
        }

        /// <summary>
        /// Returns a link directly to a file.
        /// Similar to /shares. The difference is that this bypasses the Dropbox webserver, used to provide a preview of the file, so that you can effectively stream the contents of your media.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ShareResponse> GetMedia(string path)
        {
            var requestUrl = MakeRequestString(string.Format("1/media/{0}/{1}", Root, path.CleanPath()), ApiType.Base);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);

            var response = await SendAsync<ShareResponse>(request);

            return response;
        }

        /// <summary>
        /// Gets the thumbnail of an image given its MetaData
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<byte[]> GetThumbnail(MetaData file)
        {
            return await GetThumbnail(file.Path, ThumbnailSize.Small);
        }

        /// <summary>
        /// Gets the thumbnail of an image given its MetaData
        /// </summary>
        /// <param name="file"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<byte[]> GetThumbnail(MetaData file, ThumbnailSize size)
        {
            return await GetThumbnail(file.Path, size);
        }

        /// <summary>
        /// Gets the thumbnail of an image given its path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<byte[]> GetThumbnail(string path)
        {
            return await GetThumbnail(path, ThumbnailSize.Small);
        }

        /// <summary>
        /// Gets the thumbnail of an image given its path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<byte[]> GetThumbnail(string path, ThumbnailSize size)
        {
            var requestUrl = MakeRequestString(string.Format("1/thumbnails/{0}/{1}", Root, path.CleanPath()), ApiType.Content);

            var request = new HttpRequest(HttpMethod.Get, requestUrl);
            request.AddParameter("size", ThumbnailSizeString(size));

            _oauthHandler.Authenticate(request);

            var response = await _httpClient.SendAsync(request);

            //TODO - Error Handling

            return await response.Content.ReadAsByteArrayAsync();
        }


        /// <summary>
        /// Gets the deltas for a user's folders and files.
        /// </summary>
        /// <param name="cursor">The value returned from the prior call to GetDelta or an empty string</param>
        /// <returns></returns>
        public async Task<DeltaPage> GetDelta(string cursor)
        {
            var requestUrl = MakeRequestString("1/delta", ApiType.Base);

            var request = new HttpRequest(HttpMethod.Post, requestUrl);

            request.AddParameter("cursor", cursor);

            _oauthHandler.Authenticate(request);

            var response = await _httpClient.SendAsync(request);

            //TODO - Error Handling

            string responseBody = await response.Content.ReadAsStringAsync();

            var deltaResponse = JsonConvert.DeserializeObject<DeltaPageInternal>(responseBody);

            var deltaPage = new DeltaPage
            {
                Cursor = deltaResponse.Cursor,
                Has_More = deltaResponse.Has_More,
                Reset = deltaResponse.Reset,
                Entries = new List<DeltaEntry>()
            };

            foreach (var stringList in deltaResponse.Entries)
            {
                deltaPage.Entries.Add(StringListToDeltaEntry(stringList));
            }

            return deltaPage;
        }


    }
}

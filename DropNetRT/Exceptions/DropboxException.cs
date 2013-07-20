using System;
using System.Net;
using System.Net.Http;

namespace DropNetRT.Exceptions
{
    public class DropboxException : Exception
    {
        public HttpResponseMessage Response { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public DropboxException() { }

        public DropboxException(HttpStatusCode status)
        {
            StatusCode = status;
        }

        public DropboxException(HttpResponseMessage response)
            : this(response.StatusCode)
        {
            Response = response;
        }

        public DropboxException(Exception ex)
            : base("Dropbox error occurred", ex)
        {
        }
    }
}

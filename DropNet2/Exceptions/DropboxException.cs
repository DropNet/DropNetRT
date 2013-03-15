using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DropNet2.Exceptions
{
    public class DropboxException : Exception
    {
        public System.Net.Http.HttpResponseMessage Response;

        public HttpStatusCode StatusCode { get; set; }

        public DropboxException() { }

        public DropboxException(HttpStatusCode status)
        {
            StatusCode = status;
        }

        public DropboxException(System.Net.Http.HttpResponseMessage response)
        {
            Response = response;
        }

        public DropboxException(Exception ex)
            :base("Dropbox error occurred", ex)
        {
        }
    }
}

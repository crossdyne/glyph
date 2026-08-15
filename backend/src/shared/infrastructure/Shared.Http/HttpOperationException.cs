using System.Net;

namespace Shared.Http
{
    public class HttpOperationException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorBody { get; }

        public HttpOperationException(HttpStatusCode statusCode, string errorBody) 
            : base($"HTTP error {statusCode}: {errorBody}")
        {
            StatusCode = statusCode;
            ErrorBody = errorBody;
        }
    }
}
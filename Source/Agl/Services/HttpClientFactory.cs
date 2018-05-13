using System.Net.Http;

namespace Agl.Services
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}
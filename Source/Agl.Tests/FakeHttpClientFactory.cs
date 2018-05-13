using System.Net.Http;
using Agl.Services;

namespace Agl.Tests
{
    internal class FakeHttpClientFactory : IHttpClientFactory
    {
        private readonly IHttp _http;

        public FakeHttpClientFactory(IHttp http)
        {
            _http = http;
        }

        public HttpClient Create()
        {
            return new HttpClient(new FakeHandler(_http));
        }
    }
}
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Agl.Tests
{
    internal class FakeHandler : HttpMessageHandler
    {
        private readonly IHttp _http;

        public FakeHandler(IHttp http)
        {
            _http = http;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_http.Send(request));
        }
    }
}
using System.Net.Http;

namespace Agl.Tests
{
    public interface IHttp
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}
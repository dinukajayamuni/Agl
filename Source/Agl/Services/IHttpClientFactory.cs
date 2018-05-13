using System.Net.Http;

namespace Agl.Services
{
    internal interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
using System.Net.Http;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Net.Rest
{
    public interface IRestClient
    {
        Task<HttpResponseMessage> GetAsync(string relativeUrl);
        Task<HttpResponseMessage> PutAsync(string relativeUrl, object value);
    }
}

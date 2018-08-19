using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Net.Rest
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;
        private const string MimeType = "application/json";

        public RestClient(IHttpClientFactory httpClientFactory,
            IOptions<ExternalPaymentServiceOptions> externalPaymentServiceOptionsAccessor)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            if (externalPaymentServiceOptionsAccessor == null)
                throw new ArgumentNullException(nameof(externalPaymentServiceOptionsAccessor));

            var options = externalPaymentServiceOptionsAccessor.Value;
            _baseUrl = options.BaseUrl;

        }

        public async Task<HttpResponseMessage> GetAsync(string relativeUrl)
        {
            if (relativeUrl == null) throw new ArgumentNullException(nameof(relativeUrl));

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = CreateUri(relativeUrl),
            };

            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync(string relativeUrl, object value)
        {
            if (relativeUrl == null) throw new ArgumentNullException(nameof(relativeUrl));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var request = CreatePutRequest(relativeUrl, value);

            return await SendAsync(request);
        }

        private HttpRequestMessage CreatePutRequest(string relativeUrl, object value)
        {
            var content = JsonConvert.SerializeObject(value);

            return new HttpRequestMessage
            {
                Content = new StringContent(content, Encoding.UTF8, MimeType),
                Method = HttpMethod.Put,
                RequestUri = CreateUri(relativeUrl),
            };
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var client = _httpClientFactory.CreateClient();

            request.Headers.Add("Accept", MimeType);

            return await client.SendAsync(request);
        }

        private Uri CreateUri(string relativePath) => new Uri(_baseUrl + relativePath);
    }
}

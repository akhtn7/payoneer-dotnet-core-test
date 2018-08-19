using Newtonsoft.Json;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Net.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Service.Internal
{
    public class PaymentServiceInternal : IPaymentServiceInternal
    {
        private readonly IRestClient _restClient;

        public PaymentServiceInternal(IRestClient restClient) =>
            _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));

        public async Task<IEnumerable<Payment>> GetPaymentsFiltered(IEnumerable<PaymentStatus> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var queryItems = filter.Select(x => (int)x).Select(x => "paymentStatus=" + x);
            var query = string.Join("&", queryItems);
            var url = "api/payments";

            if (query.Length > 0) url += "?" + query;

            var response = await _restClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var payments = JsonConvert.DeserializeObject<IEnumerable<Payment>>(result).ToList();
            return payments;
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var response = await _restClient.GetAsync($"api/payments/{id}");
            var result = await response.Content.ReadAsStringAsync();
            var payment = JsonConvert.DeserializeObject<Payment>(result);
            return payment;
        }

        public async Task<PaymentChangeStatusResponse> UpdatePaymentStatusAsync(PaymentChangeStatusRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var response = await _restClient.PutAsync($"api/payments/{request.Id}", request);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Extrernal payment service return an error: '{response.StatusCode}'");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaymentChangeStatusResponse>(result);
        }
    }
}
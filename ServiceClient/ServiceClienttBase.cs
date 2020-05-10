using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceClient.Contracts;

namespace ServiceClient
{
    /// <summary>
    /// Base Service client used to connect to a service
    /// </summary>
    public abstract class ServiceClientBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        // these values should come from a config file
        private const int MaxNumberOfRetries = 3;
        private const int BackOffDelay = 1000;

        public Dictionary<String, String> Headers { protected set; get; } // collection of headers in the format header key, header value 

        protected ServiceClientBase(IHttpClientFactory httpClientFactory, ILogger<ServiceClientBase> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClientFactory.Client ?? throw new ArgumentNullException(nameof(httpClientFactory));

            Headers = new Dictionary<string, string>();
        }


        protected async Task<HttpResponseMessage> GetResponseMessage(string serviceEndPointUri)
        {
            // naive retry and back-off mechanism.
            var retryCount = 0;
            
            while (retryCount < MaxNumberOfRetries)
            {
                HttpResponseMessage response = await _httpClient.SendAsync(RequestMessage(serviceEndPointUri));
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                // we only retry service unavailable statuses

                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    retryCount++;
                    if (response.Headers.RetryAfter.Delta != null)
                    {
                        var delay = (int) response.Headers.RetryAfter.Delta.Value.TotalMilliseconds + (retryCount - 1) * BackOffDelay;
                    
                        _logger.LogInformation($"Calling endpoint {serviceEndPointUri} retry attempt {retryCount} in {delay} ms");

                        System.Threading.Thread.Sleep(delay);
                    }
                }
                else
                {
                    return response;
                }
            }

            throw new ServiceException("Could not connect to service");
        }

        protected HttpRequestMessage RequestMessage(string serviceEndPointUri)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(serviceEndPointUri),
                Method = HttpMethod.Get,
            };

            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            return request;
        }
    }
}

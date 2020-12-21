using System;
using System.Net.Http;
using IntegrationTests.Infrastructure.Utilities;

namespace IntegrationTests.Infrastructure
{
    public class BaseHttpApiTest: BaseTest, IDisposable
    {
        protected readonly HttpApiFactory Factory;

        protected BaseHttpApiTest()
        {
            Factory = new HttpApiFactory();
            DatabaseUtility.CleanUp().Wait();
        }

        private HttpClient _httpClient;
        protected HttpClient HttpClient
        {
            get { return _httpClient ??= Factory.CreateClient(); }
        }

        public void Dispose()
        {
            Factory?.Dispose();
        }
    }
}

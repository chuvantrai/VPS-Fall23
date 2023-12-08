using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace InvoiceApi.ExternalData
{
    public class RestClient : HttpClient
    {
        public RestClient(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException();
            }

            this.BaseAddress = new Uri(baseUrl);
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.SecurityProtocol |=
                SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public async Task<T> Get<T>(string requestUri)
        {
            HttpResponseMessage response = await this.GetAsync(requestUri);

            // Throw exception if HTTP Status code is not Success (2xx)
            response.EnsureSuccessStatusCode();

            var resultData = await response.Content.ReadFromJsonAsync<T>();

            return resultData;
        }

        public HttpContent GetFile(string requestUri)
        {
            HttpResponseMessage response = this.GetAsync(requestUri).Result;
            response.EnsureSuccessStatusCode();
            return response.Content;
        }

        public async Task<U> Post<T, U>(string requestUri, T t)
            where T : class
            where U : class
        {
            HttpResponseMessage response = await this.PostAsJsonAsync(requestUri, t);

            // Throw exception if HTTP Status code is not Success (2xx)
            response.EnsureSuccessStatusCode();
            var resultData = response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<U>();
        }

        public async Task<bool> Delete(string requestUri)
        {
            HttpResponseMessage response = await base.DeleteAsync(requestUri);

            //// Throw exception if HTTP Status code is not Success (2xx)
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        
        public async Task<U> Put<T, U>(string requestUri, T t)
            where T : class
            where U : class
        {
            HttpResponseMessage response = await this.PutAsJsonAsync(requestUri, t);

            // Throw exception if HTTP Status code is not Success (2xx)
            response.EnsureSuccessStatusCode();
            var resultData = response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<U>();
        }

        public async Task<U> Patch<T, U>(string requestUri, T t)
            where T : class
            where U : class
        {
            HttpRequestMessage httpRequestMessage =
                new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(t),
                        Encoding.UTF8, "application/json")
                };
            HttpResponseMessage response = await this.SendAsync(httpRequestMessage);
            // Throw exception if HTTP Status code is not Success (2xx)
            response.EnsureSuccessStatusCode();
            var resultData = response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<U>();
        }
    }
}
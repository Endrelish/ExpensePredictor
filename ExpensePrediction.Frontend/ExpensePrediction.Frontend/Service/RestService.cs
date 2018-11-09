using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.Frontend.Service
{
    public class RestService
    {
        private const string ApplicationJsonContentType = "application/json";
        private readonly HttpClient client;
        public RestService()
        {
            client = new HttpClient();
        }

        private HttpContent CreateContent(object dto)
        {
            return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, ApplicationJsonContentType);
        }

        private async Task<T> GetContent<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> PostAsync<T>(string uri, object content)
        {
            var response = await client.PostAsync(uri, CreateContent(content));

            return await GetContent<T>(response);
        }

    }
}

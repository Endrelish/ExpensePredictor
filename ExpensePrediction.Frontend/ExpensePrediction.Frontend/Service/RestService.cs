using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ExpensePrediction.Frontend.Service
{
    public static class RestService
    {
        private const string ApplicationJsonContentType = "application/json";
        private static readonly HttpClient _client;

        static RestService()
        {
            _client = new HttpClient();
        }
        
        private static async Task<T> GetContentAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        private static async Task<HttpRequestMessage> GetRequestAsync(string uri, bool authorize, HttpMethod method, object dto = null)
        {
            try
            {
                var request = new HttpRequestMessage(method, uri);
                if (authorize)
                {
                    var token = await SecureStorage.GetAsync(Constants.Token);
                    token = "Bearer " + token;
                    request.Headers.Add("Authorization", token);
                }
                if(method != HttpMethod.Get)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, ApplicationJsonContentType);
                }

                return request;
            }
            catch (Exception)
            {
                //TODO sth
                throw;
            }
        }

        public static async Task<T> PostAsync<T>(string uri, object content, bool authorize = true)
        {
            var response = await _client.SendAsync(await GetRequestAsync(uri, authorize, HttpMethod.Post, content));

            if (!response.IsSuccessStatusCode)
            {
                //NOOOOOOOOOOOOOOOOOOOOOOOOOO
                //TODO sth
            }

            return await GetContentAsync<T>(response);
        }

        public static async Task<T> GetAsync<T>(string uri, bool authorize = true)
        {
            var response = await _client.SendAsync(await GetRequestAsync(uri, authorize, HttpMethod.Get));

            if (!response.IsSuccessStatusCode)
            {
                //NOOOOOOOOOOOOOOOOOOOOOOOOOO
                //TODO sth
            }

            var content = await GetContentAsync<T>(response);
            return content;
        }
    }
}
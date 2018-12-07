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

        private static async Task<HttpContent> CreateContent(object dto, bool authorize)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8,
                ApplicationJsonContentType);

            var headers = content.Headers;
            if (authorize)
            {
                try
                {
                    var token = await SecureStorage.GetAsync(Constants.Token);
                    token = "Bearer " + token;
                    content.Headers.Add("Authorization", token);
                }
                catch (Exception)
                {
                    //TODO don't know what
                }
            }

            return content;
        }

        private static async Task<T> GetContent<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private static async Task<HttpRequestMessage> GetGetRequest(string uri, bool authorize)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                if (authorize)
                {
                    var token = await SecureStorage.GetAsync(Constants.Token);
                    token = "Bearer " + token;
                    request.Headers.Add("Authorization", token);
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
            var response = await _client.PostAsync(uri, await CreateContent(content, authorize));

            if (!response.IsSuccessStatusCode)
            {
                //NOOOOOOOOOOOOOOOOOOOOOOOOOO
                //TODO sth
            }

            return await GetContent<T>(response);
        }

        public static async Task<T> GetAsync<T>(string uri, bool authorize = true)
        {
            var response = await _client.SendAsync(await GetGetRequest(uri, authorize));

            if (!response.IsSuccessStatusCode)
            {
                //NOOOOOOOOOOOOOOOOOOOOOOOOOO
                //TODO sth
            }

            return await GetContent<T>(response);
        }
    }
}
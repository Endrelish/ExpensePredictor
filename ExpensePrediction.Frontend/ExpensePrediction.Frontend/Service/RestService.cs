using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.Exceptions;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public static async Task<T> PostAsync<T>(string uri, object content, bool authorize = true)
        {
            var response = await _client.SendAsync(await GetRequestAsync(uri, authorize, HttpMethod.Post, content));

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    void Navigate()
                    {
                        Application.Current.MainPage = new NavigationPage(new MainPage());
                    }
                    AuthService.Logout(Navigate);
                    await Application.Current.MainPage.DisplayAlert("Logout", "You session expired", "");
                }
                var error = await GetContentAsync<ErrorDto>(response);
                throw new RestException(error.Message, error.ErrorCode);
            }

            return await GetContentAsync<T>(response);
        }

        public static async Task<T> GetAsync<T>(string uri, bool authorize = true)
        {
            var response = await _client.SendAsync(await GetRequestAsync(uri, authorize, HttpMethod.Get));

            if (!response.IsSuccessStatusCode)
            {
                var error = await GetContentAsync<ErrorDto>(response);
                throw new RestException(error.Message, error.ErrorCode);
            }

            var content = await GetContentAsync<T>(response);
            return content;
        }
    }
}
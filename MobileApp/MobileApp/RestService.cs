using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using MobileApp.ApiJsonResponse;

using Newtonsoft.Json;

namespace MobileApp
{
    internal class RestService
    {
        private readonly HttpClient _httpClient;

        public RestService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<LoginResponse> GetLoginResponseAsync(string url)
        {
            LoginResponse loginResponse = null;

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(content);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("\tERROR {0}", exception.Message);
            }

            return loginResponse;
        }
    }
}

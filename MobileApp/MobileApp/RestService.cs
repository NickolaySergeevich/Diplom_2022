using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MobileApp.ApiJsonRequest;
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

        public async Task<RegistrationResponse> GetRegistrationResponseAsync(string url,
            RegistrationRequest requestData)
        {
            RegistrationResponse registrationResponse = null;

            var postData = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, postData);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    registrationResponse = JsonConvert.DeserializeObject<RegistrationResponse>(content);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("\tERROR {0}", exception.Message);
            }

            return registrationResponse;
        }
    }
}

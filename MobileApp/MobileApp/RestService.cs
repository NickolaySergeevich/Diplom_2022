using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<T> GetResponseAsync<T>(string url)
        {
            var answer = default(T);

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    answer = JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("\tERROR {0}", exception.Message);
            }

            return answer;
        }

        public async Task<T> GetResponseWithBody<T, TB>(string url, TB requestData)
        {
            var answer = default(T);

            var postData = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8,
                "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, postData);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    answer = JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("\tERROR {0}", exception.Message);
            }

            return answer;
        }
    }
}

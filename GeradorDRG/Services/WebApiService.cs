using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GeradorDRG.Services
{
    public static class WebApiService
    {
        public static async Task<T> GetResponseAsync<T>(string URLBase, string Url) where T : new()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLBase);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(
                                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                string.Format("{0}:{1}", "user", "password"))));

                HttpResponseMessage response = await client.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;

                    string result = await content.ReadAsStringAsync();

                    T l = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);

                    return l;
                }

            }
            return new T();
        }
    }
}

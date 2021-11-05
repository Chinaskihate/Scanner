using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI
{
    public class ManagerAPIHttpClient : HttpClient
    {
        public ManagerAPIHttpClient()
        {
            BaseAddress = new Uri("https://localhost:44379/");
        }

        public async Task<T> GetAsync<T>(string url)
        {
            HttpResponseMessage response = await GetAsync(url);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}

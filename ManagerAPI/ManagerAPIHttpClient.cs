using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI
{
    /// <summary>
    /// Custom http client for scan manager api.
    /// </summary>
    public class ManagerAPIHttpClient : HttpClient
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ManagerAPIHttpClient()
        {
            BaseAddress = new Uri("https://localhost:5001/");
        }

        /// <summary>
        /// Get response.
        /// </summary>
        /// <typeparam name="T"> Type of response. </typeparam>
        /// <param name="url"> Query. </param>
        /// <returns> Response content. </returns>
        public async Task<T> GetAsync<T>(string url)
        {
            HttpResponseMessage response = await GetAsync(url);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch
            {
                throw new ArgumentException(jsonResponse);
            }
        }
    }
}

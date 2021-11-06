using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI
{
    /// <summary>
    /// Factory for DI.
    /// </summary>
    public class ManagerAPIHttpClientFactory
    {
        /// <summary>
        /// Creates http client.
        /// </summary>
        /// <returns> Custom http client. </returns>
        public ManagerAPIHttpClient CreateHttpClient()
        {
            return new ManagerAPIHttpClient();
        }
    }
}

using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI.Services
{
    /// <summary>
    /// Service for scanning.
    /// </summary>
    public class ScanService
    {
        private readonly ManagerAPIHttpClientFactory _managerAPIHttpClientFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="managerAPIHttpClientFactory"> Factory. </param>
        public ScanService(ManagerAPIHttpClientFactory managerAPIHttpClientFactory)
        {
            _managerAPIHttpClientFactory = managerAPIHttpClientFactory;
        }

        /// <summary>
        /// Create scan.
        /// </summary>
        /// <param name="path"> Path to directory. </param>
        /// <returns> Id of created scan. </returns>
        public async Task<int> CreateScan(string path)
        {
            using (ManagerAPIHttpClient client = _managerAPIHttpClientFactory.CreateHttpClient())
            {
                string queryUrl = $"create-scan?path={System.Web.HttpUtility.UrlEncode(path)}";

                int id = await client.GetAsync<int>(queryUrl);
                return id;
            }
        }

        /// <summary>
        /// Get scan status.
        /// </summary>
        /// <param name="id"> Id of scan. </param>
        /// <returns> Scan status. </returns>
        public async Task<ScanStatus> GetScanStatusById(int id)
        {
            using (ManagerAPIHttpClient client = _managerAPIHttpClientFactory.CreateHttpClient())
            {
                string queryUrl = $"get-scan-status?id={id}";

                ScanStatus status = await client.GetAsync<ScanStatus>(queryUrl);
                return status;
            }

        }
    }
}

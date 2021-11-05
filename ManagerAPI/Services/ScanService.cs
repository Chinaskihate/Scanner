using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI.Services
{
    public class ScanService
    {
        private readonly ManagerAPIHttpClientFactory _managerAPIHttpClientFactory;

        public ScanService(ManagerAPIHttpClientFactory managerAPIHttpClientFactory)
        {
            _managerAPIHttpClientFactory = managerAPIHttpClientFactory;
        }

        public async Task<int> CreateTask(string path)
        {
            using (ManagerAPIHttpClient client = _managerAPIHttpClientFactory.CreateHttpClient())
            {
                string queryUrl = $"create-scan?path={System.Web.HttpUtility.UrlEncode(path)}";

                int id = await client.GetAsync<int>(queryUrl);
                return id;
            }
        }

        public async Task<ScanStatus> GetTaskById(int id)
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

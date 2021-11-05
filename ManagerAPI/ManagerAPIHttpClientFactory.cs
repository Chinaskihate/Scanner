using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI
{
    public class ManagerAPIHttpClientFactory
    {
        public ManagerAPIHttpClient CreateHttpClient()
        {
            return new ManagerAPIHttpClient();
        }
    }
}

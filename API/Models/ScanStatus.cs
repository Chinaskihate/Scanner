using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    /// <summary>
    /// Scan status.
    /// </summary>
    public class ScanStatus
    {
        /// <summary>
        /// Is scan completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Report of scan status.
        /// </summary>
        public string Report { get; set; }
    }
}

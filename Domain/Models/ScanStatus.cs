using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
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

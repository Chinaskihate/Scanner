using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    /// <summary>
    /// Result of scan.
    /// </summary>
    public class ScanResult
    {
        private static object _locker = new object();
        private int _totalProcessedFiles = 0;


        /// <summary>
        /// Total files processed.
        /// </summary>
        public int TotalProcessedFiles
        {
            get => _totalProcessedFiles; 
            set
            {
                lock (_locker)
                {
                    _totalProcessedFiles = value;
                }
            }
        }

        /// <summary>
        /// Total evil javascripts detects.
        /// </summary>
        public int TotalEvilJSDetects { get; set; }

        /// <summary>
        /// Total rm -rf detects.
        /// </summary>
        public int TotalRMDetects { get; set; }

        /// <summary>
        /// Total Rundll32 sus.dll SusEntry detects.
        /// </summary>
        public int TotalRunDLLDetects { get; set; }

        /// <summary>
        /// Total errors.
        /// </summary>
        public int TotalErrors { get; set; }

        /// <summary>
        /// Error messages.
        /// </summary>
        public List<string> ErrorMessages { get; set; } = new List<string>();

        /// <summary>
        /// Scan execution time.
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Concats two scan results.
        /// </summary>
        /// <param name="first"> First scan result. </param>
        /// <param name="second"> Second scar result. </param>
        /// <returns></returns>
        public static ScanResult operator +(ScanResult first, ScanResult second)
        {
            lock (_locker)
            {

                ScanResult res = new ScanResult()
                {
                    TotalProcessedFiles = first.TotalProcessedFiles + second.TotalProcessedFiles,
                    TotalEvilJSDetects = first.TotalEvilJSDetects + second.TotalEvilJSDetects,
                    TotalRMDetects = first.TotalRMDetects + second.TotalRMDetects,
                    TotalRunDLLDetects = first.TotalRunDLLDetects + second.TotalRunDLLDetects,
                    TotalErrors = first.TotalErrors + second.TotalErrors,
                    ErrorMessages = first.ErrorMessages.Concat(second.ErrorMessages).ToList(),
                    ExecutionTime = first.ExecutionTime + second.ExecutionTime
                };

                return res;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("====== SCAN RESULT ======");
            sb.AppendLine($"Processed files: {TotalProcessedFiles}");
            sb.AppendLine($"JS detects: {TotalEvilJSDetects}");
            sb.AppendLine($"rm -rf detects: {TotalRMDetects}");
            sb.AppendLine($"Rundll32 detects: {TotalRunDLLDetects}");
            sb.AppendLine($"Errors: {TotalErrors}");
            sb.AppendLine($"Error messages:");
            ErrorMessages.ForEach(m => sb.AppendLine($"  {m}"));
            sb.AppendLine($"Execution time: {ExecutionTime}");

            return sb.ToString();
        }
    }
}

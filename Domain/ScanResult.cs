using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class ScanResult
    {
        private static object _locker = new object();
        private int _totalProcessedFiles = 0;

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

        //TODO: think about prop names
        public int TotalJSDetects { get; set; }

        public int TotalRMDetects { get; set; }

        public int TotalRunDLLDetects { get; set; }

        public int TotalErrors { get; set; }

        public List<string> ErrorMessages { get; set; } = new List<string>();

        public TimeSpan ExecutionTime { get; set; }

        public static ScanResult operator +(ScanResult first, ScanResult second)
        {
            lock (_locker)
            {

                ScanResult res = new ScanResult()
                {
                    TotalProcessedFiles = first.TotalProcessedFiles + second.TotalProcessedFiles,
                    TotalJSDetects = first.TotalJSDetects + second.TotalJSDetects,
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
            sb.AppendLine($"JS detects: {TotalJSDetects}");
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

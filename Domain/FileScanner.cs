using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Domain
{
    public class FileScanner
    {
        private static object _locker = new object();

        /// <summary>
        /// Current scanning file.
        /// </summary>
        public string CurrentFile { get; private set; }

        /// <summary>
        /// Scans directory asynchronously.
        /// </summary>
        /// <param name="path"> Path to directory. </param>
        /// <returns> Scan result(task). </returns>
        public async Task<ScanResult> ScanAsync(string path)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ScanResult res = new ScanResult();
            CurrentFile = path;
            await Task.Run(() => res = ScanDirectory(path));
            watch.Stop();
            res.ExecutionTime = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            return res;
        }

        /// <summary>
        /// Scans directory.
        /// </summary>
        /// <param name="path"> Path to directory. </param>
        /// <returns> Scan result. </returns>
        private ScanResult ScanDirectory(string path)
        {
            ScanResult res = new ScanResult();
            CurrentFile = path;
            try
            {
                string[] files = Directory.GetFiles(path);
                res += ScanFiles(files);
                string[] subdirectories = Directory.GetDirectories(path);
                Parallel.ForEach(subdirectories, subdirectory =>
                {
                    var directoryResult = ScanDirectory(subdirectory);
                    lock (_locker)
                    {
                        res += directoryResult;
                    };
                });
            }
            catch (Exception ex)
            {
                res.TotalErrors++;
                res.ErrorMessages.Add(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Scans files.
        /// </summary>
        /// <param name="files"> Paths to files. </param>
        /// <returns> Scan result. </returns>
        private ScanResult ScanFiles(string[] files)
        {
            ScanResult res = new ScanResult();
            res.TotalProcessedFiles += files.Length;
            Parallel.ForEach(files, file =>
            {
                try
                {
                    CurrentFile = file;
                    bool isJS = Path.GetExtension(file) == ".js";
                    foreach (var line in File.ReadLines(file))
                    {
                        ProcessMalwareType(res, AnalyzeLine(line, isJS));
                    }
                }
                catch (Exception ex)
                {
                    res.TotalErrors++;
                    res.ErrorMessages.Add(ex.Message);
                }
            });

            return res;
        }

        /// <summary>
        /// Processes malware type, change scan result if needed.
        /// </summary>
        /// <param name="res"> Scan result. </param>
        /// <param name="malwareType"> Malware type. </param>
        private void ProcessMalwareType(ScanResult res, MalwareType malwareType)
        {
            lock (_locker)
            {
                switch (malwareType)
                {
                    case MalwareType.EvilJS:
                        res.TotalEvilJSDetects++;
                        break;
                    case MalwareType.Remover:
                        res.TotalRMDetects++;
                        break;
                    case MalwareType.DLLRunner:
                        res.TotalRunDLLDetects++;
                        break;
                }
            }
        }

        /// <summary>
        /// Analyze line for suspicious strings. 
        /// </summary>
        /// <param name="line"> Line. </param>
        /// <param name="isJS"> If file is javascript. </param>
        /// <returns> Malware type. </returns>
        private MalwareType AnalyzeLine(string line, bool isJS)
        {
            if (isJS)
            {
                if (IsEvilJS(line))
                {
                    return MalwareType.EvilJS;
                }
            }

            if (IsRemover(line))
            {
                return MalwareType.Remover;
            }

            if (IsDLLRunner(line))
            {
                return MalwareType.DLLRunner;
            }

            return MalwareType.SafeFile;
        }

        /// <summary>
        /// Check line for evil javascript.
        /// </summary>
        /// <param name="line"> Line. </param>
        /// <returns></returns>
        private bool IsEvilJS(string line)
        {
            return line.Contains("<script>evil_script()</script>");
        }

        /// <summary>
        /// Check line for rm -rf commands.
        /// </summary>
        /// <param name="line"> Line. </param>
        /// <returns></returns>
        private bool IsRemover(string line)
        {
            // TODO: should it be same path at remove command as current directory
            return line.Contains("rm -rf ");
        }

        /// <summary>
        /// Check line for  Rundll32 sus.dll SusEntry commands.
        /// </summary>
        /// <param name="line"> Line. </param>
        /// <returns></returns>
        private bool IsDLLRunner(string line)
        {
            return line.Contains("Rundll32 sus.dll SusEntry");
        }
    }
}

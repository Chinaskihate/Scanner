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

        public string CurrentFile { get; private set; }

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

        public ScanResult Scan(string path)
        {;
            ScanResult res = ScanDirectory(path);
            
            return res;
        }

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

        private void ProcessMalwareType(ScanResult res, MalwareType malwareType)
        {
            lock (_locker)
            {
                switch (malwareType)
                {
                    case MalwareType.EvilJS:
                        res.TotalJSDetects++;
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

        private bool IsEvilJS(string line)
        {
            return line.Contains("<script>evil_script()</script>");
        }

        private bool IsRemover(string line)
        {
            // TODO: should it be same path at remove command as current directory
            return line.Contains("rm -rf ");
        }

        private bool IsDLLRunner(string line)
        {
            return line.Contains("Rundll32 sus.dll SusEntry");
        }
    }
}

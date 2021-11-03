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
        public ScanResult Scan(string path)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ScanResult res = ScanDirectory(path);
            watch.Stop();
            res.ExecutionTime = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            return res;
        }

        private ScanResult ScanDirectory(string path)
        {
            ScanResult res = new ScanResult();
            try
            {
                string[] files = Directory.GetFiles(path);
                res += ScanFiles(files);
                string[] subdirectories = Directory.GetDirectories(path);
                foreach(var subdirectory in subdirectories)
                {
                    res += ScanDirectory(subdirectory);
                }
            }
            catch(Exception ex)
            {
                res.TotalErrors++;
                res.ErrorMessages.Add(ex.Message);
            }

            return res;
        }

        private ScanResult ScanFiles(string[] files)
        {
            ScanResult res = new ScanResult();
            foreach (var file in files)
            {
                try
                {
                    bool isJS = Path.GetExtension(file) == ".js";
                    // TODO: is it effective and good(or better to use tasks?)
                    res.TotalProcessedFiles++;
                    Parallel.ForEach(File.ReadLines(file), line =>
                    {
                        ProcessMalwareType(res, AnalyzeLine(line, isJS));
                    });
                }
                catch (Exception ex)
                {
                    res.TotalErrors++;
                    res.ErrorMessages.Add(ex.Message);
                }
            }

            return res;
        }

        private void ProcessMalwareType(ScanResult res, MalwareType malwareType)
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

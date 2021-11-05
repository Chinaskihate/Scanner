using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Domain.Models
{
    public class ScannersManager
    {
        private ConcurrentDictionary<int, Task<ScanResult>> _tasks;

        public ScannersManager()
        {
            _tasks = new ConcurrentDictionary<int, Task<ScanResult>>();
        }

        public int CreateScan(string path)
        {
            int id = _tasks.Keys.Max() + 1;
            FileScanner fs = new FileScanner();
            if (_tasks.TryAdd(id, fs.ScanAsync(path)))
            {
                return id;
            };

            return -1;
        }

        public string GetStatus(int id)
        {
            if (!_tasks.ContainsKey(id))
            {
                return $"Task {id} doesn't exist";
            }

            Task<ScanResult> currTask = _tasks[id];
            if (!currTask.IsCompleted)
            {
                return $"Scan task {id} in progress, please wait";
            }

            return currTask.Result.ToString();
        }
    }
}

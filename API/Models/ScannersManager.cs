using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Domain.Models;

namespace API.Models
{
    /// <summary>
    /// Handles different scans.
    /// </summary>
    public class ScannersManager
    {
        private ConcurrentDictionary<int, Task<ScanResult>> _tasks;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScannersManager()
        {
            _tasks = new ConcurrentDictionary<int, Task<ScanResult>>();
        }

        /// <summary>
        /// Create scan.
        /// </summary>
        /// <param name="path"> Path to directory. </param>
        /// <returns> Id of scan, if something goes wrong, returns -1. </returns>
        public int CreateScan(string path)
        {
            int id = _tasks.Count;
            FileScanner fs = new FileScanner();
            if (_tasks.TryAdd(id, fs.ScanAsync(path)))
            {
                return id;
            };

            return -1;
        }

        /// <summary>
        /// Get status of scan task.
        /// </summary>
        /// <param name="id"> Id of scan. </param>
        /// <returns> Scan status. </returns>
        public ScanStatus GetStatus(int id)
        {
            if (!_tasks.ContainsKey(id))
            {
                throw new ArgumentException($"Task {id} doesn't exist.");
            }

            Task<ScanResult> currTask = _tasks[id];

            return new ScanStatus()
            {
                IsCompleted = currTask.IsCompleted,
                Report = currTask.IsCompleted ? currTask.Result.ToString() : $"Scan task {id} in progress, please wait"
            };
        }
    }
}

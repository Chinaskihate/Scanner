using Domain;
using System;
using System.Threading;

namespace CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Incorrect command! Use: cmd <path to directory>");
                return;
            }

            string path = args[0];
            
            FileScanner fs = new FileScanner();
            var res = fs.ScanAsync(path);
            string scanMessage = string.Empty;
            int currentLineCursor = Console.CursorTop;
            while (!res.IsCompleted)
            {
                scanMessage = $"Scanning {fs.CurrentFile}";
                Console.WriteLine(scanMessage);
                Thread.Sleep(1000);
                ClearLastConsoleSymbols(scanMessage.Length, currentLineCursor);
            }
            Console.WriteLine();
            Console.WriteLine(res.Result);
            Console.ReadKey();
        }

        /// <summary>
        /// Clear last console symbols.
        /// </summary>
        /// <param name="len"> Number of symbols. </param>
        /// <param name="currentLineCursor"> Remove to this line. </param>
        public static void ClearLastConsoleSymbols(int len, int currentLineCursor)
        {
            Console.SetCursorPosition(0, currentLineCursor);
            for (int i = 0; i < len; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}

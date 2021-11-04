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
            while (!res.IsCompleted)
            {
                scanMessage = $"Scanning {fs.CurrentFile}";
                Console.WriteLine(scanMessage);
                Thread.Sleep(1000);
                ClearCurrentConsoleLine(scanMessage.Length);
            }
            Console.WriteLine();
            Console.WriteLine(res.Result);
            Console.ReadKey();
        }

        public static void ClearCurrentConsoleLine(int len)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, 1);
            for (int i = 0; i < len; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, 1);
        }
    }
}

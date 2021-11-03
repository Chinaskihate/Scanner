using Domain;
using System;

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

            FileScanner fs = new FileScanner();
            var res = fs.Scan(args[0]);
            while
        }
    }
}

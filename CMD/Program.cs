using Domain.Models;
using ManagerAPI.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMD
{
    enum CommandType
    {
        CreateScan,
        GetScanStatus,
        Incorrect
    }

    class Program
    {
        private static ScanService _scanService = new ScanService(new ManagerAPI.ManagerAPIHttpClientFactory());

        static async Task Main(string[] args)
        {
            if (args.Length == 2)
            {
                switch (ParseCommandType(args[0]))
                {
                    case CommandType.CreateScan:
                        Console.WriteLine(await ProcessCreateScanCommand(args[1]));;
                        break;
                    case CommandType.GetScanStatus:
                        Console.WriteLine(await GetScanStatus(args[1]));
                        break;
                }
            }
            else
            {
                PrintIncorrectCommandMessage();
            }
        }

        private static async Task<string> GetScanStatus(string str)
        {
            int id;
            if (int.TryParse(str, out id) && id >= 0)
            {
                ScanStatus status = await _scanService.GetTaskById(id);
                return status.Report;
            }
            else
            {
                return "Scan id have to be integer at least 0.";
            }
        }

        static void PrintIncorrectCommandMessage()
        {
            Console.WriteLine($"Incorrect command!{Environment.NewLine}" +
                    $"Use: scan <path to directory>{Environment.NewLine}" +
                    $"Or: status <scan id>");
        }

        static async Task<string> ProcessCreateScanCommand(string path)
        {
            int id = await CreateScan(path);
            return $"Scan task was created with ID: {id}";
        }

        static async Task<int> CreateScan(string path)
        {
            int id = await _scanService.CreateScan(path);
            return id;
        }

        static CommandType ParseCommandType(string str)
        {
            if (str.ToLower() == "scan")
            {
                return CommandType.CreateScan;
            }

            if (str.ToLower() == "status")
            {
                return CommandType.GetScanStatus;
            }

            return CommandType.Incorrect;
        }
    }
}

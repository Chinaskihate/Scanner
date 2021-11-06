using Domain.Models;
using ManagerAPI.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMD
{
    /// <summary>
    /// Type of command.
    /// </summary>
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
                        Console.WriteLine(await ProcessCreateScanCommand(args[1])); ;
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

            Console.ReadLine();
        }

        /// <summary>
        /// Get scan status.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static async Task<string> GetScanStatus(string str)
        {
            int id;
            try
            {
                if (int.TryParse(str, out id) && id >= 0)
                {
                    ScanStatus status = await _scanService.GetScanStatusById(id);
                    return status.Report;
                }
                else
                {
                    return "Scan id have to be integer at least 0.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Print incorrect command message.
        /// </summary>
        static void PrintIncorrectCommandMessage()
        {
            Console.WriteLine($"Incorrect command!{Environment.NewLine}" +
                    $"Use: scan <path to directory>{Environment.NewLine}" +
                    $"Or: status <scan id>");
        }

        /// <summary>
        /// Process create scan command.
        /// </summary>
        /// <param name="path"> Path to directory. </param>
        /// <returns> Message. </returns>
        static async Task<string> ProcessCreateScanCommand(string path)
        {
            try
            {
                int id = await CreateScan(path);
                return $"Scan was created with ID: {id}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Create scan.
        /// </summary>
        /// <param name="path"> Path to directory. </param>
        /// <returns> Scan id. </returns>
        static async Task<int> CreateScan(string path)
        {
            int id = await _scanService.CreateScan(path);
            return id;
        }

        /// <summary>
        /// Parse command type.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <returns> Command type. </returns>
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

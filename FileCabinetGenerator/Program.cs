using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Interacts with API.
    /// </summary>
    public static class Program
    {
        private static readonly Tuple<string, Action<string>>[] ProgramSettings = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("--output-type", SetOutputType),
            new Tuple<string, Action<string>>("-t", SetOutputType),
            new Tuple<string, Action<string>>("--output", SetOutput),
            new Tuple<string, Action<string>>("-o", SetOutput),
            new Tuple<string, Action<string>>("--record-amount", SetRecordAmount),
            new Tuple<string, Action<string>>("-a", SetRecordAmount),
            new Tuple<string, Action<string>>("--start-id", SetStartId),
            new Tuple<string, Action<string>>("-i", SetStartId),
            new Tuple<string, Action<string>>("--storage", SetTypeFileCabinetService),
            new Tuple<string, Action<string>>("-s", SetTypeFileCabinetService),
        };

        private static Tuple<string, Type>[] fileCabinetServices = new Tuple<string, Type>[]
        {
            new Tuple<string, Type>("memory", typeof(FileCabinetMemoryService)),
            new Tuple<string, Type>("file", typeof(FileCabinetFilesystemService)),
        };

        private static Tuple<string, Action<StreamWriter, FileCabinetServiceSnapshot>>[] imports = new Tuple<string, Action<StreamWriter, FileCabinetServiceSnapshot>>[]
        {
            new Tuple<string, Action<StreamWriter, FileCabinetServiceSnapshot>>("csv", ExportToCsv),
            new Tuple<string, Action<StreamWriter, FileCabinetServiceSnapshot>>("xml", ExportToXml),
        };

        private static Settings settings = new Settings();
        private static IFileCabinetService fileCabinetService;
        private static Action<StreamWriter, FileCabinetServiceSnapshot> exportMethod = ExportToCsv;

        /// <summary>
        /// Point of entry.
        /// </summary>
        /// <param name="args">Property.</param>
        public static void Main(string[] args)
        {
            if (args != null)
            {
                SetSettings(args);
            }
        }

        /// <summary>
        /// Set settings.
        /// </summary>
        /// <param name="args">property of settings.</param>
        private static void SetSettings(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string command = string.Empty;
                string parameter = string.Empty;
                if (args[i].StartsWith("--", StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] split = args[i].Split('=', 2);
                    if (split.Length == 2)
                    {
                        command = split[0];
                        parameter = split[1];
                    }
                }
                else if (args[i].StartsWith('-'))
                {
                    command = args[i];
                    if (i + 1 < args.Length)
                    {
                        i++;
                        parameter = args[i];
                    }
                }
                else
                {
                    continue;
                }

                try
                {
                    ExcuteCommand(command, ProgramSettings)(parameter);
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }

            CreateFileCabinet();
            GenerateRecords(settings.RecordsAmount);
            Export(exportMethod);
        }

        /// <summary>
        /// Find and return command.
        /// </summary>
        /// <typeparam name="T">Type command.</typeparam>
        /// <param name="commandName">Command name.</param>
        /// <param name="commands">Array of command.</param>
        /// <returns>Command.</returns>
        private static T ExcuteCommand<T>(string commandName, Tuple<string, T>[] commands)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException($"{commandName}");
            }

            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                return commands[index].Item2;
            }
            else
            {
                PrintMissedCommandInfo(commandName);
                throw new ArgumentException("Command not found", $"{commandName}");
            }
        }

        /// <summary>
        /// Print that command missed.
        /// </summary>
        /// <param name="command">Missed command.</param>
        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        /// <summary>
        /// Sets the type to be saved to.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        private static void SetOutputType(string parameter)
        {
            exportMethod = ExcuteCommand(parameter, imports);
        }

        /// <summary>
        /// Sets the path where records should be saved.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        private static void SetOutput(string parameter)
        {
            settings.FilePath = parameter;
        }

        /// <summary>
        /// Sets the number of records to be created.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        private static void SetRecordAmount(string parameter)
        {
            int amount;
            _ = int.TryParse(parameter, out amount);
            settings.RecordsAmount = amount;
        }

        /// <summary>
        /// Sets the identifier from which to start generating data.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        private static void SetStartId(string parameter)
        {
            int id;
            _ = int.TryParse(parameter, out id);
            settings.StartId = id;
        }

        /// <summary>
        /// Set FileCabinet type.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        private static void SetTypeFileCabinetService(string parameter)
        {
            settings.FileCabinetType = ExcuteCommand(parameter, fileCabinetServices);
        }

        /// <summary>
        /// Generates records.
        /// </summary>
        /// <param name="amount">Number of records to create.</param>
        private static void GenerateRecords(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                fileCabinetService.CreateRecord(Generator.GenerateRecord(settings.Validator));
            }
        }

        /// <summary>
        /// Creates File Cabinet.
        /// </summary>
        private static void CreateFileCabinet()
        {
            IRecordValidator validator = new DefaultValidator();
            if (settings.FileCabinetType.Equals(typeof(FileCabinetMemoryService)))
            {
                fileCabinetService = new FileCabinetMemoryService(validator);
            }
            else if (settings.FileCabinetType.Equals(typeof(FileCabinetFilesystemService)))
            {
                FileStream fileStream = new FileStream(Settings.FileNameStorage, FileMode.Create, FileAccess.ReadWrite);
                fileCabinetService = new FileCabinetFilesystemService(validator, fileStream, settings.StartId);
            }
        }

        /// <summary>
        /// Print records.
        /// </summary>
        /// <param name="records">Records which need print.</param>
        private static void Print(IReadOnlyCollection<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}," +
                    $" age: {record.Age}, salary {record.Salary}, access {record.Access}");
            }
        }

        private static void Export(Action<StreamWriter, FileCabinetServiceSnapshot> exportMethod)
        {
            FileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();
            using (StreamWriter writer = new StreamWriter(settings.FilePath))
            {
                exportMethod(writer, snapshot);
            }

            Console.WriteLine($"{settings.RecordsAmount} records were written to {settings.FilePath}");
        }

        /// <summary>
        /// Saves data to csv file.
        /// </summary>
        /// <param name="writer">Provider for writing.</param>
        /// <param name="snapshot">The state to be saved.</param>
        private static void ExportToCsv(StreamWriter writer, FileCabinetServiceSnapshot snapshot)
        {
            snapshot.SaveToCsv(writer);
        }

        /// <summary>
        /// Saves data to xml file.
        /// </summary>
        /// <param name="writer">Provider for writing.</param>
        /// <param name="snapshot">The state to be saved.</param>
        private static void ExportToXml(StreamWriter writer, FileCabinetServiceSnapshot snapshot)
        {
            snapshot.SaveToXml(writer);
        }
    }
}

using System;
using System.IO;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Interacts with API.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Oleg Yagovdic";
        private const string NameFileStorage = "cabinet-records.db";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static readonly Tuple<string, Action<string>>[] ProgramSetting = new Tuple<string, Action<string>>[]
{
            new Tuple<string, Action<string>>("--validation-rules", SetTypeValidationRules),
            new Tuple<string, Action<string>>("-v", SetTypeValidationRules),
            new Tuple<string, Action<string>>("--storage", SetTypeFileCabinetService),
            new Tuple<string, Action<string>>("-s", SetTypeFileCabinetService),
            new Tuple<string, Action<string>>("-use-stopwatch", SetStopWatch),
            new Tuple<string, Action<string>>("-use-logger", SetLogger),
};

        private static readonly Tuple<string, IRecordValidator>[] Validators = new Tuple<string, IRecordValidator>[]
        {
            new Tuple<string, IRecordValidator>("default", new ValidatorBuilder().CreateDefault()),
            new Tuple<string, IRecordValidator>("custom", new ValidatorBuilder().CreateCustom()),
        };

        private static readonly Tuple<string, Type>[] FileCabinetServices = new Tuple<string, Type>[]
        {
            new Tuple<string, Type>("memory", typeof(FileCabinetMemoryService)),
            new Tuple<string, Type>("file", typeof(FileCabinetFilesystemService)),
        };

        private static IRecordValidator validator = new ValidatorBuilder().CreateDefault();
        private static Type service = typeof(FileCabinetMemoryService);
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(validator);
        private static bool useStopWatch;
        private static bool useLogger;
        private static bool isRunning = true;

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

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];
                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                var commandHendler = CreateCommandHandlers();

                commandHendler.Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);
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
                    if (i + 1 < args.Length && !command.Equals("-use-logger") && !command.Equals("-use-stopwatch"))
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
                    ExcuteCommand(command, ProgramSetting)(parameter);
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }

            CreateFileCabinetService();
        }

        /// <summary>
        /// Set type validation-rules.
        /// </summary>
        /// <param name="parameter">Validation parameter.</param>
        private static void SetTypeValidationRules(string parameter)
        {
            validator = ExcuteCommand(parameter, Validators);
        }

        /// <summary>
        /// Set type file cabinet service.
        /// </summary>
        /// <param name="parameter">Validation parameter.</param>
        private static void SetTypeFileCabinetService(string parameter)
        {
            service = ExcuteCommand(parameter, FileCabinetServices);
        }

        private static void SetStopWatch(string parameter)
        {
            useStopWatch = true;
        }

        private static void SetLogger(string parameter)
        {
            useLogger = true;
        }

        /// <summary>
        /// Creates file cabinet service.
        /// </summary>
        private static void CreateFileCabinetService()
        {
            if (service.Equals(typeof(FileCabinetMemoryService)))
            {
                fileCabinetService = new FileCabinetMemoryService(validator);
            }
            else if (service.Equals(typeof(FileCabinetFilesystemService)))
            {
                FileStream fileStream = new FileStream(NameFileStorage, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fileCabinetService = new FileCabinetFilesystemService(validator, fileStream);
            }

            if (useStopWatch)
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }

            if (useLogger)
            {
                fileCabinetService = new ServiceLogger(fileCabinetService);
            }
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var recordPrinte = new DefaultRecordPrinter();
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(ChangeStatus);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var insertHandler = new InsertCommandHandler(fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(fileCabinetService);
            var updateHandler = new UpdateCommandHandler(fileCabinetService);
            var selectHandler = new SelectCommandHandler(fileCabinetService, Console.WriteLine);

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(exitHandler);
            exitHandler.SetNext(exportHandler);
            exportHandler.SetNext(statHandler);
            statHandler.SetNext(insertHandler);
            insertHandler.SetNext(deleteHandler);
            deleteHandler.SetNext(updateHandler);
            updateHandler.SetNext(selectHandler);

            return helpHandler;
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

        private static void ChangeStatus(bool newStatus)
        {
            isRunning = newStatus;
        }
    }
}
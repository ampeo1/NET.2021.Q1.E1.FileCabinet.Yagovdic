using System;
using System.IO;
using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    /// <summary>
    /// Interacts with API.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// If true than programm works else program doesn't work.
        /// </summary>
        public static bool isRunning = true;

        private const string DeveloperName = "Oleg Yagovdic";
        private const string NameFileStorage = "cabinet-records.db";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private static Type validator = typeof(DefaultValidator);
        private static Type service = typeof(FileCabinetMemoryService);

        private static Tuple<string, Action<string>>[] programSetting = new Tuple<string, Action<string>>[]
       {
            new Tuple<string, Action<string>>("--validation-rules", SetTypeValidationRules),
            new Tuple<string, Action<string>>("-v", SetTypeValidationRules),
            new Tuple<string, Action<string>>("--storage", SetTypeFileCabinetService),
            new Tuple<string, Action<string>>("-s", SetTypeFileCabinetService),
       };

        private static Tuple<string, Type>[] validators = new Tuple<string, Type>[]
        {
            new Tuple<string, Type>("default", typeof(DefaultValidator)),
            new Tuple<string, Type>("custom", typeof(CustomValidator)),
        };

        private static Tuple<string, Type>[] fileCabinetServices = new Tuple<string, Type>[]
        {
            new Tuple<string, Type>("memory", typeof(FileCabinetMemoryService)),
            new Tuple<string, Type>("file", typeof(FileCabinetFilesystemService)),
        };

        /// <summary>
        /// File cabinet service.
        /// </summary>
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

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
                    ExcuteCommand(command, programSetting)(parameter);
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
            validator = ExcuteCommand(parameter, validators);
        }

        /// <summary>
        /// Set type file cabinet service.
        /// </summary>
        /// <param name="parameter">Validation parameter.</param>
        private static void SetTypeFileCabinetService(string parameter)
        {
            service = ExcuteCommand(parameter, fileCabinetServices);
        }

        /// <summary>
        /// Creates file cabinet service.
        /// </summary>
        private static void CreateFileCabinetService()
        {
            IRecordValidator recordValidator = (IRecordValidator)validator.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>());
            if (service.Equals(typeof(FileCabinetMemoryService)))
            {
                fileCabinetService = new FileCabinetMemoryService(recordValidator);
            }
            else if (service.Equals(typeof(FileCabinetFilesystemService)))
            {
                FileStream fileStream = new FileStream(NameFileStorage, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fileCabinetService = new FileCabinetFilesystemService(recordValidator, fileStream);
            }
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var editHandler = new EditCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler();
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(editHandler);
            editHandler.SetNext(removeHandler);
            removeHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(listHandler);
            listHandler.SetNext(exitHandler);
            exitHandler.SetNext(exportHandler);
            exportHandler.SetNext(findHandler);
            findHandler.SetNext(statHandler);

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
    }
}
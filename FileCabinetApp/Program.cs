using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Interacts with API. 
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Oleg Yagovdic";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService = new FileCabinetService(new DefaultValidator());

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static Tuple<string, Func<string, IReadOnlyCollection<FileCabinetRecord>>>[] findProperty = new Tuple<string, Func<string, IReadOnlyCollection<FileCabinetRecord>>>[]
        {
            new Tuple<string, Func<string, IReadOnlyCollection<FileCabinetRecord>>>("firstname", FindByFirstname),
            new Tuple<string, Func<string, IReadOnlyCollection<FileCabinetRecord>>>("lastname", FindByLastname),
            new Tuple<string, Func<string, IReadOnlyCollection<FileCabinetRecord>>>("dateofbirth", FindByBirthDay),
        };

        private static Tuple<string, string, Action<string>>[] programSetting = new Tuple<string, string, Action<string>>[]
        {
            new Tuple<string, string, Action<string>>("--validation-rules", "-v", SetFileCabinetService),
        };

        private static Tuple<string, IRecordValidator>[] fileCabinets = new Tuple<string, IRecordValidator>[]
        {
            new Tuple<string, IRecordValidator>("default", new DefaultValidator()),
            new Tuple<string, IRecordValidator>("custom", new CustomValidator()),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "prints the statistics of records", "The 'stat' command prints the statistics of records." },
            new string[] { "create", "creates new record", "The 'create' command creates new record" },
            new string[] { "create", "change record", "The 'edit' command changes record" },
            new string[] { "list", "lists records", "The 'lists' command lists records " },
            new string[] { "find", "finds records", "The 'find' command finds records " },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// Point of entry.
        /// </summary>
        /// <param name="args">Property.</param>
        public static void Main(string[] args)
        {
            SetSettings(args);
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        /// <summary>
        /// Set settings.
        /// </summary>
        /// <param name="args">property of settings.</param>
        private static void SetSettings(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            string command = args[0];
            int index = -1;
            if (command[0].Equals('-') && command[1].Equals('-'))
            {
                string[] splittingCommand = command.Split('=');
                if (splittingCommand.Length == 2)
                {
                    index = Array.FindIndex(programSetting, 0, programSetting.Length, i => i.Item1.Equals(splittingCommand[0], StringComparison.InvariantCultureIgnoreCase));
                    if (index >= 0)
                    {
                        programSetting[index].Item3(splittingCommand[1]);
                    }
                }

                return;
            }

            if (command[0].Equals('-'))
            {
                if (args.Length == 2)
                {
                    index = Array.FindIndex(programSetting, 0, programSetting.Length, i => i.Item2.Equals(args[0], StringComparison.InvariantCultureIgnoreCase));
                    if (index >= 0)
                    {
                        programSetting[index].Item3(args[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Set file cabinet service.
        /// </summary>
        /// <param name="parameter">Validation parameter.</param>
        private static void SetFileCabinetService(string parameter)
        {
            var index = Array.FindIndex(fileCabinets, 0, fileCabinets.Length, i => i.Item1.Equals(parameter, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                IRecordValidator validator = fileCabinets[index].Item2;
                fileCabinetService = new FileCabinetService(validator);
            }
            else
            {
                fileCabinetService = new FileCabinetService(new DefaultValidator());
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
        /// Print help information.
        /// </summary>
        /// <param name="parameters">Parameter wich need print help information.</param>
        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Prints statistics.
        /// </summary>
        /// <param name="parameters">Parameter.</param>
        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        /// <summary>
        /// Create new record.
        /// </summary>
        /// <param name="parameters">Parameter.</param>
        private static void Create(string parameters)
        {
            int id;

            DataRecord dataRecord = CollectRecordData();
            try
            {
                id = fileCabinetService.CreateRecord(dataRecord);
                Console.WriteLine($"Record #{id} is created.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Change record.
        /// </summary>
        /// <param name="parameters">Id record.</param>
        private static void Edit(string parameters)
        {
            int id = 0;
            int index;
            try
            {
                id = int.Parse(parameters, CultureInfo.InvariantCulture);
                index = fileCabinetService.FindIndexById(id);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            DataRecord dataRecord = CollectRecordData();
            dataRecord.Id = int.Parse(parameters, CultureInfo.InvariantCulture);

            try
            {
                fileCabinetService.EditRecord(dataRecord);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Gathers information about a record.
        /// </summary>
        /// <returns>Record data.</returns>
        private static DataRecord CollectRecordData()
        {
            DataRecord dataRecord = new DataRecord();
            IRecordValidator validator = fileCabinetService.GetValidator();
            Console.Write("First name: ");
            dataRecord.FirstName = ReadInput(Converter.StringConverter, validator.ValidateFirstName);

            Console.Write("Last Name: ");
            dataRecord.LastName = ReadInput(Converter.StringConverter, validator.ValidateLastName);

            Console.Write("Date of birth: ");
            dataRecord.DateOfBirth = ReadInput(Converter.DateConverter, validator.ValidateDateOfBirth);

            Console.Write("Access: ");
            dataRecord.Access = ReadInput(Converter.CharConverted, validator.ValidateAccess);

            return dataRecord;
        }

        /// <summary>
        /// Reads data from the console.
        /// </summary>
        /// <typeparam name="T">Type of object being read.</typeparam>
        /// <param name="converter">Function that performs convertation.</param>
        /// <param name="validator">Function that performs validation.</param>
        /// <returns>Processed data.</returns>
        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// Finds record.
        /// </summary>
        /// <param name="parameters">search property and search data.</param>
        private static void Find(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            const int commandIndex = 0;
            var command = inputs[commandIndex];
            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine($"Error. find [property]");
                return;
            }

            IReadOnlyCollection<FileCabinetRecord> records = Array.Empty<FileCabinetRecord>();
            var index = Array.FindIndex(findProperty, 0, findProperty.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));

            if (index >= 0)
            {
                const int parametersIndex = 1;
                var key = inputs.Length > 1 ? inputs[parametersIndex].Trim('\"') : string.Empty;
                records = findProperty[index].Item2(key);
            }

            Print(records);
        }

        /// <summary>
        /// Find record by first name.
        /// </summary>
        /// <param name="firstname">First name.</param>
        /// <returns>Found records.</returns>
        private static IReadOnlyCollection<FileCabinetRecord> FindByFirstname(string firstname)
        {
            return fileCabinetService.FindByFirstName(firstname);
        }

        /// <summary>
        /// Find record by last name.
        /// </summary>
        /// <param name="lastname">last name.</param>
        /// <returns>Found records.</returns>
        private static IReadOnlyCollection<FileCabinetRecord> FindByLastname(string lastname)
        {
            return fileCabinetService.FindByLastname(lastname);
        }

        /// <summary>
        /// Find record by date of birth.
        /// </summary>
        /// <param name="birthday">Date of birth.</param>
        /// <returns>Found records.</returns>
        private static IReadOnlyCollection<FileCabinetRecord> FindByBirthDay(string birthday)
        {
            if (!DateTime.TryParseExact(birthday, "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("Error. Incorrect format, must be yyyy-mmm-dd");
                return Array.Empty<FileCabinetRecord>();
            }

            return fileCabinetService.FindByBirthDay(date);
        }

        /// <summary>
        /// Print all records.
        /// </summary>
        /// <param name="parameters">Parameter.</param>
        private static void List(string parameters)
        {
            IReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.GetRecords();
            Print(records);
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
                    $" age: {record.Age}, amount records {record.AmountRecords}, access {record.Access}");
            }
        }

        /// <summary>
        /// Exit from programm.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}
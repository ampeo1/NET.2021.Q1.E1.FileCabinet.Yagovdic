using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Oleg Yagovdic";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

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

        private static Tuple<string, Func<string, FileCabinetRecord[]>>[] findProperty = new Tuple<string, Func<string, FileCabinetRecord[]>>[]
        {
            new Tuple<string, Func<string, FileCabinetRecord[]>>("firstname", FindByFirstname),
            new Tuple<string, Func<string, FileCabinetRecord[]>>("lastname", FindByLastname),
            new Tuple<string, Func<string, FileCabinetRecord[]>>("dateofbirth", FindByBirthDay),
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

        public static void Main(string[] args)
        {
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

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

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

        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            bool key = true;
            int id = 0;

            while (key)
            {
                Console.Write("First name: ");
                string firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Date of birth: ");
                DateTime dateTime;
                while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine("Error. Incorrect format, must be dd/mm/yyyy");
                    Console.Write("Date of birth: ");
                }

                char access;
                Console.Write("Access: ");
                while (!char.TryParse(Console.ReadLine(), out access))
                {
                    Console.WriteLine("Error. Input one letter.");
                    Console.Write("Access: ");
                }

                try
                {
                    id = fileCabinetService.CreateRecord(firstName, lastName, dateTime, access);
                    key = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    key = true;
                }
            }

            Console.WriteLine($"Record #{id} is created.");
        }

        private static void Edit(string parameters)
        {
            bool key = true;
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

            while (key)
            {
                Console.Write("First name: ");
                string firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Date of birth: ");
                DateTime dateTime;
                while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine("Error. Incorrect format, must be dd/mm/yyyy");
                    Console.Write("Date of birth: ");
                }

                char access;
                Console.Write("Access: ");
                while (!char.TryParse(Console.ReadLine(), out access))
                {
                    Console.WriteLine("Error. Input one letter.");
                    Console.Write("Access: ");
                }

                id = int.Parse(parameters, CultureInfo.InvariantCulture);

                try
                {
                    fileCabinetService.EditRecord(id, firstName, lastName, dateTime, access);
                    key = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    key = true;
                }
            }

            Console.WriteLine($"Record #{id} is updated.");
        }

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

            FileCabinetRecord[] records = Array.Empty<FileCabinetRecord>();
            var index = Array.FindIndex(findProperty, 0, findProperty.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));

            if (index >= 0)
            {
                const int parametersIndex = 1;
                var key = inputs.Length > 1 ? inputs[parametersIndex].Trim('\"') : string.Empty;
                records = findProperty[index].Item2(key);
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}," +
                $" age: {record.Age}, amount records {record.AmountRecords}, access {record.Access}");
            }
        }

        private static FileCabinetRecord[] FindByFirstname(string firstname)
        {
            return fileCabinetService.FindByFirstName(firstname);
        }

        private static FileCabinetRecord[] FindByLastname(string lastname)
        {
            return fileCabinetService.FindByLastname(lastname);
        }

        private static FileCabinetRecord[] FindByBirthDay(string birthday)
        {
            DateTime date;
            if (!DateTime.TryParseExact(birthday, "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                Console.WriteLine("Error. Incorrect format, must be yyyy-mmm-dd");
                return Array.Empty<FileCabinetRecord>();
            }

            return fileCabinetService.FindByBirthDay(date);
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}," +
                    $" age: {record.Age}, amount records {record.AmountRecords}, access {record.Access}");
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}
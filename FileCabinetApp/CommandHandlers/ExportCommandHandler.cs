using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public ExportCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        protected override string NameCommand => "export";

        /// <summary>
        /// Exports data in file.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            string[] splitParameters = command.Parameters.Split(' ');
            if (splitParameters.Length != 2)
            {
                return;
            }

            Action<StreamWriter, FileCabinetServiceSnapshot> executeCommand = splitParameters[0] switch
            {
                "csv" => ExportToCsv,
                "xml" => ExportToXml,
                _ => ExportToCsv
            };

            try
            {
                if (!CheckFile(splitParameters[1]))
                {
                    return;
                }

                FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
                using (StreamWriter writer = new StreamWriter(splitParameters[1], false))
                {
                    executeCommand(writer, snapshot);
                    Console.WriteLine($"All records are exported to file {splitParameters[1]}.");
                }
            }
            catch (Exception ex) when (
                ex is ArgumentException || ex is NotSupportedException || ex is FileNotFoundException || ex is IOException ||
                ex is System.Security.SecurityException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException ||
                ex is PathTooLongException)
            {
                Console.WriteLine($"Export failed: can't open file {splitParameters[1]}.");
            }
        }

        /// <summary>
        /// Checks file. If the file contains something, it will ask if it can be overwritten.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>True if verification was successful; otherwise false.</returns>
        private static bool CheckFile(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (fileStream.Length != 0)
                {
                    Console.WriteLine($"File is exist - rewrite {path}? [Y/n]");
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey();
                    }
                    while (!key.KeyChar.Equals('y') && !key.KeyChar.Equals('n'));
                    Console.WriteLine();

                    if (key.KeyChar.Equals('n'))
                    {
                        return false;
                    }
                }
            }

            return true;
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

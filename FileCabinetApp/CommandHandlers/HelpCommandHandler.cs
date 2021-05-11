using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Help command.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private readonly string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "prints the statistics of records", "The 'stat' command prints the statistics of records." },
            new string[] { "create", "creates new record", "The 'create' command creates new record" },
            new string[] { "update", "change record", "The 'update' command changes record" },
            new string[] { "delete", "removes record", "The 'delete' command deletes record" },
            new string[] { "purge", "defragments a file", "The 'purge' command defragments a file" },
            new string[] { "list", "lists records", "The 'lists' command lists records" },
            new string[] { "find", "finds records", "The 'find' command finds records" },
            new string[] { "export", "exports records", "The 'export' command saves records" },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <inheritdoc/>
        protected override string NameCommand => "help";

        /// <summary>
        /// Print help information.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            int commandHelpIndex = 0;
            int descriptionHelpIndex = 1;
            int explanationHelpIndex = 2;

            if (!string.IsNullOrEmpty(command.Parameters))
            {
                var index = Array.FindIndex(this.helpMessages, 0, this.helpMessages.Length, i => string.Equals(i[commandHelpIndex], command.Parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(this.helpMessages[index][explanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{command.Parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in this.helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[commandHelpIndex], helpMessage[descriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}

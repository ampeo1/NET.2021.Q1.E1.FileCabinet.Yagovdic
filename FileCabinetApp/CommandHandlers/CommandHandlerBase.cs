using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base class for Command.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        /// <summary>
        /// Gets name current command.
        /// </summary>
        /// <value>
        /// Name current command.
        /// </value>
        protected abstract string NameCommand { get; }

        /// <summary>
        /// Will try to execute the command.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public abstract void Handle(AppCommandRequest command);

        /// <summary>
        /// Sets next command.
        /// </summary>
        /// <param name="handler">Command.</param>
        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
        }

        /// <summary>
        /// Checks the current command satisfies the request; if not, then go to the next command.
        /// </summary>
        /// <param name="command">Request command.</param>
        /// <returns>True if satisfies; otherwise False.</returns>
        protected bool GoToNextCommand(AppCommandRequest command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (this.NameCommand.Equals(command.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (this.nextHandler is null)
            {
                PrintSimilarCommand(command.Command);
            }
            else
            {
                this.nextHandler.Handle(command);
            }

            return true;
        }

        private static void PrintSimilarCommand(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
            List<string[]> similarCommands = new List<string[]>(Array.FindAll(HelpCommandHandler.HelpMessages, i =>
            {
                int commandHelpIndex = 0;
                if (i[commandHelpIndex].Contains(command, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                int firstIndex = 0, lastIndex = i[commandHelpIndex].Length - 1;
                if (i[commandHelpIndex][firstIndex].Equals(command[firstIndex]) || i[commandHelpIndex][lastIndex].Equals(command[command.Length - 1]))
                {
                    return true;
                }

                return false;
            }));

            if (similarCommands.Count == 0)
            {
                return;
            }

            if (similarCommands.Count == 1)
            {
                Console.Write("The most similar command is\n");
            }
            else
            {
                Console.Write("The most similar commands are\n");
            }

            int commandHelpIndex = 0, commandDescriptionIndex = 2;
            foreach (var similarCommand in similarCommands)
            {
                Console.Write($"{similarCommand[commandHelpIndex]} - {similarCommand[commandDescriptionIndex]} \n");
            }
        }
    }
}

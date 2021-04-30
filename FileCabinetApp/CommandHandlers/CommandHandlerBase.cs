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
                Console.WriteLine($"There is no '{command}' command.");
                Console.WriteLine();
            }
            else
            {
                this.nextHandler.Handle(command);
            }

            return true;
        }
    }
}

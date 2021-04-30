using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string NameCommand { get => "exit"; }

        /// <summary>
        /// Exit from programm.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }
    }
}

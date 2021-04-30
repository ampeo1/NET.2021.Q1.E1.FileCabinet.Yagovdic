using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> changeStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="changeStatus">The delegate which changes the status of the program.</param>
        public ExitCommandHandler(Action<bool> changeStatus)
        {
            this.changeStatus = changeStatus;
        }

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
            this.changeStatus(false);
        }
    }
}

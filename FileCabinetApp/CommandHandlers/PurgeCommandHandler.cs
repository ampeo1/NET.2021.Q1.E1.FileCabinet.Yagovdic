using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string NameCommand => "purge";

        /// <summary>
        /// Defragments a file.
        /// </summary>
        /// <param name="command">Parameters of command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            int count = Program.fileCabinetService.GetCount();
            int removed = Program.fileCabinetService.Purge();
            Console.WriteLine($"Data file processing is completed: {removed} of {count}");
        }
    }
}

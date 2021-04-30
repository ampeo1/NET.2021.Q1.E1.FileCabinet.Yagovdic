using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string NameCommand => "stat";

        /// <summary>
        /// Prints statistics.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            var recordsCount = Program.fileCabinetService.GetCount();
            var recordsRemoved = Program.fileCabinetService.GetCountRemovedRecords();
            Console.WriteLine($"{recordsCount} record(s). {recordsRemoved} removed record(s).");
        }
    }
}

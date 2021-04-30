using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public StatCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

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

            var recordsCount = this.service.GetCount();
            var recordsRemoved = this.service.GetCountRemovedRecords();
            Console.WriteLine($"{recordsCount} record(s). {recordsRemoved} removed record(s).");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public PurgeCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

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

            int count = this.service.GetCount();
            int removed = this.service.Purge();
            Console.WriteLine($"Data file processing is completed: {removed} of {count}");
        }
    }
}

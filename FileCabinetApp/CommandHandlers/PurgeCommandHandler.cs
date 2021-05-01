using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Purge command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public PurgeCommandHandler(IFileCabinetService service)
           : base(service)
        {
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

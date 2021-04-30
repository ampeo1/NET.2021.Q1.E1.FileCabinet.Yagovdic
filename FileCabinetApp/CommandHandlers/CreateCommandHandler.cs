using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string NameCommand => "create";

        /// <summary>
        /// Create new record.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            int id;

            DataRecord dataRecord = DataRecord.CollectRecordData(this.service);
            try
            {
                id = this.service.CreateRecord(dataRecord);
                Console.WriteLine($"Record #{id} is created.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

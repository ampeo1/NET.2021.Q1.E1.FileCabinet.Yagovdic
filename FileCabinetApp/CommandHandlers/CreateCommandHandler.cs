using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler : CommandHandlerBase
    {
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

            DataRecord dataRecord = DataRecord.CollectRecordData();
            try
            {
                id = Program.fileCabinetService.CreateRecord(dataRecord);
                Console.WriteLine($"Record #{id} is created.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

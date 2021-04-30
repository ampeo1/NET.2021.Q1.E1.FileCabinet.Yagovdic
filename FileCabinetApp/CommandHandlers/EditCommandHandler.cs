using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class EditCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string NameCommand => "edit";

        /// <summary>
        /// Change record.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            if (!int.TryParse(command.Parameters, out int id))
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            long position = Program.fileCabinetService.FindById(id);
            if (position == -1)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            DataRecord dataRecord = DataRecord.CollectRecordData();
            dataRecord.Id = id;

            try
            {
                Program.fileCabinetService.EditRecord(dataRecord, position);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

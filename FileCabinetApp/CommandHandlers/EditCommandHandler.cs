using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Edit record command.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public EditCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

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

            long position = this.service.FindById(id);
            if (position == -1)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            DataRecord dataRecord = DataRecord.CollectRecordData(this.service);
            dataRecord.Id = id;

            try
            {
                this.service.EditRecord(dataRecord, position);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

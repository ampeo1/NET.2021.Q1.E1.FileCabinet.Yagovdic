using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Find date of birth command.
    /// </summary>
    public class FindDateOfBirthCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDateOfBirthCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        /// <param name="printer">Print style.</param>
        public FindDateOfBirthCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <inheritdoc/>
        protected override string NameCommand => "dateofbirth";

        /// <summary>
        /// Finds records by date of birth.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            if (!DateTime.TryParseExact(command.Parameters, "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("Error. Incorrect format, must be yyyy-mmm-dd");
            }

            var records = this.service.FindByBirthDay(date);
            this.printer.Print(records);
        }
    }
}

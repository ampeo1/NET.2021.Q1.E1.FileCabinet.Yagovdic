using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindDateOfBirthCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindDateOfBirthCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public FindDateOfBirthCommandHandler(IFileCabinetService service)
            : base(service)
        {
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
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, age: {record.Age}, salary {record.Salary}, access {record.Access}");
            }
        }
    }
}

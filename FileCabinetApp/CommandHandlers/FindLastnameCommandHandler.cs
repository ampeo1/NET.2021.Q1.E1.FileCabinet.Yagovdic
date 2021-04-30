using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindLastnameCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindLastnameCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public FindLastnameCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string NameCommand => "lastname";

        /// <summary>
        /// Finds records by lastname.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            var records = this.service.FindByLastname(command.Parameters);
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, age: {record.Age}, salary {record.Salary}, access {record.Access}");
            }
        }
    }
}

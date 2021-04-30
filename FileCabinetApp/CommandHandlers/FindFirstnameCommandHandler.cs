using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindFirstnameCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string NameCommand => "firstname";

        /// <summary>
        /// Finds records by firstname.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            var records = Program.fileCabinetService.FindByFirstName(command.Parameters);
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, age: {record.Age}, salary {record.Salary}, access {record.Access}");
            }
        }
    }
}

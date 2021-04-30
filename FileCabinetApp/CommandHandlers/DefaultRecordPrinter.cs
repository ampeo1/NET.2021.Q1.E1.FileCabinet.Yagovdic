using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class which prints records in default format.
    /// </summary>
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <inheritdoc/>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, age: {record.Age}, salary {record.Salary}, access {record.Access}");
            }
        }
    }
}

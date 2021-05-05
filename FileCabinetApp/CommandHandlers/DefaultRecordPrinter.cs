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
        /// <exception cref="ArgumentNullException">Throws when <paramref name="records"/> is null.</exception>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, age: {record.Age}, salary {record.Salary}, access {record.Access}");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="iterator"/> is null.</exception>
        public void PrintForIterator(IRecordIterator iterator)
        {
            if (iterator is null)
            {
                throw new ArgumentNullException(nameof(iterator));
            }

            while (iterator.HasMore())
            {
                FileCabinetRecord record = iterator.GetNext();
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, age: {record.Age}, salary {record.Salary}, access {record.Access}");
            }
        }
    }
}

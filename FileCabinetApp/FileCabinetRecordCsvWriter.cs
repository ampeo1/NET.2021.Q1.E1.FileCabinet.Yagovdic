using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// A class that writes data to a csv file.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Provider for writing.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)}");
            }

            this.writer = writer;
        }

        /// <summary>
        /// Writes data in csv file.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="record"/> is null.</exception>
        /// <param name="record">The record to be saved.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{record}");
            }

            char separator = ',';
            StringBuilder builder = new StringBuilder();
            builder.Append(record.Id);
            builder.Append(separator);
            builder.Append(record.FirstName);
            builder.Append(separator);
            builder.Append(record.LastName);
            builder.Append(separator);
            builder.Append(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            builder.Append(separator);
            builder.Append(record.Age);
            builder.Append(separator);
            builder.Append(record.Access);
            builder.Append(separator);
            builder.Append(record.AmountRecords);
            this.writer.WriteLine(builder.ToString());
        }
    }
}

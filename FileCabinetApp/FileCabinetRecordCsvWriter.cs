using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)}");
            }

            this.writer = writer;
        }

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

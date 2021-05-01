using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for reading records from csv file.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">Reader.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records from csv file.
        /// </summary>
        /// <returns>Records which contains in file.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            char separator = ',';
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            while (!this.reader.EndOfStream)
            {
                string[] parameters = this.reader.ReadLine().Split(separator);
                if (parameters.Length != 7)
                {
                    Console.WriteLine("Not enough parameters.");
                    continue;
                }

                FileCabinetRecord record = new FileCabinetRecord();
                try
                {
                    record.Id = DataChecking(Converter.IntConverted, parameters[0]);
                    record.FirstName = DataChecking(Converter.StringConverter, parameters[1]);
                    record.LastName = DataChecking(Converter.StringConverter, parameters[2]);
                    record.DateOfBirth = DataChecking(Converter.DateConverter, parameters[3]);
                    record.Age = DataChecking(Converter.ShortConverted, parameters[4]);
                    record.Access = DataChecking(Converter.CharConverted, parameters[5]);
                    record.Salary = DataChecking(Converter.DecimalConverted, parameters[6]);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Conversion failed: {ex.Message}.");
                    continue;
                }

                records.Add(record);
            }

            return records;
        }

        /// <summary>
        /// Checking data.
        /// </summary>
        /// <typeparam name="T">Type data.</typeparam>
        /// <param name="converter">Converter.</param>
        /// <param name="parameter">Parameter to be converted.</param>
        /// <exception cref="ArgumentException">Throws when parameters didn't converte.</exception>
        /// <returns>Converted parameter.</returns>
        private static T DataChecking<T>(Func<string, Tuple<bool, string, T>> converter, string parameter)
        {
            Tuple<bool, string, T> result = converter(parameter);
            if (!result.Item1)
            {
                throw new ArgumentException(result.Item2, nameof(parameter));
            }

            return result.Item3;
        }
    }
}

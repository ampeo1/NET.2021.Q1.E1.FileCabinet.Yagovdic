using System;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Introduce Parameter Object.
    /// </summary>
    public class DataRecord
    {
        /// <summary>
        /// Gets or sets identifier.
        /// </summary>
        /// <value>
        /// Identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        /// <value>
        /// First name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        /// <value>
        /// last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        /// <value>
        /// dd/MM/yyyy.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets access.
        /// </summary>
        /// <value>
        /// Access.
        /// </value>
        public char Access { get; set; }

        /// <summary>
        /// Gets or sets salary.
        /// </summary>
        /// <value>
        /// Salary.
        /// </value>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gathers information about a record.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        /// <exception cref="ArgumentNullException">Throws when service is null.</exception>
        /// <returns>Record data.</returns>
        public static DataRecord CollectRecordData(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            DataRecord dataRecord = new DataRecord();
            IRecordValidator validator = service.GetValidator();
            Console.Write("First name: ");
            dataRecord.FirstName = ReadInput(Converter.StringConverter);

            Console.Write("Last Name: ");
            dataRecord.LastName = ReadInput(Converter.StringConverter);

            Console.Write("Date of birth: ");
            dataRecord.DateOfBirth = ReadInput(Converter.DateConverter);

            Console.Write("Access: ");
            dataRecord.Access = ReadInput(Converter.CharConverted);

            Console.Write("Salary: ");
            dataRecord.Salary = ReadInput(Converter.DecimalConverted);

            return dataRecord;
        }

        /// <summary>
        /// Reads data from the console.
        /// </summary>
        /// <typeparam name="T">Type of object being read.</typeparam>
        /// <param name="converter">Function that performs convertation.</param>
        /// <returns>Processed data.</returns>
        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                return value;
            }
            while (true);
        }
    }
}

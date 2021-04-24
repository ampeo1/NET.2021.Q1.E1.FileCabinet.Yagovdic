using System;
using System.Text;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Generate random DataRecord.
    /// </summary>
    public static class Generator
    {
        private static readonly Random Randomization = new Random();

        /// <summary>
        /// Generate DataRecord.
        /// </summary>
        /// <param name="validator">The rules by which the data will be randomized.</param>
        /// <returns>DataRecord.</returns>
        public static DataRecord GenerateRecord(IRecordValidator validator)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            DataRecord record = new DataRecord
            {
                FirstName = GenerateString(validator.MinLengthForString, validator.MaxLengthForString),
                LastName = GenerateString(validator.MinLengthForString, validator.MaxLengthForString),
                Access = GenerateChar(validator.MinValueForChar, validator.MaxValueForChar),
                DateOfBirth = GenerateDateTime(validator.MinDateOfBirth, validator.MaxDateOfBirth),
                Salary = GenerateDecimal(validator.MinValueForSalary, validator.MaxValueForSalary),
            };
            return record;
        }

        private static string GenerateString(int minLength, int maxLength)
        {
            int length = Randomization.Next(minLength, maxLength + 1);
            StringBuilder builder = new StringBuilder();
            char startUpperCaseAlphabet = 'A', finishUpperCaseAlphabet = 'Z', startLowerCaseAlpahbet = 'a', finishLowerCaseAlphabet = 'z';
            if (length != 0)
            {
                builder.Append(GenerateChar(startUpperCaseAlphabet, finishUpperCaseAlphabet));
            }

            for (int i = 1; i < length; i++)
            {
                builder.Append(GenerateChar(startLowerCaseAlpahbet, finishLowerCaseAlphabet));
            }

            return builder.ToString();
        }

        private static char GenerateChar(char startAlphabet, char finishAlphabet)
        {
            return (char)Randomization.Next(startAlphabet, finishAlphabet + 1);
        }

        private static DateTime GenerateDateTime(DateTime minDate, DateTime maxDate)
        {
            int year = Randomization.Next(minDate.Year, maxDate.Year + 1);
            int month = Randomization.Next(minDate.Month, maxDate.Month + 1);
            int day = Randomization.Next(minDate.Day, maxDate.Day + 1);
            return new DateTime(year, month, day);
        }

        private static decimal GenerateDecimal(decimal min, decimal max)
        {
            return (decimal)Randomization.NextDouble() * Randomization.Next((int)min, (int)max);
        }
    }
}

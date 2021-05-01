using System;
using System.Text;
using FileCabinetApp;
using FileCabinetApp.Validators;

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
        /// <returns>DataRecord.</returns>
        public static DataRecord GenerateRecord()
        {
            DataRecord record = new DataRecord
            {
                FirstName = GenerateString(DefaultValidator.MinLengthForString, DefaultValidator.MaxLengthForString),
                LastName = GenerateString(DefaultValidator.MinLengthForString, DefaultValidator.MaxLengthForString),
                Access = GenerateChar(DefaultValidator.MinValueForChar, DefaultValidator.MaxValueForChar),
                DateOfBirth = GenerateDateTime(DefaultValidator.MinDateOfBirth, DefaultValidator.MaxDateOfBirth),
                Salary = GenerateDecimal(DefaultValidator.MinValueForSalary, DefaultValidator.MaxValueForSalary),
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

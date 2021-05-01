using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// This class does validate data.
    /// </summary>
    public static class DefaultValidator
    {
        private const char MinChar = 'A';
        private const char MaxChar = 'G';
        private const short MinLength = 2;
        private const short MaxLength = 60;
        private const decimal MaxSalary = 100000m;
        private const decimal MinSalary = decimal.Zero;
        private static readonly DateTime MinDate = new DateTime(1950, 01, 01);

        /// <summary>
        /// Gets min value for char.
        /// </summary>
        /// <value>
        /// Min value for char.
        /// </value>
        public static char MinValueForChar => MinChar;

        /// <summary>
        /// Gets max value for char.
        /// </summary>
        /// <value>
        /// Max value for char.
        /// </value>
        public static char MaxValueForChar => MaxChar;

        /// <summary>
        /// Gets min length for string.
        /// </summary>
        /// <value>
        /// Min length for string.
        /// </value>
        public static short MinLengthForString => MinLength;

        /// <summary>
        /// Gets max length for string.
        /// </summary>
        /// <value>
        /// Max length for string.
        /// </value>
        public static short MaxLengthForString => MaxLength;

        /// <summary>
        /// Gets min value for salary.
        /// </summary>
        /// <value>
        /// Min value for salary.
        /// </value>
        public static decimal MinValueForSalary => MinSalary;

        /// <summary>
        /// Gets max value for salary.
        /// </summary>
        /// <value>
        /// Max value for salary.
        /// </value>
        public static decimal MaxValueForSalary => MaxSalary;

        /// <summary>
        /// Gets min value for date of birth.
        /// </summary>
        /// <value>
        /// Min value for date of birth.
        /// </value>
        public static DateTime MinDateOfBirth => MinDate;

        /// <summary>
        /// Gets min value for date of birth.
        /// </summary>
        /// <value>
        /// Min value for date of birth.
        /// </value>
        public static DateTime MaxDateOfBirth => DateTime.Now;
    }
}

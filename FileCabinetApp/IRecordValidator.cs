using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Validate record.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Gets min value for char.
        /// </summary>
        /// <value>
        /// Min value for char.
        /// </value>
        public char MinValueForChar { get; }

        /// <summary>
        /// Gets max value for char.
        /// </summary>
        /// <value>
        /// Max value for char.
        /// </value>
        public char MaxValueForChar { get; }

        /// <summary>
        /// Gets min length for string.
        /// </summary>
        /// <value>
        /// Min length for string.
        /// </value>
        public short MinLengthForString { get; }

        /// <summary>
        /// Gets max length for string.
        /// </summary>
        /// <value>
        /// Max length for string.
        /// </value>
        public short MaxLengthForString { get; }

        /// <summary>
        /// Gets min value for salary.
        /// </summary>
        /// <value>
        /// Min value for salary.
        /// </value>
        public decimal MinValueForSalary { get; }

        /// <summary>
        /// Gets max value for salary.
        /// </summary>
        /// <value>
        /// Max value for salary.
        /// </value>
        public decimal MaxValueForSalary { get; }

        /// <summary>
        /// Gets min value for date of birth.
        /// </summary>
        /// <value>
        /// Min value for date of birth.
        /// </value>
        public DateTime MinDateOfBirth { get; }

        /// <summary>
        /// Gets min value for date of birth.
        /// </summary>
        /// <value>
        /// Min value for date of birth.
        /// </value>
        public DateTime MaxDateOfBirth { get; }

        /// <summary>
        /// Validate all arguments.
        /// </summary>
        /// <param name="dataRecord">Checked arguments.</param>
        public void ValidateParameters(DataRecord dataRecord);

        /// <summary>
        /// Validate first name property.
        /// </summary>
        /// <param name="firstName">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateFirstName(string firstName);

        /// <summary>
        /// Validate last name property.
        /// </summary>
        /// <param name="lastName">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateLastName(string lastName);

        /// <summary>
        /// Validate access property.
        /// </summary>
        /// <param name="access">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateAccess(char access);

        /// <summary>
        /// Validate date of birth property.
        /// </summary>
        /// <param name="dateOfBirth">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Validate salary property.
        /// </summary>
        /// <param name="salary">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateSalary(decimal salary);
    }
}

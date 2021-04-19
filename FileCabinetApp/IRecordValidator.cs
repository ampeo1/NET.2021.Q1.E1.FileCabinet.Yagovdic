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

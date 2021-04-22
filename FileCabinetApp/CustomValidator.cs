using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// This class does validate data.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        private const char MinChar = 'a';
        private const char MaxChar = 'g';
        private const short MinLength = 1;
        private const short MaxLength = 100;
        private const decimal MaxSalary = 10000000m;
        private const decimal MinSalary = decimal.Zero;
        private static readonly DateTime MinDate = new DateTime(1920, 01, 01);

        /// <summary>
        /// Gets min value for char.
        /// </summary>
        /// <value>
        /// Min value for char.
        /// </value>
        public static char MinValueForChar
        {
            get
            {
                return MinChar;
            }
        }

        /// <summary>
        /// Gets max value for char.
        /// </summary>
        /// <value>
        /// Max value for char.
        /// </value>
        public static char MaxValueForChar
        {
            get
            {
                return MaxChar;
            }
        }

        /// <summary>
        /// Gets min length for string.
        /// </summary>
        /// <value>
        /// Min length for string.
        /// </value>
        public static short MinLengthForString
        {
            get
            {
                return MinLength;
            }
        }

        /// <summary>
        /// Gets max length for string.
        /// </summary>
        /// <value>
        /// Max length for string.
        /// </value>
        public static short MaxLengthForString
        {
            get
            {
                return MaxLength;
            }
        }

        /// <summary>
        /// Gets min value for salary.
        /// </summary>
        /// <value>
        /// Min value for salary.
        /// </value>
        public static decimal MinValueForSalary
        {
            get
            {
                return MinSalary;
            }
        }

        /// <summary>
        /// Gets max value for salary.
        /// </summary>
        /// <value>
        /// Max value for salary.
        /// </value>
        public static decimal MaxValueForSalary
        {
            get
            {
                return MaxSalary;
            }
        }

        /// <summary>
        /// Gets min value for date of birth.
        /// </summary>
        /// <value>
        /// Min value for date of birth.
        /// </value>
        public static DateTime MinDateOfBirth
        {
            get
            {
                return MinDate;
            }
        }

        /// <summary>
        /// Gets min value for date of birth.
        /// </summary>
        /// <value>
        /// Min value for date of birth.
        /// </value>
        public static DateTime MaxDateOfBirth
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Validate access property.
        /// </summary>
        /// <param name="access">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateAccess(char access)
        {
            string errorMessage = string.Empty;
            if (access < MinChar || access > MaxChar)
            {
                errorMessage = "Access doesn't contains [a, b, c, d, e, f, g]";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        /// <summary>
        /// Validate date of birth property.
        /// </summary>
        /// <param name="dateOfBirth">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            string errorMessage = string.Empty;
            if (DateTime.Compare(DateTime.Now, dateOfBirth) < 0 || DateTime.Compare(MinDate, dateOfBirth) > 0)
            {
                errorMessage = $"Date of birth is less than {MinDateOfBirth.ToString("yyyy - MMM - dd", CultureInfo.InvariantCulture)} or greater than now";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        /// <summary>
        /// Validate first name property.
        /// </summary>
        /// <param name="firstName">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(firstName))
            {
                errorMessage = "Argument is null or with whiteSpace";
                return new Tuple<bool, string>(false, errorMessage);
            }

            if (firstName.Length < MinLength || firstName.Length > MaxLength)
            {
                errorMessage = $"Length is less than {MinLength} or greater than {MaxLength}";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        /// <summary>
        /// Validate last name property.
        /// </summary>
        /// <param name="lastName">Checked argument.</param>
        /// <returns>
        /// First argument: True if successfully validated; otherwise false.
        /// Second argument: Error message if there is an error.
        /// </returns>
        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(lastName))
            {
                errorMessage = "Argument is null or with whiteSpace";
                return new Tuple<bool, string>(false, errorMessage);
            }

            if (lastName.Length < MinLength || lastName.Length > MaxLength)
            {
                errorMessage = $"Length is less than {MinLength} or greater than {MaxLength}";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> ValidateSalary(decimal salary)
        {
            string errorMessage = string.Empty;
            if (salary.CompareTo(MinSalary) == -1 || salary.CompareTo(MaxSalary) == 1)
            {
                errorMessage = $"Length is less than {MinSalary} or greater than {MaxSalary}";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        /// <summary>
        /// Validate all arguments.
        /// </summary>
        /// <param name="dataRecord">Checked arguments.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="dataRecord"/> is null.</exception>
        public void ValidateParameters(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            CheckResult(this.ValidateFirstName(dataRecord.FirstName));
            CheckResult(this.ValidateLastName(dataRecord.LastName));
            CheckResult(this.ValidateDateOfBirth(dataRecord.DateOfBirth));
            CheckResult(this.ValidateAccess(dataRecord.Access));
        }

        /// <summary>
        /// Checks if validation was successful.
        /// </summary>
        /// <param name="resultValidation">Result of validation.</param>
        /// <exception cref="ArgumentException">Throws when validation failed.</exception>
        private static void CheckResult(Tuple<bool, string> resultValidation)
        {
            if (!resultValidation.Item1)
            {
                throw new ArgumentException($"{resultValidation.Item2}");
            }
        }
    }
}

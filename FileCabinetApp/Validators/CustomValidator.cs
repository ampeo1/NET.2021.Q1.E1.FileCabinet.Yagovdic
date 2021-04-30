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

            var firstNameValidator = new FirstnameValidator(MinLength, MaxLength);
            var lastNameValidator = new LastnameValidator(MinLength, MaxLength);
            var dateOfBirthValidator = new DateOfBirthValidator(MinDate, DateTime.Now);
            var accessValidator = new AccessValidator(MinChar, MaxChar);
            var salaryValidator = new SalaryValidator(MinSalary, MaxSalary);

            try
            {
                firstNameValidator.ValidateParameters(dataRecord);
                lastNameValidator.ValidateParameters(dataRecord);
                dateOfBirthValidator.ValidateParameters(dataRecord);
                accessValidator.ValidateParameters(dataRecord);
                salaryValidator.ValidateParameters(dataRecord);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

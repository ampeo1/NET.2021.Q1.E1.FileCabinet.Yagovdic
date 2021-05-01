using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Creates validator.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBuilder"/> class.
        /// </summary>
        public ValidatorBuilder()
        {
            this.validators = new List<IRecordValidator>();
        }

        /// <summary>
        /// Add validator for first name.
        /// </summary>
        /// <param name="min">Min length.</param>
        /// <param name="max">Max length.</param>
        /// <returns>Builder validator.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstnameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Add validator for last name.
        /// </summary>
        /// <param name="min">Min length.</param>
        /// <param name="max">Max length.</param>
        /// <returns>Builder validator.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastnameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Add validator for date of birth.
        /// </summary>
        /// <param name="from">Min date.</param>
        /// <param name="to">Max date.</param>
        /// <returns>Builder validator.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>
        /// Add validator for access.
        /// </summary>
        /// <param name="min">Min char value.</param>
        /// <param name="max">Max char value.</param>
        /// <returns>Builder validator.</returns>
        public ValidatorBuilder ValidateAccess(char min, char max)
        {
            this.validators.Add(new AccessValidator(min, max));
            return this;
        }

        /// <summary>
        /// Add validator for salary.
        /// </summary>
        /// <param name="min">Min salary.</param>
        /// <param name="max">Max salary.</param>
        /// <returns>Builder validator.</returns>
        public ValidatorBuilder ValidateSalary(decimal min, decimal max)
        {
            this.validators.Add(new SalaryValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates IRecordValidator.
        /// </summary>
        /// <returns>IRecordValidator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}

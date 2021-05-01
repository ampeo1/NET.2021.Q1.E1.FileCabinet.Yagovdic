using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validate salary.
    /// </summary>
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal minSalary;
        private readonly decimal maxSalary;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryValidator"/> class.
        /// </summary>
        /// <param name="minSalary">Min value for validation.</param>
        /// <param name="maxSalary">Max value for validation.</param>
        public SalaryValidator(decimal minSalary, decimal maxSalary)
        {
            this.minSalary = minSalary;
            this.maxSalary = maxSalary;
        }

        /// <summary>
        /// Validate salary.
        /// </summary>
        /// <param name="dataRecord">Data for validations.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="dataRecord"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when salary greater than max value or less than min value.</exception>
        public void ValidateParameters(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            if (dataRecord.Salary.CompareTo(this.minSalary) == -1 || dataRecord.Salary.CompareTo(this.maxSalary) == 1)
            {
                string errorMessage = $"Value is less than {this.minSalary} or greater than {this.maxSalary}";
                throw new ArgumentException(errorMessage, nameof(dataRecord));
            }
        }
    }
}

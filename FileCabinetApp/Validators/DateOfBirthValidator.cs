using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validate date of birth.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">Min date for validation.</param>
        /// <param name="to">Max date for validation.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Validate date of birth.
        /// </summary>
        /// <param name="dataRecord">Data for validations.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="dataRecord"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when date of birth greater than max date or less than min date.</exception>
        public void ValidateParameters(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            if (DateTime.Compare(this.to, dataRecord.DateOfBirth) < 0 || DateTime.Compare(this.from, dataRecord.DateOfBirth) > 0)
            {
                string errorMessage = $"Date of birth is less than {this.from.ToString("yyyy - MMM - dd", CultureInfo.InvariantCulture)} or greater than {this.to.ToString("yyyy - MMM - dd", CultureInfo.InvariantCulture)}";
                throw new ArgumentException(errorMessage, nameof(dataRecord));
            }
        }
    }
}

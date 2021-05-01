using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validate lastname.
    /// </summary>
    public class LastnameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastnameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Min length for validation.</param>
        /// <param name="maxLength">Max length for validation.</param>
        public LastnameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Validate lastname.
        /// </summary>
        /// <param name="dataRecord">Data for validations.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="dataRecord"/> is null or lastname is null.</exception>
        /// <exception cref="ArgumentException">Throws when length lastname greater than max length or less than min length.</exception>
        public void ValidateParameters(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(dataRecord.LastName))
            {
                errorMessage = "Argument is null or with whiteSpace";
                throw new ArgumentNullException(errorMessage, nameof(dataRecord.LastName));
            }

            if (dataRecord.LastName.Length < this.minLength || dataRecord.LastName.Length > this.maxLength)
            {
                errorMessage = $"Length is less than {this.minLength} or greater than {this.maxLength}";
                throw new ArgumentException(errorMessage, nameof(dataRecord));
            }
        }
    }
}

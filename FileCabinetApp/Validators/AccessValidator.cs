using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validate access.
    /// </summary>
    public class AccessValidator : IRecordValidator
    {
        private readonly char min;
        private readonly char max;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessValidator"/> class.
        /// </summary>
        /// <param name="min">Min char for validation.</param>
        /// <param name="max">Max char for validation.</param>
        public AccessValidator(char min, char max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Validate access.
        /// </summary>
        /// <param name="dataRecord">Data for validations.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="dataRecord"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when value char greater than max value char or less than min value char.</exception>
        public void ValidateParameters(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            string errorMessage = string.Empty;

            if (dataRecord.Access < this.min || dataRecord.Access > this.max)
            {
                errorMessage = $"Access doesn't contains {this.min} - {this.max}";
                throw new ArgumentException(errorMessage, nameof(dataRecord));
            }
        }
    }
}

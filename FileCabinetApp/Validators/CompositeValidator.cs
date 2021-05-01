using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Composite for validators.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = new List<IRecordValidator>(validators);
        }

        /// <summary>
        /// Validate parameters.
        /// </summary>
        /// <param name="dataRecord">Parameters.</param>
        /// <exception cref="ArgumentException">Throws when parameters doesn't validate.</exception>
        public void ValidateParameters(DataRecord dataRecord)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(dataRecord);
            }
        }
    }
}

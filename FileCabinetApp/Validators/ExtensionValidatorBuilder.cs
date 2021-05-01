using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public static class ExtensionValidatorBuilder
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            char minChar = 'A';
            char maxChar = 'G';
            short minLength = 2;
            short maxLength = 60;
            decimal maxSalary = 100000m;
            decimal minSalary = decimal.Zero;
            DateTime minDate = new DateTime(1950, 01, 01);
            DateTime maxDate = DateTime.Now;
            return builder.ValidateFirstName(minLength, maxLength)
                          .ValidateLastName(minLength, maxLength)
                          .ValidateDateOfBirth(minDate, maxDate)
                          .ValidateAccess(minChar, maxChar)
                          .ValidateSalary(minSalary, maxSalary)
                          .Create();
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            char minChar = 'a';
            char maxChar = 'g';
            short minLength = 1;
            short maxLength = 100;
            decimal maxSalary = 10000000m;
            decimal minSalary = decimal.Zero;
            DateTime minDate = new DateTime(1920, 01, 01);
            DateTime maxDate = DateTime.Now;
            return builder.ValidateFirstName(minLength, maxLength)
                          .ValidateLastName(minLength, maxLength)
                          .ValidateDateOfBirth(minDate, maxDate)
                          .ValidateAccess(minChar, maxChar)
                          .ValidateSalary(minSalary, maxSalary)
                          .Create();
        }
    }
}

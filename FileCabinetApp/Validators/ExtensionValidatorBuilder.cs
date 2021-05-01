using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Extension builder validator.
    /// </summary>
    public static class ExtensionValidatorBuilder
    {
        /// <summary>
        /// Create default validator.
        /// </summary>
        /// <param name="builder">Extension type.</param>
        /// <returns>Record validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            IConfiguration config = new ConfigurationBuilder().AddJsonFile("validation-rules.json").Build();

            char minChar = char.Parse(config["default:access:min"]);
            char maxChar = char.Parse(config["default:access:max"]);
            int minLengthFirstName = int.Parse(config["default:firstName:min"], CultureInfo.InvariantCulture);
            int maxLengthFirstName = int.Parse(config["default:firstName:max"], CultureInfo.InvariantCulture);
            int minLengthLastName = int.Parse(config["default:lastName:min"], CultureInfo.InvariantCulture);
            int maxLengthLastName = int.Parse(config["default:lastName:max"], CultureInfo.InvariantCulture);
            decimal maxSalary = decimal.Parse(config["default:salary:max"], CultureInfo.InvariantCulture);
            decimal minSalary = decimal.Parse(config["default:salary:min"], CultureInfo.InvariantCulture);
            DateTime minDate = DateTime.Parse(config["default:dateOfBirth:from"], CultureInfo.InvariantCulture);
            DateTime maxDate = DateTime.Parse(config["default:dateOfBirth:to"], CultureInfo.InvariantCulture);
            return builder.ValidateFirstName(minLengthFirstName, maxLengthFirstName)
                          .ValidateLastName(minLengthLastName, maxLengthLastName)
                          .ValidateDateOfBirth(minDate, maxDate)
                          .ValidateAccess(minChar, maxChar)
                          .ValidateSalary(minSalary, maxSalary)
                          .Create();
        }

        /// <summary>
        /// Create custom validator.
        /// </summary>
        /// <param name="builder">Extension type.</param>
        /// <returns>Record validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            IConfiguration config = new ConfigurationBuilder().AddJsonFile("validation-rules.json").Build();

            char minChar = char.Parse(config["custom:access:min"]);
            char maxChar = char.Parse(config["custom:access:max"]);
            int minLengthFirstName = int.Parse(config["custom:firstName:min"], CultureInfo.InvariantCulture);
            int maxLengthFirstName = int.Parse(config["custom:firstName:max"], CultureInfo.InvariantCulture);
            int minLengthLastName = int.Parse(config["custom:lastName:min"], CultureInfo.InvariantCulture);
            int maxLengthLastName = int.Parse(config["custom:lastName:max"], CultureInfo.InvariantCulture);
            decimal maxSalary = decimal.Parse(config["custom:salary:max"], CultureInfo.InvariantCulture);
            decimal minSalary = decimal.Parse(config["custom:salary:min"], CultureInfo.InvariantCulture);
            DateTime minDate = DateTime.Parse(config["custom:dateOfBirth:from"], CultureInfo.InvariantCulture);
            DateTime maxDate = DateTime.Parse(config["custom:dateOfBirth:to"], CultureInfo.InvariantCulture);
            return builder.ValidateFirstName(minLengthFirstName, maxLengthFirstName)
                          .ValidateLastName(minLengthLastName, maxLengthLastName)
                          .ValidateDateOfBirth(minDate, maxDate)
                          .ValidateAccess(minChar, maxChar)
                          .ValidateSalary(minSalary, maxSalary)
                          .Create();
        }
    }
}

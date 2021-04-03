using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        public Tuple<bool, string> ValidateAccess(char access)
        {
            string errorMessage = string.Empty;
            if (access < 'a' || access > 'g')
            {
                errorMessage = "Access doesn't contains [a, b, c, d, e, f, g]";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            string errorMessage = string.Empty;
            if (dateOfBirth < new DateTime(1920, 01, 01) || dateOfBirth > DateTime.Now)
            {
                errorMessage = "Date of birth is less than 01-jan-1920 or greater than now";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(firstName))
            {
                errorMessage = "Argument is null or with whiteSpace";
                return new Tuple<bool, string>(false, errorMessage);
            }

            if (firstName.Length < 1 || firstName.Length > 100)
            {
                errorMessage = "Length is less than 1 or greater than 100";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(lastName))
            {
                errorMessage = "Argument is null or with whiteSpace";
                return new Tuple<bool, string>(false, errorMessage);
            }

            if (lastName.Length < 1 || lastName.Length > 100)
            {
                errorMessage = "Length is less than 1 or greater than 100";
                return new Tuple<bool, string>(false, errorMessage);
            }

            return new Tuple<bool, string>(true, errorMessage);
        }

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

        private static void CheckResult(Tuple<bool, string> resultValidation)
        {
            if (!resultValidation.Item1)
            {
                throw new ArgumentException($"{resultValidation.Item2}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        public void ValidateParameters(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            if (string.IsNullOrWhiteSpace(dataRecord.FirstName) || string.IsNullOrWhiteSpace(dataRecord.LastName))
            {
                throw new ArgumentNullException($"{nameof(dataRecord.FirstName)}, {nameof(dataRecord.LastName)}");
            }

            if (dataRecord.FirstName.Length < 1 || dataRecord.FirstName.Length > 100 || dataRecord.LastName.Length < 1 || dataRecord.LastName.Length > 100)
            {
                throw new ArgumentException($"{nameof(dataRecord.FirstName)} or {nameof(dataRecord.LastName)} length is less than 1 or greater than 100", $"{nameof(dataRecord.FirstName)}, {nameof(dataRecord.LastName)}");
            }

            if (dataRecord.DateOfBirth < new DateTime(1920, 01, 01) || dataRecord.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dataRecord.DateOfBirth)} is less than 01-jan-1920 or greater than now");
            }

            if (dataRecord.Access < 'a' || dataRecord.Access > 'g')
            {
                throw new ArgumentException($"{nameof(dataRecord.Access)} doesn't contains [a, b, c, d, e, f, g]");
            }
        }
    }
}

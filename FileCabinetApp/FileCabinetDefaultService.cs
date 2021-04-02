using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetDefaultService : FileCabinetService
    {
        protected override void ValidateParameters(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            if (string.IsNullOrWhiteSpace(dataRecord.FirstName) || string.IsNullOrWhiteSpace(dataRecord.LastName))
            {
                throw new ArgumentNullException($"{nameof(dataRecord.FirstName)}, {nameof(dataRecord.LastName)}");
            }

            if (dataRecord.FirstName.Length < 2 || dataRecord.FirstName.Length > 60 || dataRecord.LastName.Length < 2 || dataRecord.LastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(dataRecord.FirstName)} or {nameof(dataRecord.LastName)} length is less than 2 or greater than 60", $"{nameof(dataRecord.FirstName)}, {nameof(dataRecord.LastName)}");
            }

            if (dataRecord.DateOfBirth < new DateTime(1950, 01, 01) || dataRecord.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dataRecord.DateOfBirth)} is less than 01-jan-1950 or greater than now");
            }

            if (dataRecord.Access < 'A' || dataRecord.Access > 'G')
            {
                throw new ArgumentException($"{nameof(dataRecord.Access)} doesn't contains [A, B, C, D, E, F, G]");
            }
        }
    }
}

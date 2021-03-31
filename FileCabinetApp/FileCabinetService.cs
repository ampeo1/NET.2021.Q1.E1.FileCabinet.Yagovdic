using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char access)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException($"{nameof(firstName)}, {nameof(lastName)}");
            }

            if (firstName.Length < 2 || firstName.Length > 60 || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(firstName)} or {nameof(lastName)} length is less than 2 or greater than 60", $"{nameof(firstName)}, {nameof(lastName)}");
            }

            if (dateOfBirth < new DateTime(01, 01, 1950) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is less than 01-jan-1950 or greater than now");
            }

            if (access < 'A' || access > 'G')
            {
                throw new ArgumentException($"{nameof(access)} does not contains [A, B, C, D, E, F, G]");
            }

            short age = (short)(DateTime.Now.Year - dateOfBirth.Year);
            if (dateOfBirth > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Access = access,
                AmountRecords = 0,
                Age = age,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}

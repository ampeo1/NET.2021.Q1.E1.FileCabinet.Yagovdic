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
            int id = this.list.Count + 1;
            FileCabinetRecord record = Create(id, firstName, lastName, dateOfBirth, access);

            this.list.Add(record);

            return record.Id;
        }

        public void EditRecord(int index, int id, string firstName, string lastName, DateTime dateOfBirth, char access)
        {
            FileCabinetRecord record = Create(id, firstName, lastName, dateOfBirth, access);
            this.list[index] = record;
        }

        public int FindIndexById(int id)
        {
            int index = this.list.FindIndex(x => x.Id == id);
            if (index == -1)
            {
                throw new ArgumentException($"{nameof(id)} record is not found.");
            }

            return index;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> results = new List<FileCabinetRecord>();
            foreach (var item in this.list)
            {
                if (item.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    results.Add(item);
                }
            }

            return results.ToArray();
        }

        public FileCabinetRecord[] FindByLastname(string lastname)
        {
            List<FileCabinetRecord> results = new List<FileCabinetRecord>();
            foreach (var item in this.list)
            {
                if (item.LastName.Equals(lastname, StringComparison.InvariantCultureIgnoreCase))
                {
                    results.Add(item);
                }
            }

            return results.ToArray();
        }

        public FileCabinetRecord[] FindByBirthDay(DateTime date)
        {
            List<FileCabinetRecord> results = new List<FileCabinetRecord>();
            foreach (var item in this.list)
            {
                if (item.DateOfBirth.Equals(date))
                {
                    results.Add(item);
                }
            }

            return results.ToArray();
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        private static FileCabinetRecord Create(int id, string firstName, string lastName, DateTime dateOfBirth, char access)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException($"{nameof(firstName)}, {nameof(lastName)}");
            }

            if (firstName.Length < 2 || firstName.Length > 60 || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(firstName)} or {nameof(lastName)} length is less than 2 or greater than 60", $"{nameof(firstName)}, {nameof(lastName)}");
            }

            if (dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
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
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Access = access,
                AmountRecords = 0,
                Age = age,
            };

            return record;
        }

    }
}

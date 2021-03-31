using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char access)
        {
            int id = this.list.Count + 1;
            FileCabinetRecord record = Create(id, firstName, lastName, dateOfBirth, access);
            this.AddRecordToDictionaries(record);
            this.list.Add(record);

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char access)
        {
            int index = this.FindIndexById(id);
            FileCabinetRecord record = Create(id, firstName, lastName, dateOfBirth, access);
            this.RemoveRecordFromDictionaries(this.list[index]);
            this.AddRecordToDictionaries(record);
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
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        public FileCabinetRecord[] FindByLastname(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return this.lastNameDictionary[lastName].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
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

        private void AddRecordToDictionaries(FileCabinetRecord record)
        {
            if (!this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary[record.FirstName] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[record.FirstName].Add(record);

            if (!this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[record.LastName] = new List<FileCabinetRecord>();
            }

            this.lastNameDictionary[record.LastName].Add(record);
        }

        private void RemoveRecordFromDictionaries(FileCabinetRecord record)
        {
            if (this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary[record.FirstName].Remove(record);
            }

            if (this.firstNameDictionary.ContainsKey(record.LastName))
            {
                this.firstNameDictionary[record.LastName].Remove(record);
            }
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

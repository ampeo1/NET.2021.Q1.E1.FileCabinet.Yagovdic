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
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            dataRecord.Id = this.list.Count + 1;
            FileCabinetRecord record = Create(dataRecord);
            this.AddRecordToDictionaries(record);
            this.list.Add(record);

            return record.Id;
        }

        public void EditRecord(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            int index = this.FindIndexById(dataRecord.Id);
            FileCabinetRecord record = Create(dataRecord);
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
            if (this.dateOfBirthDictionary.ContainsKey(date))
            {
                return this.dateOfBirthDictionary[date].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
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

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth] = new List<FileCabinetRecord>();
            }

            this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
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

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
            }
        }

        private static FileCabinetRecord Create(DataRecord dataRecord)
        {
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

            short age = (short)(DateTime.Now.Year - dataRecord.DateOfBirth.Year);
            if (dataRecord.DateOfBirth > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            var record = new FileCabinetRecord
            {
                Id = dataRecord.Id,
                FirstName = dataRecord.FirstName,
                LastName = dataRecord.LastName,
                DateOfBirth = dataRecord.DateOfBirth,
                Access = dataRecord.Access,
                AmountRecords = 0,
                Age = age,
            };

            return record;
        }

    }
}

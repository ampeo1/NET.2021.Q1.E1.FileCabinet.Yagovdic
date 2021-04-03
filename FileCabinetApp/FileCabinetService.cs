using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;

        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        public int CreateRecord(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            dataRecord.Id = this.list.Count + 1;
            FileCabinetRecord record = this.Create(dataRecord);
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
            FileCabinetRecord record = this.Create(dataRecord);
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

        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName];
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByLastname(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return this.lastNameDictionary[lastName];
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByBirthDay(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                return this.dateOfBirthDictionary[dateOfBirth];
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list;
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        private static void RemoveRecordFromDictiornary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T key, FileCabinetRecord record)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Remove(record);
            }
        }

        private static void AddRecordToDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T key, FileCabinetRecord record)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<FileCabinetRecord>();
            }

            dictionary[key].Add(record);
        }

        private FileCabinetRecord Create(DataRecord dataRecord)
        {
            this.validator.ValidateParameters(dataRecord);

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

        private void AddRecordToDictionaries(FileCabinetRecord record)
        {
            AddRecordToDictionary(this.firstNameDictionary, record.FirstName, record);
            AddRecordToDictionary(this.lastNameDictionary, record.LastName, record);
            AddRecordToDictionary(this.dateOfBirthDictionary, record.DateOfBirth, record);
        }

        private void RemoveRecordFromDictionaries(FileCabinetRecord record)
        {
            RemoveRecordFromDictiornary(this.firstNameDictionary, record.FirstName, record);
            RemoveRecordFromDictiornary(this.lastNameDictionary, record.LastName, record);
            RemoveRecordFromDictiornary(this.dateOfBirthDictionary, record.DateOfBirth, record);
        }
    }
}

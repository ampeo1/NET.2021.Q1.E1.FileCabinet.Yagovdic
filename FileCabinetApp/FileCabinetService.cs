using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Representation file cabinet service.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">validation-rules.</param>
        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="dataRecord">record data.</param>
        /// <exception cref="ArgumentNullException">Trows when <paramref name="dataRecord"/> is null.</exception>
        /// <exception cref="ArgumentException">Trows when data is invalid.</exception>
        /// <returns>Record id.</returns>
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

        /// <summary>
        /// Changes record.
        /// </summary>
        /// <exception cref="ArgumentNullException">Trows when <paramref name="dataRecord"/> is null.</exception>
        /// <exception cref="ArgumentException">Trows when data is invalid.</exception>
        /// <param name="dataRecord">Record data.</param>
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

        /// <summary>
        /// Finds index record by id.
        /// </summary>
        /// <param name="id">Identifier of the searched record.</param>
        /// <exception cref="ArgumentException">Throws when record not found.</exception>
        /// <returns>Index record.</returns>
        public int FindIndexById(int id)
        {
            int index = this.list.FindIndex(x => x.Id == id);
            if (index == -1)
            {
                throw new ArgumentException($"{nameof(id)} record is not found.");
            }

            return index;
        }

        /// <summary>
        /// Finds record by first name.
        /// </summary>
        /// <param name="firstName">First name of the searched record.</param>
        /// <returns>Found records.</returns>
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

        /// <summary>
        /// Finds record by last name.
        /// </summary>
        /// <param name="lastName">Last name of the searched record.</param>
        /// <returns>Found records.</returns>
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

        /// <summary>
        /// Finds record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">date of birth of the searched record.</param>
        /// <returns>Found records.</returns>
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

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>Records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list;
        }

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Gets validator.
        /// </summary>
        /// <returns>Validator.</returns>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <summary>
        /// Removes record from dictionary by key.
        /// </summary>
        /// <typeparam name="T">Type key.</typeparam>
        /// <param name="dictionary">The dictionary in which to remove the element.</param>
        /// <param name="key">The key by which the search will be.</param>
        /// <param name="record">The record to be deleted.</param>
        private static void RemoveRecordFromDictiornary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T key, FileCabinetRecord record)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Remove(record);
            }
        }

        /// <summary>
        /// Adds record to dictionary by key.
        /// </summary>
        /// <typeparam name="T">Type key.</typeparam>
        /// <param name="dictionary">The dictionary in which to add the element.</param>
        /// <param name="key">The key by which the search will be.</param>
        /// <param name="record">The record to be added.</param>
        private static void AddRecordToDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T key, FileCabinetRecord record)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<FileCabinetRecord>();
            }

            dictionary[key].Add(record);
        }

        /// <summary>
        /// Creates record.
        /// </summary>
        /// <param name="dataRecord">Record data.</param>
        /// <returns>Record.</returns>
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

        /// <summary>
        /// Adds a record to all dictionaries.
        /// </summary>
        /// <param name="record">The record which need to add.</param>
        private void AddRecordToDictionaries(FileCabinetRecord record)
        {
            AddRecordToDictionary(this.firstNameDictionary, record.FirstName, record);
            AddRecordToDictionary(this.lastNameDictionary, record.LastName, record);
            AddRecordToDictionary(this.dateOfBirthDictionary, record.DateOfBirth, record);
        }

        /// <summary>
        /// Removes a record to all dictionaries.
        /// </summary>
        /// <param name="record">The record which need to remove.</param>
        private void RemoveRecordFromDictionaries(FileCabinetRecord record)
        {
            RemoveRecordFromDictiornary(this.firstNameDictionary, record.FirstName, record);
            RemoveRecordFromDictiornary(this.lastNameDictionary, record.LastName, record);
            RemoveRecordFromDictiornary(this.dateOfBirthDictionary, record.DateOfBirth, record);
        }
    }
}

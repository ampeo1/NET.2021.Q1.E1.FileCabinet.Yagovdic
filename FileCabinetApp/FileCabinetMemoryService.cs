using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Representation file cabinet service in memory.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> records = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;
        private int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">Validation-rules.</param>
        /// <param name="startId">The id to start with.</param>
        public FileCabinetMemoryService(IRecordValidator validator, int startId = 0)
        {
            this.validator = validator;
            this.id = startId;
        }

        /// <inheritdoc/>
        public int CreateRecord(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            dataRecord.Id = this.id++;
            FileCabinetRecord record = this.Create(dataRecord);
            this.AddRecordToDictionaries(record);
            this.records.Add(record);

            return record.Id;
        }

        /// <inheritdoc/>
        public void EditRecord(DataRecord dataRecord, long position)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException($"{nameof(dataRecord)}");
            }

            if (position < 0 || position > this.records.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            int index;
            try
            {
                index = checked((int)position);
            }
            catch (OverflowException)
            {
                return;
            }

            FileCabinetRecord record = this.Create(dataRecord);
            this.RemoveRecordFromDictionaries(this.records[index]);
            this.AddRecordToDictionaries(record);
            this.records[index] = record;
        }

        /// <inheritdoc/>
        public long FindById(int id)
        {
            return this.records.FindIndex(x => x.Id == id);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.records;
        }

        /// <inheritdoc/>
        public int GetCount()
        {
            return this.records.Count;
        }

        /// <inheritdoc/>
        public int GetCountRemovedRecords()
        {
            return 0;
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.records.ToArray());
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            if (snapshot.Records is null)
            {
                throw new ArgumentException("Snapshot hasn't FileCabinetRecords.", nameof(snapshot));
            }

            foreach (var item in snapshot.Records)
            {
                int index = this.records.FindIndex(0, this.records.Count, x => x.Id == item.Id);
                if (index != -1)
                {
                    this.records[index] = item;
                }
                else
                {
                    this.records.Add(item);
                }
            }
        }

        /// <inheritdoc/>
        public bool Remove(int id)
        {
            FileCabinetRecord record = this.records.Find(x => x.Id == id);
            if (record is null)
            {
                return false;
            }

            this.records.Remove(record);
            return true;
        }

        /// <summary>
        /// Always return 0.
        /// </summary>
        /// <returns>0.</returns>
        public int Purge()
        {
            return 0;
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
                Salary = dataRecord.Salary,
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

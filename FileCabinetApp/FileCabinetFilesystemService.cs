using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Representation file cabinet service in file.
    /// </summary>
    public sealed class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private const short SizeRecord = 277;
        private const byte SizeStringProperty = 120;
        private readonly FileStream fileStream;
        private readonly Dictionary<string, List<long>> firstNameDictionary = new Dictionary<string, List<long>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, List<long>> lastNameDictionary = new Dictionary<string, List<long>>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary = new Dictionary<DateTime, List<long>>();
        private readonly Dictionary<int, long> dictionaryForId = new Dictionary<int, long>();
        private readonly IRecordValidator validator;
        private int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">Validation-rules.</param>
        /// <param name="fileStream">File stream.</param>
        /// <param name="startId">The id to start with.</param>
        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream, int? startId = null)
        {
            this.fileStream = fileStream ?? throw new ArgumentNullException(nameof(validator));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            if (startId is null)
            {
                this.id = (int)fileStream.Length / SizeRecord;
            }
            else
            {
                this.id = (int)startId;
            }

            IReadOnlyCollection<FileCabinetRecord> records = this.GetRecords();
            long position = 0;
            foreach (var record in records)
            {
                this.AddRecordToDictionaries(record, position);
                position += SizeRecord;
            }
        }

        /// <inheritdoc/>
        public int CreateRecord(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            dataRecord.Id = ++this.id;
            FileCabinetRecord record = this.Create(dataRecord);

            byte[] data = ConvertRecordToBytes(record);

            this.fileStream.Position = this.fileStream.Length;
            this.AddRecordToDictionaries(record, this.fileStream.Position);
            this.fileStream.Write(data, 0, data.Length);
            return record.Id;
        }

        /// <inheritdoc/>
        public void EditRecord(DataRecord dataRecord, long position)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            if (position < 0 || position > this.fileStream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            FileCabinetRecord record = this.Create(dataRecord);

            this.fileStream.Position = position;
            this.RemoveRecordFromDictionaries(record, this.fileStream.Position);
            this.AddRecordToDictionaries(record, this.fileStream.Position);

            byte[] data = ConvertRecordToBytes(record);
            this.fileStream.Write(data, 0, data.Length);
        }

        /// <inheritdoc/>
        public bool Remove(int id)
        {
            if (this.FindById(id) != -1)
            {
                long position = this.fileStream.Position;
                FileCabinetRecord record = this.ReadRecord();
                this.RemoveRecordFromDictionaries(record, position);

                this.fileStream.Position = position;
                short shift = sizeof(int);
                this.fileStream.Position += shift;
                bool deleted = true;
                byte[] data = BitConverter.GetBytes(deleted);
                this.fileStream.Write(data, 0, data.Length);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public int Purge()
        {
            this.fileStream.Position = 0;
            int count = 0;
            long newPosition = 0;
            using (FileStream tempStream = new FileStream("temp.db", FileMode.Create, FileAccess.ReadWrite))
            {
                while (this.fileStream.Position < this.fileStream.Length)
                {
                    long pos = this.fileStream.Position;
                    FileCabinetRecord record = this.ReadRecord();
                    if (record != null)
                    {
                        this.RemoveRecordFromDictionaries(record, pos);
                        this.AddRecordToDictionaries(record, newPosition);
                        newPosition += SizeRecord;

                        byte[] data = ConvertRecordToBytes(record);
                        tempStream.Write(data, 0, data.Length);
                    }
                    else
                    {
                        count++;
                    }
                }

                this.fileStream.SetLength(0);
                tempStream.Position = 0;
                tempStream.CopyTo(this.fileStream);
            }

            return count;
        }

        /// <inheritdoc/>
        public IRecordIterator FindByBirthDay(DateTime dateOfBirth)
        {
            List<long> positions = this.FindRecordsByDictionary(this.dateOfBirthDictionary, dateOfBirth);
            return new FileSystemIterator(this.fileStream, positions);
        }

        /// <inheritdoc/>
        public IRecordIterator FindByFirstName(string firstName)
        {
            List<long> positions = this.FindRecordsByDictionary(this.firstNameDictionary, firstName);
            return new FileSystemIterator(this.fileStream, positions);
        }

        /// <inheritdoc/>
        public IRecordIterator FindByLastname(string lastName)
        {
            List<long> positions = this.FindRecordsByDictionary(this.lastNameDictionary, lastName);
            return new FileSystemIterator(this.fileStream, positions);
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.fileStream.Position = 0;
            while (this.fileStream.Position < this.fileStream.Length)
            {
                FileCabinetRecord record = this.ReadRecord();
                if (record != null)
                {
                    records.Add(record);
                }
            }

            return records;
        }

        /// <inheritdoc/>
        public int GetCount()
        {
            return (int)this.fileStream.Length / SizeRecord;
        }

        /// <inheritdoc/>
        public int GetCountRemovedRecords()
        {
            short shift = sizeof(int);
            this.fileStream.Position = shift;
            int count = 0;
            while (this.fileStream.Position < this.fileStream.Length)
            {
                byte[] data = new byte[sizeof(bool)];
                this.fileStream.Read(data, 0, data.Length);
                bool isDeleted = BitConverter.ToBoolean(data, 0);
                if (isDeleted)
                {
                    count++;
                }

                this.fileStream.Position += SizeRecord - sizeof(bool);
            }

            return count;
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
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
                long position = this.FindById(item.Id);
                if (position != this.fileStream.Length)
                {
                    this.fileStream.Position = position;
                    FileCabinetRecord oldRecord = this.ReadRecord();
                    this.RemoveRecordFromDictionaries(oldRecord, position);
                }

                this.AddRecordToDictionaries(item, position);
                byte[] data = ConvertRecordToBytes(item);
                this.fileStream.Position = position;
                this.fileStream.Write(data, 0, data.Length);
            }
        }

        /// <inheritdoc/>
        public long FindById(int id)
        {
            return this.dictionaryForId.ContainsKey(id) ? this.dictionaryForId[id] : this.fileStream.Length;
        }

        /// <summary>
        /// Close current stream.
        /// </summary>
        public void Dispose()
        {
            this.fileStream.Close();
        }

        /// <summary>
        /// Adds record to dictionary by key.
        /// </summary>
        /// <typeparam name="T">Type key.</typeparam>
        /// <param name="dictionary">The dictionary in which to add the element.</param>
        /// <param name="key">The key by which the search will be.</param>
        /// <param name="position">Position record in file.</param>
        private static void AddRecordToDictionary<T>(Dictionary<T, List<long>> dictionary, T key, long position)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<long>();
            }

            dictionary[key].Add(position);
        }

        /// <summary>
        /// Removes record from dictionary by key.
        /// </summary>
        /// <typeparam name="T">Type key.</typeparam>
        /// <param name="dictionary">The dictionary in which to remove the element.</param>
        /// <param name="key">The key by which the search will be.</param>
        /// <param name="position">Position record in file.</param>
        private static void RemoveRecordFromDictiornary<T>(Dictionary<T, List<long>> dictionary, T key, long position)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Remove(position);
            }
        }

        /// <summary>
        /// Converts record to array of bytes.
        /// </summary>
        /// <param name="record">Record wich needs to convert.</param>
        /// <returns>Array of bytes.</returns>
        private static byte[] ConvertRecordToBytes(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            List<byte> data = new List<byte>();

            data.AddRange(BitConverter.GetBytes(record.Id));
            bool isDeleted = false;
            data.AddRange(BitConverter.GetBytes(isDeleted));

            byte[] dataForString = new byte[SizeStringProperty];
            Encoding.Default.GetBytes(record.FirstName, 0, record.FirstName.Length, dataForString, 0);
            data.AddRange(dataForString);

            dataForString = new byte[SizeStringProperty];
            Encoding.Default.GetBytes(record.LastName, 0, record.LastName.Length, dataForString, 0);
            data.AddRange(dataForString);

            data.AddRange(BitConverter.GetBytes(record.DateOfBirth.Year));
            data.AddRange(BitConverter.GetBytes(record.DateOfBirth.Month));
            data.AddRange(BitConverter.GetBytes(record.DateOfBirth.Day));

            data.AddRange(BitConverter.GetBytes(record.Access));

            data.AddRange(BitConverter.GetBytes(record.Age));
            int[] bits = decimal.GetBits(record.Salary);
            foreach (int bit in bits)
            {
                data.AddRange(BitConverter.GetBytes(bit));
            }

            return data.ToArray();
        }

        private List<long> FindRecordsByDictionary<T>(Dictionary<T, List<long>> dictionary, T key)
        {
            List<long> positions = new List<long>();
            if (dictionary.ContainsKey(key))
            {
                positions = dictionary[key];
            }

            return positions;
        }

        /// <summary>
        /// Adds a record to all dictionaries.
        /// </summary>
        /// <param name="record">The record which need to add.</param>
        private void AddRecordToDictionaries(FileCabinetRecord record, long position)
        {
            AddRecordToDictionary(this.firstNameDictionary, record.FirstName, position);
            AddRecordToDictionary(this.lastNameDictionary, record.LastName, position);
            AddRecordToDictionary(this.dateOfBirthDictionary, record.DateOfBirth, position);
            this.dictionaryForId.Add(record.Id, position);
        }

        /// <summary>
        /// Removes a record to all dictionaries.
        /// </summary>
        /// <param name="record">The record which need to remove.</param>
        private void RemoveRecordFromDictionaries(FileCabinetRecord record, long position)
        {
            RemoveRecordFromDictiornary(this.firstNameDictionary, record.FirstName, position);
            RemoveRecordFromDictiornary(this.lastNameDictionary, record.LastName, position);
            RemoveRecordFromDictiornary(this.dateOfBirthDictionary, record.DateOfBirth, position);
            this.dictionaryForId.Remove(record.Id);
        }

        private FileCabinetRecord ReadRecord()
        {
            byte[] data = new byte[SizeRecord];
            this.fileStream.Read(data, 0, SizeRecord);

            FileCabinetRecord record = new FileCabinetRecord();
            int position = 0;
            record.Id = BitConverter.ToInt32(data, position);
            position += sizeof(int);

            bool isDeleted = BitConverter.ToBoolean(data, position);
            position += sizeof(bool);
            if (isDeleted)
            {
                return null;
            }

            record.FirstName = Encoding.Default.GetString(data, position, SizeStringProperty).Trim('\0');
            position += SizeStringProperty;

            record.LastName = Encoding.Default.GetString(data, position, SizeStringProperty).Trim('\0');
            position += SizeStringProperty;

            int year = BitConverter.ToInt32(data, position);
            position += sizeof(int);
            int month = BitConverter.ToInt32(data, position);
            position += sizeof(int);
            int day = BitConverter.ToInt32(data, position);
            position += sizeof(int);
            record.DateOfBirth = new DateTime(year, month, day);

            record.Access = BitConverter.ToChar(data, position);
            position += sizeof(char);

            record.Age = BitConverter.ToInt16(data, position);
            position += sizeof(short);

            int[] bits = new int[4];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = BitConverter.ToInt32(data, position);
                position += sizeof(int);
            }

            record.Salary = new decimal(bits);

            return record;
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
    }
}

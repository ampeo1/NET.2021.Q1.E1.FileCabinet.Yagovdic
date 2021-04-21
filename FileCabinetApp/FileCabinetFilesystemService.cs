using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Representation file cabinet service in file.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const short SizeRecord = 276;
        private const byte SizeStringProperty = 120;
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
        private int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">Validation-rules.</param>
        /// <param name="fileStream">File stream.</param>
        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream)
        {
            this.fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.id = (int)fileStream.Length / SizeRecord;
        }

        /// <inheritdoc/>
        public int CreateRecord(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                throw new ArgumentNullException(nameof(dataRecord));
            }

            List<byte> data = new List<byte>();
            dataRecord.Id = ++this.id;
            FileCabinetRecord record = this.Create(dataRecord);

            data.AddRange(BitConverter.GetBytes(record.Id));

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

            this.fileStream.Position = this.fileStream.Length;
            this.fileStream.Write(data.ToArray(), 0, data.Count);
            return record.Id;
        }

        /// <inheritdoc/>
        public void EditRecord(DataRecord dataRecord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByBirthDay(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastname(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int FindIndexById(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.fileStream.Position = 0;
            while (this.fileStream.Position < this.fileStream.Length)
            {
                byte[] data = new byte[SizeRecord];
                this.fileStream.Read(data, 0, SizeRecord);

                FileCabinetRecord record = new FileCabinetRecord();
                int position = 0;
                record.Id = BitConverter.ToInt32(data, position);
                position += sizeof(int);

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
                records.Add(record);
            }

            return records;
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
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

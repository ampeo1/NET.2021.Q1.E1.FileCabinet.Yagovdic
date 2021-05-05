using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public sealed class EnumeratorFilesystem : IEnumerator<FileCabinetRecord>
    {
        private const int SizeRecord = 277;
        private const int SizeStringProperty = 120;
        private readonly FileStream fileStream;
        private readonly List<long> positions;
        private int index;
        private FileCabinetRecord currentRecord;

        public EnumeratorFilesystem(FileStream fileStream, List<long> positions)
        {
            this.fileStream = fileStream;
            this.positions = positions;
            this.index = -1;
        }

        public FileCabinetRecord Current
        {
            get
            {
                if (this.index < 0 || this.index > this.positions.Count)
                {
                    throw new InvalidOperationException();
                }

                return this.currentRecord;
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (++this.index < this.positions.Count)
            {
                this.fileStream.Position = this.positions[this.index];
                this.currentRecord = this.ReadRecord();

                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.index = -1;
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
    }
}

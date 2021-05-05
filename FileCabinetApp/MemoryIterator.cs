using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class MemoryIterator : IRecordIterator
    {
        private FileCabinetRecord[] records;
        private int position;

        public MemoryIterator(FileCabinetRecord[] records)
        {
            this.records = records;
            this.position = 0;
        }

        public FileCabinetRecord GetNext()
        {
            if (this.HasMore())
            {
                return this.records[this.position++];
            }

            throw new IndexOutOfRangeException();
        }

        public bool HasMore()
        {
            return this.position < this.records.Length;
        }

        public void Reset()
        {
            this.position = 0;
        }
    }
}

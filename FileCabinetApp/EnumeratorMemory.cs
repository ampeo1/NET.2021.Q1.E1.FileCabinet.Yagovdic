using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class EnumeratorMemory : IEnumerator<FileCabinetRecord>
    {
        private int position;
        private readonly FileCabinetRecord[] records;
        private FileCabinetRecord currentRecord;

        public EnumeratorMemory(FileCabinetRecord[] records)
        {
            this.records = records;
            this.position = -1;
            this.currentRecord = null;
        }

        public FileCabinetRecord Current
        {
            get
            {
                if (this.position < 0 || this.position > this.records.Length)
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
            if (++this.position < this.records.Length)
            {
                this.currentRecord = this.records[this.position];
                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.position = -1;
            this.currentRecord = null;
        }
    }
}

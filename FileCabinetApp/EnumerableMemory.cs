using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class EnumerableMemory : IEnumerable<FileCabinetRecord>
    {
        private FileCabinetRecord[] records;

        public EnumerableMemory(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new EnumeratorMemory(this.records);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EnumeratorMemory(this.records);
        }
    }
}

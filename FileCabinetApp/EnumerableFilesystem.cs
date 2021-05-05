using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public class EnumerableFilesystem : IEnumerable<FileCabinetRecord>
    {
        private FileStream fileStream;
        private List<long> positions;

        public EnumerableFilesystem(FileStream fileStream, List<long> positions)
        {
            this.fileStream = fileStream;
            this.positions = positions;
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new EnumeratorFilesystem(this.fileStream, positions);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EnumeratorFilesystem(this.fileStream, positions);
        }
    }
}

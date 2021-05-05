using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Print interface.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints records.
        /// </summary>
        /// <param name="records">Records to be printed.</param>
        public void Print(IEnumerable<FileCabinetRecord> records);

        /// <summary>
        /// Prints records.
        /// </summary>
        /// <param name="iterator">Iterator for records.</param>
        public void PrintForIterator(IRecordIterator iterator);
    }
}

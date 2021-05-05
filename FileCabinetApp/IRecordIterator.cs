using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface iterator.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Gets next FileCabinetRecord.
        /// </summary>
        /// <returns>FileCabinerRecord.</returns>
        public FileCabinetRecord GetNext();

        /// <summary>
        /// Checks if there is a next item.
        /// </summary>
        /// <returns>True if has; otherwise false.</returns>
        public bool HasMore();

        /// <summary>
        /// Back to the beginning.
        /// </summary>
        public void Reset();
    }
}

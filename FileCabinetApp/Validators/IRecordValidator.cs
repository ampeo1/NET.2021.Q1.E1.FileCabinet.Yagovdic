using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Validate record.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validate parameters.
        /// </summary>
        /// <param name="dataRecord">Checked arguments.</param>
        public void ValidateParameters(DataRecord dataRecord);

    }
}

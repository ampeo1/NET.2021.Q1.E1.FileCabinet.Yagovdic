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
        private const string FileName = "cabinet-records.db";
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">Validation-rules.</param>
        /// <param name="fileStream">File stream.</param>
        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream)
        {
            this.fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <inheritdoc/>
        public int CreateRecord(DataRecord dataRecord)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}

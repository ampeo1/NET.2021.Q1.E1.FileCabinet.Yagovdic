using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Representation file cabinet service.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="dataRecord">record data.</param>
        /// <exception cref="ArgumentNullException">Trows when <paramref name="dataRecord"/> is null.</exception>
        /// <exception cref="ArgumentException">Trows when data is invalid.</exception>
        /// <returns>Record id.</returns>
        public int CreateRecord(DataRecord dataRecord);

        /// <summary>
        /// Changes record.
        /// </summary>
        /// <exception cref="ArgumentNullException">Trows when <paramref name="dataRecord"/> is null.</exception>
        /// <exception cref="ArgumentException">Trows when data is invalid.</exception>
        /// <param name="dataRecord">Record data.</param>
        public void EditRecord(DataRecord dataRecord);

        /// <summary>
        /// Finds index record by id.
        /// </summary>
        /// <param name="id">Identifier of the searched record.</param>
        /// <exception cref="ArgumentException">Throws when record not found.</exception>
        /// <returns>Index record.</returns>
        public int FindIndexById(int id);

        /// <summary>
        /// Finds record by first name.
        /// </summary>
        /// <param name="firstName">First name of the searched record.</param>
        /// <returns>Found records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds record by last name.
        /// </summary>
        /// <param name="lastName">Last name of the searched record.</param>
        /// <returns>Found records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastname(string lastName);

        /// <summary>
        /// Finds record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">date of birth of the searched record.</param>
        /// <returns>Found records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByBirthDay(DateTime dateOfBirth);

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>Records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat();

        /// <summary>
        /// Gets validator.
        /// </summary>
        /// <returns>Validator.</returns>
        public IRecordValidator GetValidator();

        /// <summary>
        /// Saves сurrent state.
        /// </summary>
        /// <returns>State.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Updates records.
        /// </summary>
        /// <param name="snapshot">The state to be updated.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);
    }
}

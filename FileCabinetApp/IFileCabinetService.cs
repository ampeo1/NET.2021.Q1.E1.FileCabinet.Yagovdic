using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Validators;

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
        /// Finds position of record by id.
        /// </summary>
        /// <param name="id">Identifier of the searched record.</param>
        /// <returns>if record finds that return position of record else return -1.</returns>
        public FileCabinetRecord FindById(int id);

        /// <summary>
        /// Select records.
        /// </summary>
        /// <param name="properties">Properties to search.</param>
        /// <param name="record">Record to search.</param>
        /// <returns>Record Iterator.</returns>
        public IEnumerable<FileCabinetRecord> SelectRecords(PropertyInfo[][] properties, FileCabinetRecord[] record);

        /// <summary>
        /// Finds record by first name.
        /// </summary>
        /// <param name="firstName">First name of the searched record.</param>
        /// <returns>Found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds record by last name.
        /// </summary>
        /// <param name="lastName">Last name of the searched record.</param>
        /// <returns>Found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastname(string lastName);

        /// <summary>
        /// Finds record by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">date of birth of the searched record.</param>
        /// <returns>Found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByBirthDay(DateTime dateOfBirth);

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>Records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetCount();

        /// <summary>
        /// Gets count deleted records.
        /// </summary>
        /// <returns>Count deleted records.</returns>
        public int GetCountRemovedRecords();

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

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Id of the record to be deleted.</param>
        /// <returns>True if the record has been deleted; otherwise false.</returns>
        public bool Remove(int id);

        /// <summary>
        /// Defragments a file.
        /// </summary>
        /// <returns>Count deleted records.</returns>
        public int Purge();
    }
}

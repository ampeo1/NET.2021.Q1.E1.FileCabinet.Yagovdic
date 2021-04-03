using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        public int CreateRecord(DataRecord dataRecord);

        public void EditRecord(DataRecord dataRecord);

        public int FindIndexById(int id);

        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        public IReadOnlyCollection<FileCabinetRecord> FindByLastname(string lastName);

        public IReadOnlyCollection<FileCabinetRecord> FindByBirthDay(DateTime dateOfBirth);

        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        public int GetStat();

        public IRecordValidator GetValidator();
    }
}

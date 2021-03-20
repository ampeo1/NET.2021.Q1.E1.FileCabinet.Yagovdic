using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public static int CreateRecord(string firstName, string lastName, DateTime dateOfBirth)
        {
            // TODO: добавьте реализацию метода
            return 0;
        }

        public static FileCabinetRecord[] GetRecords()
        {
            // TODO: добавьте реализацию метода
            return Array.Empty<FileCabinetRecord>();
        }

        public static int GetStat()
        {
            // TODO: добавьте реализацию метода
            return 0;
        }
    }
}

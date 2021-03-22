using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id
        {
            get; set;
        }

        public string FirstName
        {
            get; set;
        }

        public string LastName
        {
            get; set;
        }

        public DateTime DateOfBirth
        {
            get; set;
        }

        public char Access
        {
            get; set;
        }

        public short Age
        {
            get; set;
        }

        public long AmountRecords
        {
            get; set;
        }
    }
}

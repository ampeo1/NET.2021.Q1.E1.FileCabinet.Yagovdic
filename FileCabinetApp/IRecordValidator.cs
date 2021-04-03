using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        public void ValidateParameters(DataRecord dataRecord);

        public Tuple<bool, string> ValidateFirstName(string firstName);

        public Tuple<bool, string> ValidateLastName(string lastName);

        public Tuple<bool, string> ValidateAccess(char access);

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth);
    }
}

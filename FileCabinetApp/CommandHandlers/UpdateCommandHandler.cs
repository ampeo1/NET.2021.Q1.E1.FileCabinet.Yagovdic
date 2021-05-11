using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class UpdateCommandHandler : CRUDCommandHandlerBase
    {
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        protected override string NameCommand => "update";

        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            string parameters = command.Parameters;

            if (!parameters.StartsWith("set", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Invalid format. Doesn't start with set.");
                return;
            }

            parameters = parameters.Remove(0, 3);
            string[] splitParameters = parameters.Split("where", StringSplitOptions.RemoveEmptyEntries);

            if (splitParameters.Length != 2)
            {
                Console.WriteLine("Invalid format. Doesn't contain where.");
            }

            IEnumerable<FileCabinetRecord> records;
            Dictionary<PropertyInfo, object> newRecordProperty;
            try
            {
                string separator = ",";
                string[] nameProperties, valueProperties;
                (nameProperties, valueProperties) = SplitParameters(splitParameters[0], separator);
                newRecordProperty = ParseParameters(nameProperties, valueProperties);
                separator = "and";
                (nameProperties, valueProperties) = SplitParameters(splitParameters[1], separator);
                var soughtProperties = ParseParameters(nameProperties, valueProperties);
                records = this.FindRecords(soughtProperties);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            foreach (var record in records)
            {
                DataRecord dataRecord = ConvertFileCabinetRecordToDataRecord(record);

                foreach (var property in newRecordProperty.Keys)
                {
                    object value = newRecordProperty[property];
                    typeof(DataRecord).GetProperty(property.Name).SetValue(dataRecord, value);
                }

                dataRecord.Id = record.Id;

                try
                {
                    this.service.EditRecord(dataRecord);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static DataRecord ConvertFileCabinetRecordToDataRecord(FileCabinetRecord record)
        {
            DataRecord dataRecord = new DataRecord
            {
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                Access = record.Access,
                Salary = record.Salary,
            };

            return dataRecord;
        }
    }
}

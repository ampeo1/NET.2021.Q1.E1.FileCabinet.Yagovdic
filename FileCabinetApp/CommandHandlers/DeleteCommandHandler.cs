using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class DeleteCommandHandler : CRUDCommandHandlerBase
    {
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        protected override string NameCommand => "delete";

        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            string parameters = command.Parameters;

            if (!parameters.StartsWith("where", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Invalid format. Doesn't start with wehre.");
                return;
            }

            parameters = parameters.Remove(0, 5);
            string[] nameProperties, valueProperties;

            Dictionary<PropertyInfo, object> properties;
            try
            {
                (nameProperties, valueProperties) = SplitWhereParameters(parameters);
                properties = ParseParameters(nameProperties, valueProperties);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            IEnumerable<FileCabinetRecord> records = this.FindRecords(properties);

            List<int> ids = new List<int>();
            foreach (var record in records)
            {
                ids.Add(record.Id);
                this.service.Remove(record.Id);
            }

            Console.WriteLine(GetMessage(ids));
        }

        private static string GetMessage(List<int> ids)
        {
            if (ids.Count == 0)
            {
                return "Records not found.";
            }

            if (ids.Count == 1)
            {
                return $"Record #{ids[0]} is deleted.";
            }

            StringBuilder message = new StringBuilder();
            for (int i = 0; i < ids.Count; i++)
            {
                if (i == 0)
                {
                    message.Append($"Records #{ids[i]}");
                    continue;
                }

                message.Append($", #{ids[i]}");
            }

            message.Append(" are deleted.");
            return message.ToString();
        }
    }
}

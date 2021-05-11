using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Delete command.
    /// </summary>
    public class DeleteCommandHandler : CRUDCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service">IFileCabinetService.</param>
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Gets name command.
        /// </summary>
        /// <value>
        /// Name command.
        /// </value>
        protected override string NameCommand => "delete";

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">Command info.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (this.GoToNextCommand(command))
            {
                return;
            }

            string parameters = command.Parameters;

            if (!parameters.StartsWith("where", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Invalid format. Doesn't start with where.");
                return;
            }

            parameters = parameters.Remove(0, 5);
            string[] nameProperties, valueProperties;

            Dictionary<PropertyInfo, object> properties;
            IEnumerable<FileCabinetRecord> records;
            string separator = "and";
            try
            {
                (nameProperties, valueProperties) = SplitParameters(parameters, separator);
                properties = ParseParameters(nameProperties, valueProperties);
                records = this.FindRecords(properties);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

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

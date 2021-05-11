using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Insert command.
    /// </summary>
    public class InsertCommandHandler : CRUDCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">IFileCabinetService.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Gets name command.
        /// </summary>
        /// <value>
        /// Name command.
        /// </value>
        protected override string NameCommand => "insert";

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">Command info.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!command.Parameters.Contains("values", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            string[] splitParameters = command.Parameters.Split("values", 2);
            if (splitParameters.Length != 2)
            {
                throw new ArgumentException("Invalid parameters.");
            }

            int indexForNameProperties = 0;
            int indexForValueProperties = 1;
            Dictionary<PropertyInfo, object> properties;
            try
            {
                string[] nameProperties = SplitBrackets(splitParameters[indexForNameProperties]);
                string[] valueProperties = SplitBrackets(splitParameters[indexForValueProperties]);
                properties = ParseParameters(nameProperties, valueProperties);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            DataRecord dataRecord = new DataRecord();
            foreach (var property in properties.Keys)
            {
                object value = properties[property];
                typeof(DataRecord).GetProperty(property.Name).SetValue(dataRecord, value);
            }

            try
            {
                this.service.CreateRecord(dataRecord);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

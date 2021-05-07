using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        protected override string NameCommand => "insert";

        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            Dictionary<PropertyInfo, string> properties = ParseParameters(command.Parameters);
            DataRecord dataRecord = new DataRecord();
            char[] charsToTrim = new char[] { '\'', ' ' };
            foreach (var property in properties.Keys)
            {
                object value;
                if (property.PropertyType != typeof(DateTime))
                {
                    try
                    {
                        value = Convert.ChangeType(properties[property].Trim(charsToTrim), property.PropertyType, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Incorrect format.");
                        return;
                    }
                }
                else
                {
                    string temp = properties[property].Trim(charsToTrim);
                    if (!DateTime.TryParseExact(properties[property].Trim(charsToTrim), "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
                    {
                        Console.WriteLine("Date invalid format. dd/mm/yyyy.");
                        return;
                    }

                    value = date;
                }

                property.SetValue(dataRecord, value);
            }

            this.service.CreateRecord(dataRecord);
        }

        private static Dictionary<PropertyInfo, string> ParseParameters(string parameters)
        {
            if (!parameters.Contains("values", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Invalid parameters. Doesn't contain 'values'.");
            }

            string[] splitParameters = parameters.Split("values", 2);
            if (splitParameters.Length != 2)
            {
                throw new ArgumentException("Invalid parameters.");
            }

            int indexForNameProperties = 0;
            int indexForValueProperties = 1;
            char separator = ',';
            char[] charsToTrim = new char[] { '(', ')', ' ' };
            string[] nameProperties = splitParameters[indexForNameProperties].Trim(charsToTrim).Split(separator);
            string[] valueProperties = splitParameters[indexForValueProperties].Trim(charsToTrim).Split(separator);
            if (nameProperties.Length != valueProperties.Length)
            {
                throw new ArgumentException("Invalid parameters. The number of properties is not equal to the number of values.");
            }

            Dictionary<PropertyInfo, string> properties = new Dictionary<PropertyInfo, string>();
            for (int i = 0; i < nameProperties.Length; i++)
            {
                PropertyInfo property = typeof(DataRecord).GetProperty(nameProperties[i].Trim(' '), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property is null)
                {
                    throw new ArgumentException($"Invalid parameters. No property named {nameProperties[i]}");
                }

                properties.Add(property, valueProperties[i]);
            }

            return properties;
        }
    }
}

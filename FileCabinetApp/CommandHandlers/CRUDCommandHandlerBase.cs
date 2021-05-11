using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base class for CRUD classes.
    /// </summary>
    public abstract class CRUDCommandHandlerBase : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CRUDCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">IFileCabinetService.</param>
        protected CRUDCommandHandlerBase(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Adds a property to the dictionary and binds a value to it.
        /// </summary>
        /// <param name="nameProperties">Property names.</param>
        /// <param name="valueProperties">Property values.</param>
        /// <returns>Dictionary with property info and value this property.</returns>
        protected static Dictionary<PropertyInfo, object> ParseParameters(string[] nameProperties, string[] valueProperties)
        {
            if (nameProperties is null || valueProperties is null)
            {
                throw new ArgumentNullException($"{nameof(nameProperties)}, {nameof(valueProperties)}");
            }

            if (nameProperties.Length != valueProperties.Length)
            {
                throw new ArgumentException("Invalid parameters. The number of properties is not equal to the number of values.");
            }

            Dictionary<PropertyInfo, object> properties = new Dictionary<PropertyInfo, object>();
            for (int i = 0; i < nameProperties.Length; i++)
            {
                PropertyInfo property = typeof(FileCabinetRecord).GetProperty(nameProperties[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property is null)
                {
                    throw new ArgumentException($"Invalid parameters. No property named {nameProperties[i]}");
                }

                object value = ParseValue(valueProperties[i], property.PropertyType);
                properties.Add(property, value);
            }

            return properties;
        }

        /// <summary>
        /// Separates parameters in parentheses.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <returns>Split parameters.</returns>
        protected static string[] SplitBrackets(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(parameters);
            }

            char separator = ',';
            char[] charsToTrim = new char[] { '(', ')', ' ', '\'' };
            string[] splitParameters = parameters.Split(separator);
            for (int i = 0; i < splitParameters.Length; i++)
            {
                splitParameters[i] = splitParameters[i].Trim(charsToTrim);
            }

            return splitParameters;
        }

        /// <summary>
        /// Separates parameters that are in the form "property = value".
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <param name="separator">Separator.</param>
        /// <returns>(Properties, Values).</returns>
        protected static (string[], string[]) SplitParameters(string parameters, string separator)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            string[] splitParameters = parameters.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            char trimForName = ' ';
            char[] trimForValue = new char[] { ' ', '\'' };
            List<string> nameProperties = new List<string>();
            List<string> valueProperties = new List<string>();
            for (int i = 0; i < splitParameters.Length; i++)
            {
                string[] splitParameter = splitParameters[i].Split('=', 2);
                if (splitParameter.Length != 2)
                {
                    throw new ArgumentException("Invalid format. The number of properties is not equal to the number of values.");
                }

                int indexNameProperty = 0;
                int indexValueProperty = 1;
                nameProperties.Add(splitParameter[indexNameProperty].Trim(trimForName));
                valueProperties.Add(splitParameter[indexValueProperty].Trim(trimForValue));
            }

            return (nameProperties.ToArray(), valueProperties.ToArray());
        }

        /// <summary>
        /// Parse string.
        /// </summary>
        /// <param name="value">The string to be parsed.</param>
        /// <param name="type">The type to parse into.</param>
        /// <returns>Parsed object.</returns>
        protected static object ParseValue(string value, Type type)
        {
            if (type != typeof(DateTime))
            {
                try
                {
                    return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid type property.");
                    return null;
                }
            }
            else
            {
                if (!DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    Console.WriteLine("Date invalid format. dd/MM/yyyy.");
                    return null;
                }

                return date;
            }
        }

        /// <summary>
        /// Finds records by properties.
        /// </summary>
        /// <param name="properties">Properties.</param>
        /// <returns>Found records.</returns>
        protected IEnumerable<FileCabinetRecord> FindRecords(Dictionary<PropertyInfo, object> properties)
        {
            if (properties is null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            foreach (var record in this.service.GetRecords())
            {
                bool result = true;
                foreach (var property in properties.Keys)
                {
                    object valueProperty = property.GetValue(record);
                    if (!valueProperty.Equals(properties[property]))
                    {
                        result = false;
                        break;
                    }
                }

                if (result)
                {
                    yield return record;
                }
            }
        }
    }
}

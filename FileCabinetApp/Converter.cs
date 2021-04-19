using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// This class converts data.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Converts from string to string.
        /// </summary>
        /// <param name="str">String representation.</param>
        /// <returns>
        /// First argument: True if successfully transformed; otherwise false.
        /// Second argument: Error message if there is an error.
        /// Third argument: Converted value.
        /// </returns>
        public static Tuple<bool, string, string> StringConverter(string str)
        {
            return new Tuple<bool, string, string>(true, string.Empty, str);
        }

        /// <summary>
        /// Converts from string to DateTime.
        /// </summary>
        /// <param name="str">String representation.</param>
        /// <returns>
        /// First argument: True if successfully transformed; otherwise false.
        /// Second argument: Error message if there is an error.
        /// Third argument: Converted value.
        /// </returns>
        public static Tuple<bool, string, DateTime> DateConverter(string str)
        {
            DateTime date = new DateTime(0);
            bool converted = false;
            string errorMessage = string.Empty;
            try
            {
                date = DateTime.ParseExact(str, "dd/MM/yyyy", null, DateTimeStyles.None);
                converted = true;
            }
            catch (ArgumentNullException)
            {
                errorMessage = "Argument is null or empty";
            }
            catch (ArgumentException)
            {
                errorMessage = "Argument is not valid";
            }
            catch (FormatException)
            {
                errorMessage = "Argument must be format dd/mm/yyyy";
            }

            return new Tuple<bool, string, DateTime>(converted, errorMessage, date);
        }

        /// <summary>
        /// Converts from string to char.
        /// </summary>
        /// <param name="str">String representation.</param>
        /// <returns>
        /// First argument: True if successfully transformed; otherwise false.
        /// Second argument: Error message if there is an error.
        /// Third argument: Converted value.
        /// </returns>
        public static Tuple<bool, string, char> CharConverted(string str)
        {
            char convertedValue = char.MinValue;
            string errorMessage = string.Empty;
            bool converted = false;
            try
            {
                convertedValue = char.Parse(str);
                converted = true;
            }
            catch (ArgumentNullException)
            {
                errorMessage = "Argument is null or empty";
            }
            catch (FormatException)
            {
                errorMessage = "Must be one character";
            }

            return new Tuple<bool, string, char>(converted, errorMessage, convertedValue);
        }

        /// <summary>
        /// Converts from string to decimal.
        /// </summary>
        /// <param name="str">String representation.</param>
        /// <returns>
        /// First argument: True if successfully transformed; otherwise false.
        /// Second argument: Error message if there is an error.
        /// Third argument: Converted value.
        /// </returns>
        public static Tuple<bool, string, decimal> DecimalConverted(string str)
        {
            decimal convertedValue = 0;
            string errorMessage = string.Empty;
            bool converted = false;
            try
            {
                convertedValue = decimal.Parse(str, CultureInfo.InvariantCulture);
                converted = true;
            }
            catch (ArgumentNullException)
            {
                errorMessage = "Argument is null or empty";
            }
            catch (FormatException)
            {
                errorMessage = "Must be one character";
            }
            catch (OverflowException)
            {
                errorMessage = "Overflow has occurred";
            }

            return new Tuple<bool, string, decimal>(converted, errorMessage, convertedValue);
        }
    }
}

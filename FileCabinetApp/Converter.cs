using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class Converter
    {
        public static Tuple<bool, string, string> StringConverter(string str)
        {
            return new Tuple<bool, string, string>(true, string.Empty, str);
        }

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
    }
}

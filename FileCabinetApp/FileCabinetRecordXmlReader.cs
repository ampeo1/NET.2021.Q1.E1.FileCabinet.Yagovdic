using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for reading records from xml file.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private readonly XmlReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">Reader.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = XmlReader.Create(reader);
        }

        /// <summary>
        /// Reads all records from xml file.
        /// </summary>
        /// <returns>Records which contains in file.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));

            try
            {
                return (FileCabinetRecord[])serializer.Deserialize(this.reader);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Failed to read data.");
            }

            return Array.Empty<FileCabinetRecord>();
        }
    }
}

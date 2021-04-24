using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// A class that writes data to a xml file.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private const string RecordElement = "record";
        private const string IdAttribute = "id";
        private const string NameElement = "name";
        private const string FirstNameAttribute = "first";
        private const string LastNameAttribute = "last";
        private const string DateOfBirthElement = "dateOfBirth";
        private const string AgeAttribute = "age";
        private const string AmountRecordsElement = "salary";
        private const string AccessElement = "access";
        private StreamWriter writer;
        private XmlSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Provider for writting.</param>
        /// <param name="serializer">Serializer for xml.</param>
        public FileCabinetRecordXmlWriter(StreamWriter writer, XmlSerializer serializer)
        {
            this.writer = writer;
            this.serializer = serializer;
        }

        /// <summary>
        /// Writes data in xml file.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="records"/> is null.</exception>
        /// <param name="records">The records to be saved.</param>
        public void Writer(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{records}");
            }

            this.serializer.Serialize(this.writer, records);
        }
    }
}

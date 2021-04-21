using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        private XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Provider for writting.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Writes data in xml file.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="record"/> is null.</exception>
        /// <param name="record">The record to be saved.</param>
        public void Writer(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{record}");
            }

            this.writer.WriteStartElement(RecordElement);
            this.writer.WriteAttributeString(IdAttribute, record.Id.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteStartElement(NameElement);
            this.writer.WriteAttributeString(FirstNameAttribute, record.FirstName);
            this.writer.WriteAttributeString(LastNameAttribute, record.LastName);
            this.writer.WriteEndElement();
            this.writer.WriteStartElement(DateOfBirthElement);
            this.writer.WriteAttributeString(AgeAttribute, record.Age.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteString(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();
            this.writer.WriteStartElement(AmountRecordsElement);
            this.writer.WriteString(record.Salary.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();
            this.writer.WriteStartElement(AccessElement);
            this.writer.WriteString(record.Access.ToString());
            this.writer.WriteEndElement();
            this.writer.WriteEndElement();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Сlass that saves state.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">File cabiner records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
            this.records = Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <value>
        /// Records.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records => Array.AsReadOnly(this.records);

        /// <summary>
        /// Writes data in Csv file.
        /// </summary>
        /// <param name="streamWriter">Provider for writing.</param>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>
        /// Writes data in xml file.
        /// </summary>
        /// <param name="streamWriter">Provider for writing.</param>
        public void SaveToXml(StreamWriter streamWriter)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            FileCabinetRecordXmlWriter xmlWriter = new FileCabinetRecordXmlWriter(streamWriter, serializer);
            xmlWriter.Writer(this.records);
        }

        /// <summary>
        /// Load records from csv file.
        /// </summary>
        /// <param name="reader">Stream Reader.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            FileCabinetRecordCsvReader csvReader = new FileCabinetRecordCsvReader(reader);
            this.records = csvReader.ReadAll().ToArray();
        }

        /// <summary>
        /// Load records from xml file.
        /// </summary>
        /// <param name="reader">Stream Reader.</param>
        public void LoadFromXml(StreamReader reader)
        {
            FileCabinetRecordXmlReader xmlReader = new FileCabinetRecordXmlReader(reader);
            this.records = xmlReader.ReadAll().ToArray();
        }
    }
}

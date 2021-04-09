using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Сlass that saves state.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Data to be written.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

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
            using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("records");
                FileCabinetRecordXmlWriter writer = new FileCabinetRecordXmlWriter(xmlWriter);
                foreach (var record in this.records)
                {
                    writer.Writer(record);
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }
    }
}

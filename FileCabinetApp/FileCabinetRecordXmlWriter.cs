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

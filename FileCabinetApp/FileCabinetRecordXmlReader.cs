using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlReader
    {
        private readonly XmlReader reader;

        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = XmlReader.Create(reader);
        }

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

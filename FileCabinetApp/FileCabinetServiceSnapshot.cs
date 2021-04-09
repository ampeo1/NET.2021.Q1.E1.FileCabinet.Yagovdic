﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        public void SaveToCsv(StreamWriter streamWriter)
        {
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }
    }
}

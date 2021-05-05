using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Decorator for FileCabinetService. Logger.
    /// </summary>
    public class ServiceLogger : IFileCabinetService
    {
        private IFileCabinetService service;
        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        /// <param name="path">The path where the logs will be saved, if null then to the current directory.</param>
        public ServiceLogger(IFileCabinetService service, string path = null)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (string.IsNullOrEmpty(path))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(Directory.GetCurrentDirectory());
                builder.Append(@"\Logs.txt");
                path = builder.ToString();
            }

            this.service = service;
            this.path = path;
        }

        /// <inheritdoc/>
        public int CreateRecord(DataRecord dataRecord)
        {
            if (dataRecord is null)
            {
                string log = $"Calling {nameof(this.CreateRecord)} with {nameof(ArgumentNullException)} {nameof(dataRecord)} is null";
                this.WriteLog(log);
                throw new ArgumentNullException(nameof(dataRecord));
            }

            StringBuilder builder = new StringBuilder();
            builder.Append($"Calling {nameof(this.CreateRecord)} with ");
            builder.Append(ConvertClassToString(dataRecord));
            this.WriteLog(builder.ToString());

            int id = this.service.CreateRecord(dataRecord);

            this.WriteLog($"{nameof(this.CreateRecord)} returned '{id}'");

            return id;
        }

        /// <inheritdoc/>
        public void EditRecord(DataRecord dataRecord, long position)
        {
            if (dataRecord is null)
            {
                string log = $"Calling {nameof(this.EditRecord)} with {nameof(ArgumentNullException)} {nameof(dataRecord)} is null";
                this.WriteLog(log);
                throw new ArgumentNullException(nameof(dataRecord));
            }

            StringBuilder builder = new StringBuilder();
            builder.Append($"Calling {nameof(this.EditRecord)} with {ConvertClassToString(dataRecord)}, {nameof(position)} = '{position}'");

            this.WriteLog(builder.ToString());

            this.service.EditRecord(dataRecord, position);
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByBirthDay(DateTime dateOfBirth)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Calling {nameof(this.FindByBirthDay)} with {nameof(dateOfBirth)} = '{dateOfBirth:dd/MM/yyyy}'");
            this.WriteLog(builder.ToString());

            var records = this.service.FindByBirthDay(dateOfBirth);

            builder = new StringBuilder();
            builder.Append($"{nameof(this.FindByBirthDay)} returned [ ");
            int i = 0;
            foreach (var record in records)
            {
                builder.Append($"{i}. {ConvertClassToString(record)}");
                i++;
            }

            builder.Append(" ]");
            this.WriteLog(builder.ToString());

            return records;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Calling {nameof(this.FindByFirstName)} with {nameof(firstName)} = '{firstName}'");
            this.WriteLog(builder.ToString());

            var records = this.service.FindByFirstName(firstName);

            builder = new StringBuilder();
            builder.Append($"{nameof(this.FindByFirstName)} returned [ ");
            int i = 0;
            foreach (var record in records)
            {
                builder.Append($"{i}. {ConvertClassToString(record)}");
                i++;
            }

            builder.Append(" ]");
            this.WriteLog(builder.ToString());

            return records;
        }

        /// <inheritdoc/>
        public long FindById(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Calling {nameof(this.FindById)} with {nameof(id)} = '{id}'");
            this.WriteLog(builder.ToString());

            var position = this.service.FindById(id);

            builder = new StringBuilder();
            builder.Append($"{nameof(this.FindById)} returned '{position}'");
            this.WriteLog(builder.ToString());

            return position;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastname(string lastName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Calling {nameof(this.FindByLastname)} with {nameof(lastName)} = {lastName}");
            this.WriteLog(builder.ToString());

            var records = this.service.FindByLastname(lastName);

            builder = new StringBuilder();
            builder.Append($"{nameof(this.FindByLastname)} returned [ ");
            int i = 0;
            foreach (var record in records)
            {
                builder.Append($"{i}. {ConvertClassToString(record)}");
                i++;
            }

            builder.Append(" ]");
            this.WriteLog(builder.ToString());

            return records;
        }

        /// <inheritdoc/>
        public int GetCount()
        {
            string log = $"Calling {nameof(this.GetCount)}";
            this.WriteLog(log);

            var count = this.service.GetCount();

            log = $"{nameof(this.GetCount)} returned '{count}'";
            this.WriteLog(log);

            return count;
        }

        /// <inheritdoc/>
        public int GetCountRemovedRecords()
        {
            string log = $"Calling {nameof(this.GetCountRemovedRecords)}";
            this.WriteLog(log);

            var count = this.service.GetCountRemovedRecords();

            log = $"{nameof(this.GetCountRemovedRecords)} returned '{count}'";
            this.WriteLog(log);

            return count;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            string log = $"Calling {nameof(this.GetRecords)}";
            this.WriteLog(log);

            var records = this.service.GetRecords().ToList();

            StringBuilder builder = new StringBuilder();
            builder.Append($"{nameof(this.GetRecords)} returned [ ");
            for (int i = 0; i < records.Count; i++)
            {
                builder.Append($"{i}. {ConvertClassToString(records[i])}");
            }

            builder.Append(" ]");
            this.WriteLog(builder.ToString());

            return records;
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            string log = $"Calling {nameof(this.GetValidator)}";
            this.WriteLog(log);

            var validator = this.service.GetValidator();

            log = $"{nameof(this.GetValidator)} returned '{validator.GetType()}'";
            this.WriteLog(log);

            return validator;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            string log = $"Calling {nameof(this.MakeSnapshot)}";
            this.WriteLog(log);

            var snapshot = this.service.MakeSnapshot();

            StringBuilder builder = new StringBuilder();
            builder.Append($"{nameof(this.GetRecords)} returned [ ");
            for (int i = 0; i < snapshot.Records.Count; i++)
            {
                builder.Append($"{i}. {ConvertClassToString(snapshot.Records[i])}");
            }

            builder.Append(" ]");
            this.WriteLog(builder.ToString());

            return snapshot;
        }

        /// <inheritdoc/>
        public int Purge()
        {
            string log = $"Calling {nameof(this.Purge)}";
            this.WriteLog(log);

            var count = this.service.Purge();

            log = $"{nameof(this.Purge)} returned '{count}'";
            this.WriteLog(log);

            return count;
        }

        /// <inheritdoc/>
        public bool Remove(int id)
        {
            string log = $"Calling {nameof(this.Remove)} with {nameof(id)} = '{id}'";
            this.WriteLog(log);

            var result = this.service.Remove(id);

            log = $"{nameof(this.Remove)} returned '{result}'";
            this.WriteLog(log);

            return result;
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            string log;
            if (snapshot is null)
            {
                log = $"Calling {nameof(this.Restore)} {nameof(ArgumentNullException)} {nameof(snapshot)} is null";
                this.WriteLog(log);
                throw new ArgumentNullException(nameof(snapshot));
            }

            log = $"Calling {nameof(this.Restore)} with {nameof(snapshot)} = '{snapshot.Records.Count}'";
            this.WriteLog(log);

            this.service.Restore(snapshot);

            log = $"{nameof(this.Restore)}";
            this.WriteLog(log);
        }

        private static string ConvertClassToString<T>(T record)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            StringBuilder builder = new StringBuilder();
            foreach (var property in properties)
            {
                builder.Append($"{property.Name} = '{property.GetValue(record)}' ");
            }

            return builder.ToString();
        }

        private void WriteLog(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(DateTime.Now.ToString("g", CultureInfo.CurrentCulture));
            builder.Append(" - ");
            builder.Append(text);
            builder.Append('\n');

            using (FileStream fileStream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fileStream.Position = fileStream.Length;
                byte[] data = Encoding.Default.GetBytes(builder.ToString());
                fileStream.Write(data, 0, data.Length);
            }
        }
    }
}

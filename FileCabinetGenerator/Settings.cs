using System;
using System.IO;
using FileCabinetApp;
using FileCabinetApp.Validators;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Settings for Program.
    /// </summary>
    internal class Settings
    {
        /// <summary>
        /// Name storage for file system cabient.
        /// </summary>
        public const string FileNameStorage = "FileCabinetGenerator.db";
        private readonly IRecordValidator validator = new ValidatorBuilder().CreateDefault();
        private string filePath = "FileGenerated.csv";
        private int recordsAmount;
        private int startId;
        private Type fileCabinetType = typeof(FileCabinetMemoryService);

        /// <summary>
        /// Gets validator.
        /// </summary>
        /// <value>
        /// IRecordValidator.
        /// </value>
        public IRecordValidator Validator => this.validator;

        /// <summary>
        /// Gets or sets file path.
        /// </summary>
        /// <value>
        /// File path.
        /// </value>
        public string FilePath
        {
            get
            {
                return this.filePath;
            }

            set
            {
                this.filePath = value;
            }
        }

        /// <summary>
        /// Gets or sets records amount.
        /// </summary>
        /// <value>
        /// Records amount.
        /// </value>
        public int RecordsAmount
        {
            get
            {
                return this.recordsAmount;
            }

            set
            {
                this.recordsAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets start id.
        /// </summary>
        /// <value>
        /// Start id.
        /// </value>
        public int StartId
        {
            get
            {
                return this.startId;
            }

            set
            {
                this.startId = value;
            }
        }

        /// <summary>
        /// Gets or sets file cabinet type.
        /// </summary>
        /// <value>
        /// File cabinet type.
        /// </value>
        public Type FileCabinetType
        {
            get
            {
                return this.fileCabinetType;
            }

            set
            {
                this.fileCabinetType = value;
            }
        }
    }
}

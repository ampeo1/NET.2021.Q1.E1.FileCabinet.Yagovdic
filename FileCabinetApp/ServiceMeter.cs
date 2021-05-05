﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Measurable file cabinet service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public int CreateRecord(DataRecord dataRecord)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int id = this.service.CreateRecord(dataRecord);
            stopWatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return id;
        }

        /// <inheritdoc/>
        public bool Remove(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            bool result = this.service.Remove(id);
            stopWatch.Stop();
            Console.WriteLine($"Remove method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public void EditRecord(DataRecord dataRecord, long position)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            this.service.EditRecord(dataRecord, position);
            stopWatch.Stop();
            Console.WriteLine($"Edit record method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public long FindById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            long result = this.service.FindById(id);
            stopWatch.Stop();
            Console.WriteLine($"Find id method execution duration is {stopWatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public IRecordIterator FindByBirthDay(DateTime dateOfBirth)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var iterator = this.service.FindByBirthDay(dateOfBirth);
            stopWatch.Stop();
            Console.WriteLine($"Find by date of birth method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return iterator;
        }

        /// <inheritdoc/>
        public IRecordIterator FindByLastname(string lastName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var iterator = this.service.FindByLastname(lastName);
            stopWatch.Stop();
            Console.WriteLine($"Find by last name method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return iterator;
        }

        /// <inheritdoc/>
        public IRecordIterator FindByFirstName(string firstName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var iterator = this.service.FindByFirstName(firstName);
            stopWatch.Stop();
            Console.WriteLine($"Find by first name method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return iterator;
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            this.service.Restore(snapshot);
            stopWatch.Stop();
            Console.WriteLine($"Restore method execution duration is {stopWatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public int GetCountRemovedRecords()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int count = this.service.GetCountRemovedRecords();
            stopWatch.Stop();
            Console.WriteLine($"Get count removed records method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return count;
        }

        /// <inheritdoc/>
        public int GetCount()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int count = this.service.GetCount();
            stopWatch.Stop();
            Console.WriteLine($"Get count records method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return count;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var records = this.service.GetRecords();
            stopWatch.Stop();
            Console.WriteLine($"Get records method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return records;
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            return this.service.GetValidator();
        }

        /// <inheritdoc/>
        public int Purge()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int count = this.service.Purge();
            stopWatch.Stop();
            Console.WriteLine($"Purge method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return count;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var snapshot = this.service.MakeSnapshot();
            stopWatch.Stop();
            Console.WriteLine($"Make snapshot method execution duration is {stopWatch.ElapsedTicks} ticks.");

            return snapshot;
        }
    }
}

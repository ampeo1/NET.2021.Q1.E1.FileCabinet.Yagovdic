using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Introduce Parameter Object.
    /// </summary>
    public class DataRecord
    {
        /// <summary>
        /// Gets or sets identifier.
        /// </summary>
        /// <value>
        /// Identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        /// <value>
        /// First name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        /// <value>
        /// last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        /// <value>
        /// dd/MM/yyyy.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets access.
        /// </summary>
        /// <value>
        /// Access.
        /// </value>
        public char Access { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Representation record in file cabinet.
    /// </summary>
    [XmlType("record")]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord()
        {
        }

        /// <summary>
        /// Gets or sets identifier.
        /// </summary>
        /// <value>
        /// Identifier.
        /// </value>
        [XmlAttribute("id")]
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
        /// Last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        /// <value>
        /// Date of birth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets access.
        /// </summary>
        /// <value>
        /// Access.
        /// </value>
        [XmlElement(typeof(char))]
        public char Access { get; set; }

        /// <summary>
        /// Gets or sets age.
        /// </summary>
        /// <value>
        /// Age.
        /// </value>
        public short Age { get; set; }

        /// <summary>
        /// Gets or sets salary.
        /// </summary>
        /// <value>
        /// Salary.
        /// </value>
        public decimal Salary { get; set; }
    }
}

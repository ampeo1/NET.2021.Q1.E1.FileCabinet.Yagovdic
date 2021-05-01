using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class contains information about command.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Name command.</param>
        /// <param name="parameters">Parameters command.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="command"/> or <paramref name="parameters"/> is null.</exception>
        public AppCommandRequest(string command, string parameters)
        {
            if (string.IsNullOrEmpty(command) || parameters is null)
            {
                throw new ArgumentNullException($"{nameof(command)}, {nameof(parameters)}");
            }

            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets command name.
        /// </summary>
        /// <value>
        /// Command name.
        /// </value>
        public string Command { get; }

        /// <summary>
        /// Gets parameters.
        /// </summary>
        /// <value>
        /// Parameters.
        /// </value>
        public string Parameters { get; }
    }
}

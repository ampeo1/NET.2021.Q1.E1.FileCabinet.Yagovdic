using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class AppCommandRequest
    {
        public string Command { get; }

        public string Parameters { get; }

        public AppCommandRequest(string command, string parameters)
        {
            if (string.IsNullOrEmpty(command) || parameters is null)
            {
                throw new ArgumentNullException($"{nameof(command)}, {nameof(parameters)}");
            }

            this.Command = command;
            this.Parameters = parameters;
        }
    }
}

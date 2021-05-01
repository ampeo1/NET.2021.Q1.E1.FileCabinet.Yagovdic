using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Interface for command.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets next command.
        /// </summary>
        /// <param name="handler">Next command.</param>
        public void SetNext(ICommandHandler handler);

        /// <summary>
        /// Try to execute command.
        /// </summary>
        /// <param name="command">Information about command.</param>
        public void Handle(AppCommandRequest command);
    }
}

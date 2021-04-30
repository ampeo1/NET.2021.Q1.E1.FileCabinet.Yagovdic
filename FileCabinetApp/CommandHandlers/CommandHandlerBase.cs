using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        public abstract void Handle(AppCommandRequest command);

        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
        }
    }
}

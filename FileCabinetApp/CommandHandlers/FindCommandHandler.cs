using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;
        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <inheritdoc/>
        protected override string NameCommand => "find";

        /// <summary>
        /// Selects the desired search.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            string[] inputs = command.Parameters.Split(' ', 2);
            const int commandIndex = 0;
            var newCommandName = inputs[commandIndex];
            if (string.IsNullOrEmpty(newCommandName))
            {
                Console.WriteLine($"Error. find [property]");
                return;
            }

            const int parametersIndex = 1;
            var parameter = inputs.Length > 1 ? inputs[parametersIndex].Trim('\"') : string.Empty;
            AppCommandRequest newCommand = new AppCommandRequest(newCommandName, parameter);
            ICommandHandler commandHandlers = this.CreateCommandHandlers();
            commandHandlers.Handle(newCommand);
        }

        private ICommandHandler CreateCommandHandlers()
        {
            var findFirstNameHandler = new FindFirstnameCommandHandler(this.service, this.printer);
            var findLastNameHandler = new FindLastnameCommandHandler(this.service, this.printer);
            var findDateOfBirthHandler = new FindDateOfBirthCommandHandler(this.service, this.printer);
            findLastNameHandler.SetNext(findDateOfBirthHandler);
            findFirstNameHandler.SetNext(findLastNameHandler);

            return findFirstNameHandler;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public FindCommandHandler(IFileCabinetService service)
            : base(service)
        {
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
            var findFirstNameHandler = new FindFirstnameCommandHandler(this.service);
            var findLastNameHandler = new FindLastnameCommandHandler(this.service);
            var findDateOfBirthHandler = new FindDateOfBirthCommandHandler(this.service);
            findLastNameHandler.SetNext(findDateOfBirthHandler);
            findFirstNameHandler.SetNext(findLastNameHandler);

            return findFirstNameHandler;
        }
    }
}

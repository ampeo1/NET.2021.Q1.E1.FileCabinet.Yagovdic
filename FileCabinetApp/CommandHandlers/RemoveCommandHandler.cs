using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public RemoveCommandHandler(IFileCabinetService service)
           : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string NameCommand => "remove";

        /// <summary>
        /// Removes record.
        /// </summary>
        /// <param name="command">Parameters command.</param>
        public override void Handle(AppCommandRequest command)
        {
            if (this.GoToNextCommand(command))
            {
                return;
            }

            Tuple<bool, string, int> convertedValue = Converter.IntConverted(command.Parameters);
            if (!convertedValue.Item1)
            {
                Console.WriteLine(convertedValue.Item2);
                return;
            }

            int id = convertedValue.Item3;
            if (this.service.Remove(id))
            {
                Console.WriteLine($"Record #{id} is removed.");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
            }
        }
    }
}

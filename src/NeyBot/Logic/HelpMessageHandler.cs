using NeyBot.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class HelpMessageHandler
    {
        public static string HelpMessageBuilder(CommandGroup commandsInfo)
        {
            var helpMessage = new StringBuilder($"These are the commands available for ``!{commandsInfo.BaseCommand}``: \n\n");
            foreach (var command in commandsInfo.Values)
            {
                var completeCommand = command.GetMainCommand() + " " + command.GetSubCommand();
                var commandParams = command.GetParams();
                var commandDescription = command.GetDescription();

                if (commandParams == null)
                {
                    helpMessage.Append($"**!{completeCommand}** - {command.GetDescription()} \n");
                }else
                {
                    foreach ( var param in commandParams)
                    {
                        completeCommand = completeCommand + " " + param;
                    }
                    helpMessage.Append($"**!{completeCommand}** - {command.GetDescription()} \n");
                }
            }
            return helpMessage.ToString();
        }
    }
}

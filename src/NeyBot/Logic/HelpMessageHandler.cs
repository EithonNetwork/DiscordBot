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
        public static string HelpMessageBuilder(List<CommandInfo> commandsInfo)
        {
            var helpMessage = new StringBuilder($"These are the commands available for ``!{commandsInfo[0].GetMainCommand()}``: \n\n");
            foreach (var command in commandsInfo)
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
        }/*

        public static string Birthday()
        {
            var mainCommand = "birthday";
            CommandInfo[] commands =
            {
                new CommandInfo(mainCommand, "setbirthdate <year>-<month>-<day>", "Sets your birthdate (Format example: 2003-09-17)"),
                new CommandInfo(mainCommand, "show @<user>", "Sets your birthdate"),
                new CommandInfo(mainCommand, "upcoming", "Shows the closest upcoming birthdays"),
            };

            return HelpMessageBuilder(commands).ToString();
        }*/


    }
}

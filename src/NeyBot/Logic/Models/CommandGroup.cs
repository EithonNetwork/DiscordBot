using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic.Models
{
    class CommandGroup
    {
        private CommandService _commandService;
        private string _baseCommand;
        private List<CommandInfo> _commandInfos = new List<CommandInfo>();

        public CommandGroup(string baseCommand, CommandService commandService)
        {
            _baseCommand = baseCommand;
            _commandService = commandService;
        }

        public void Add(CommandInfo commandInfo)
        {
            _commandInfos.Add(commandInfo);
        }

        public IEnumerable<CommandInfo> Values
        {
            get
            {
                return _commandInfos;
            }
        }

        /*public IEnumerable<CommandInfo> HelpValues
        {
            get
            {
                return _commandInfos.Where(ci=> ci.isInc;
            }
        }*/

        public string BaseCommand
        {
            get
            {
                return _baseCommand;
            }
        }

        public void Register()
        {
            var helpMessage = HelpMessageHandler.HelpMessageBuilder(this);
            var helpCommand = new CommandInfo(_baseCommand, "help")
                .SetDescription($"Displays all the commands related to !{_baseCommand}")
                .SetAction(async e => await e.Channel.SendMessage(helpMessage));
            this.Add(helpCommand);

            _commandService.CreateGroup(_baseCommand, cgb =>
            {
                foreach (var command in _commandInfos)
                {
                    CreateCommandBasedOnCommandInfo(cgb, command);
                }
            });
        }

        public void CreateCommandBasedOnCommandInfo(CommandGroupBuilder cgb, CommandInfo commandInfo)
        {
            CommandBuilder command = cgb.CreateCommand(commandInfo.GetSubCommand())
                .Description(commandInfo.GetDescription());
            List<CommandParam> paramStrings = commandInfo.GetParams();
            if (paramStrings == null) { command.Do(commandInfo.GetAction()); return; }

            foreach (var param in commandInfo.GetParams())
            {
                if (param.IsRequired()) { command.Parameter(param.GetParamName(), ParameterType.Required); continue; }
                command.Parameter(param.GetParamName());

            }
            command.Do(commandInfo.GetAction());
        }
    }
}

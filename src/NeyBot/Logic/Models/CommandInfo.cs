using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic.Models
{
    class CommandInfo
    {
        //TODO: "bool includedInHelp"
        string _commandGroup;
        string _subCommand;
        private List<CommandParam> _params = new List<CommandParam>();
        string _description;
        Func<CommandEventArgs, Task> _action;

        public CommandInfo(string commandGroup, string subCommand)
        {
            _commandGroup = commandGroup;
            _subCommand = subCommand;
        }

        public CommandInfo AddParams(string paramName, bool isRequired)
        {
            CommandParam param = new CommandParam(paramName, isRequired);
            _params.Add(param);
            return this;
        }

        public CommandInfo SetDescription(string description)
        {
            _description = description;
            return this;
        }

        public CommandInfo SetAction(Func<CommandEventArgs, Task> action)
        {
            _action = action;
            return this;
        }

        //

        public string GetMainCommand()
        {
            return _commandGroup;
        }

        public string GetSubCommand()
        {
            return _subCommand;
        }

        public List<CommandParam> GetParams()
        {
            return _params;
        }

        public string GetDescription()
        {
            return _description;
        }

        public Func<CommandEventArgs, Task> GetAction()
        {
            return _action;
        }
    }
}

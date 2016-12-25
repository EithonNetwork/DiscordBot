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
        string[] _paramsString;
        string _description;
        Func<CommandEventArgs, Task> _action;

        public CommandInfo(string commandGroup, string subCommand)
        {
            _commandGroup = commandGroup;
            _subCommand = subCommand;
        }

        public CommandInfo SetParams(params string[] paramStrings)
        {
            _paramsString = paramStrings;
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

        public string[] GetParams()
        {
            return _paramsString;
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

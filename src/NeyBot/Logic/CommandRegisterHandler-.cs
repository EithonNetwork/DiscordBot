using Discord;
using Discord.Commands;
using NeyBot.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class CommandRegisterHandler
    {
        CommandService commands;

        public CommandRegisterHandler(DiscordClient discord)
        {
            commands = discord.GetService<CommandService>();
        }

        public void RegisterCommands()
        {
            //Register the commands here
            RegisterPicTutCommand();
            RegisterInvitationlinkCommand();
            RegisterRepCommand();
            RegisterBirthdayCommand();

        }

        //Make the methods for the commands registered here

        //Registers the command !pictut which links the tutorial Zalodu made for private voice channel management (Mainly a test command)
        public void RegisterPicTutCommand()
        {
            commands.CreateCommand("pictut")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("PicTuts/discord.tutorial.png");
                });
        }
        //Registers the !neybotinvite command that links the invite link to neybot
        public void RegisterInvitationlinkCommand()
        {
            commands.CreateCommand("neybotinvite")
            .Do(async (e) =>
            {
                await e.Channel.SendMessage("https://discordapp.com/api/oauth2/authorize?client_id=239322529385152512&scope=bot&permissions=0");
            });
        }

        //Registers the Reputation command (!rep) which handles reputation on Eithon. Since it's a bigger command it has it's own handler.
        public void RegisterRepCommand()

        {
            var baseCommand = "rep";
            List<CommandInfo> commandInfo = new List<CommandInfo>();

            var pointsCommand = new CommandInfo(baseCommand, "points")
                .SetParams("@<user>")
                .SetDescription("Show a user's reputation points")
                .SetAction(e => ReputationCommandHandler.GetReputation(e));
            commandInfo.Add(pointsCommand);

            var addCommand = new CommandInfo(baseCommand, "add")
                .SetParams("@<user>")
                .SetDescription("Add reputation to a user")
                .SetAction(e => ReputationCommandHandler.Add(e));
            commandInfo.Add(addCommand);

            var removeCommand = new CommandInfo(baseCommand, "remove")
                .SetParams("@<user>")
                .SetDescription("remove reputation to a user")
                .SetAction(e => ReputationCommandHandler.Remove(e));
            commandInfo.Add(removeCommand);

            var resetCommand = new CommandInfo(baseCommand, "reset")
                .SetParams("@<user>")
                .SetDescription("Reset a user (remove from database)")
                .SetAction(e => ReputationCommandHandler.ResetUser(e));
            commandInfo.Add(resetCommand);

            var resetallCommand = new CommandInfo(baseCommand, "resetall")
                .SetDescription("Reset all reputations. Clear database")
                .SetAction(e => ReputationCommandHandler.ResetAll(e));
            commandInfo.Add(resetallCommand);

            var toplistCommand = new CommandInfo(baseCommand, "toplist")
                .SetDescription("List users with the most reputation")
                .SetAction(e => ReputationCommandHandler.GetTopList(e));
            commandInfo.Add(toplistCommand);

            var helpMessage = HelpMessageHandler.HelpMessageBuilder(commandInfo);
            var helpCommand = new CommandInfo(baseCommand, "help")
                .SetDescription($"Displays all the commands related to !{baseCommand}")
                .SetAction(async e => await e.Channel.SendMessage(helpMessage));
            commandInfo.Add(helpCommand);

            commands.CreateGroup(baseCommand, cgb =>
            {
                foreach (var command in commandInfo)
                {
                    CreateCommandBasedOnCommandInfo(cgb, command);
                }
            });
        }
        //Registers the !Birthday command. Since it's a bigger command it has it's own handler.
        public void RegisterBirthdayCommand()
        {
            var baseCommand = "birthday";
            List<CommandInfo> commandInfo = new List<CommandInfo>();

            //setbirthdate command
            var setbirthdateCommand = new CommandInfo(baseCommand, "setbirthdate")
                .SetParams("<date>")
                .SetDescription("Sets your birthdate (Date format: 2003-09-17)")
                .SetAction(e => BirthdayHandler.Set(e));
            commandInfo.Add(setbirthdateCommand);

            //show command
            var showCommand = new CommandInfo(baseCommand, "show")
                .SetParams("@<user>")
                .SetDescription("Display a user's birthday")
                .SetAction(e => BirthdayHandler.Get(e));
            commandInfo.Add(showCommand);

            //upcoming command
            var upcomingCommand = new CommandInfo(baseCommand, "upcoming")
                .SetDescription("Show the closest upcoming birthdays")
                .SetAction(BirthdayHandler.GetUpcoming);
            commandInfo.Add(upcomingCommand);

            //help command
            var helpMessage = HelpMessageHandler.HelpMessageBuilder(commandInfo);
            var helpCommand = new CommandInfo(baseCommand, "help")
                .SetDescription($"Displays all the commands related to !{baseCommand}")
                .SetAction(async e => await e.Channel.SendMessage(helpMessage));
            commandInfo.Add(helpCommand);

            commands.CreateGroup(baseCommand, cgb =>
            {
                foreach (var command in commandInfo)
                {
                    CreateCommandBasedOnCommandInfo(cgb, command);
                }
            });
        }

        public void CreateCommandBasedOnCommandInfo(CommandGroupBuilder cgb, CommandInfo commandInfo)
        {
            CommandBuilder command = cgb.CreateCommand(commandInfo.GetSubCommand())
                .Description(commandInfo.GetDescription());
            string[] paramStrings = commandInfo.GetParams();
            if (paramStrings == null) { command.Do(commandInfo.GetAction()); return; }

            foreach (var param in commandInfo.GetParams())
            {
                command.Parameter(param, ParameterType.Required);
            }
            command.Do(commandInfo.GetAction());
        }

    }
}

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
        CommandService _commandService;

        public CommandRegisterHandler(DiscordClient discord)
        {
            _commandService = discord.GetService<CommandService>();
        }

        public void RegisterCommands()
        {
            //Register the commands here
            RegisterPicTutCommand("pictut");
            RegisterInvitationlinkCommand("neybotinvite");
            RegisterRepCommand("rep");
            RegisterBirthdayCommand("birthday");
            RegisterPermCommand("perm");
        }

        private void RegisterPermCommand(string baseCommand)
        {
            CommandInfo command;
            CommandGroup commandGroup = new CommandGroup(baseCommand, _commandService);

            command = new CommandInfo(baseCommand, "role")
                .AddParams("@<role>", true)
                .AddParams("assignownrole", true)
                .AddParams("<true/false>", false)
                .SetDescription("Adds or removes the permission to assign the role to others (from the specified role)")
                .SetAction(PermissionHandler.AssignOwnRolePermission);
            commandGroup.Add(command);

            command = new CommandInfo(baseCommand, "user")
                .AddParams("@<user>", true)
                .AddParams("assign/unassign", true)
                .AddParams("@<role>", false)
                .SetDescription("Adds specified role to the specified user")
                .SetAction(PermissionHandler.AssignRoleToOther);
            commandGroup.Add(command);

            /*command = new CommandInfo(baseCommand, "role")
                .AddParams("roleName", true)
                .AddParams("add/remove/show", true)
                .AddParams("permission", false)
                .SetDescription("Show a user's reputation points")
                .SetAction(ReputationCommandHandler.GetReputation);
            commandGroup.Add(command);*/

            commandGroup.Register();
        }

        //Make the methods for the commands registered here

        //Registers the command !pictut which links the tutorial Zalodu made for private voice channel management (Mainly a test command)
        public void RegisterPicTutCommand(string baseCommand)
        {
            _commandService.CreateCommand(baseCommand)
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("PicTuts/discord.tutorial.png");
                });
        }
        //Registers the !neybotinvite command that links the invite link to neybot
        public void RegisterInvitationlinkCommand(string baseCommand)
        {
            _commandService.CreateCommand(baseCommand)
            .Do(async (e) =>
            {
                await e.Channel.SendMessage("https://discordapp.com/api/oauth2/authorize?client_id=239322529385152512&scope=bot&permissions=0");
            });
        }

        //Registers the Reputation command (!rep) which handles reputation on Eithon. Since it's a bigger command it has it's own handler.
        public void RegisterRepCommand(string baseCommand)

        {
            CommandInfo command;
            CommandGroup commandGroup = new CommandGroup(baseCommand, _commandService);

            command = new CommandInfo(baseCommand, "points")
                .AddParams("@<user>", true)
                .SetDescription("Show a user's reputation points")
                .SetAction(ReputationCommandHandler.GetReputation);
            commandGroup.Add(command);

            command = new CommandInfo(baseCommand, "add")
                .AddParams("@<user>", true)
                .SetDescription("Add reputation to a user")
                .SetAction(ReputationCommandHandler.Add);
            commandGroup.Add(command);

            command = new CommandInfo(baseCommand, "remove")
                .AddParams("@<user>", true)
                .SetDescription("remove reputation to a user")
                .SetAction(ReputationCommandHandler.Remove);
            commandGroup.Add(command);

            command = new CommandInfo(baseCommand, "reset")
                .AddParams("@<user>", true)
                .SetDescription("Reset a user (remove from database)")
                .SetAction(ReputationCommandHandler.ResetUser);
            commandGroup.Add(command);

            command = new CommandInfo(baseCommand, "resetall")
                .SetDescription("Reset all reputations. Clear database")
                .SetAction(ReputationCommandHandler.ResetAll);
            commandGroup.Add(command);

            command = new CommandInfo(baseCommand, "toplist")
                .SetDescription("List users with the most reputation")
                .SetAction(ReputationCommandHandler.GetTopList);
            commandGroup.Add(command);

            commandGroup.Register();
        }
        //Registers the !Birthday command. Since it's a bigger command it has it's own handler.
        public void RegisterBirthdayCommand(string baseCommand)
        {
            CommandGroup commandGroup = new CommandGroup(baseCommand, _commandService);

            //setbirthdate command
            var setbirthdateCommand = new CommandInfo(baseCommand, "setbirthdate")
                .AddParams("<date>", true)
                .SetDescription("Sets your birthdate (Date format: 2003-09-17)")
                .SetAction(BirthdayHandler.Set);
            commandGroup.Add(setbirthdateCommand);

            //show command
            var showCommand = new CommandInfo(baseCommand, "show")
                .AddParams("@<user>", true)
                .SetDescription("Display a user's birthday")
                .SetAction(BirthdayHandler.Get);
            commandGroup.Add(showCommand);

            //upcoming command
            var upcomingCommand = new CommandInfo(baseCommand, "upcoming")
                .SetDescription("Show the closest upcoming birthdays")
                .SetAction(BirthdayHandler.GetUpcoming);
            commandGroup.Add(upcomingCommand);

            commandGroup.Register();
        }

    }
}

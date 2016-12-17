using Discord;
using Discord.Commands;
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
            commands.CreateGroup("rep", cgb =>
            {

                cgb.CreateCommand("points")
                    .Description("Show a user's reputation points")
                    .Parameter("user", ParameterType.Required)
                    .Do(e => ReputationCommandHandler.GetReputation(e));

                cgb.CreateCommand("add")
                    .Description("Add reputation to a user")
                    .Parameter("user", ParameterType.Required)
                    //currently not used
                    .Parameter("amount", ParameterType.Optional)
                    .Do(e=> ReputationCommandHandler.Add(e));

                cgb.CreateCommand("remove")
                    .Description("Remove reputation to a user")
                    .Parameter("user", ParameterType.Required)
                    //currently not used
                    .Parameter("amount", ParameterType.Optional)
                    .Do(e => ReputationCommandHandler.Remove(e));

                cgb.CreateCommand("reset")
                    .Description("Reset a user (remove from database)")
                    .Parameter("user", ParameterType.Required)
                    .Do(e => ReputationCommandHandler.ResetUser(e));

                cgb.CreateCommand("resetall")
                    .Description("Reset all reputations. Clear database")
                    .Do(e => ReputationCommandHandler.ResetAll(e));

                cgb.CreateCommand("toplist")
                    .Description("List users with the most reputation")
                    //currently not used
                    .Parameter("amount", ParameterType.Optional)
                    .Do(e => ReputationCommandHandler.GetTopList(e));
            });
        }
        //Registers the !Birthday command. Since it's a bigger command it has it's own handler.
        public void RegisterBirthdayCommand()
        {
            commands.CreateGroup("birthday", cgb =>
            {

                cgb.CreateCommand("setbirthdate")
                    .Description("Set your own birthday")
                    .Parameter("date", ParameterType.Required)
                    .Do(e => BirthdayHandler.Set(e));

                cgb.CreateCommand("show")
                    .Description("Display a user's birthday")
                    .Parameter("user", ParameterType.Required)
                    .Do(e => BirthdayHandler.Get(e));

                cgb.CreateCommand("upcoming")
                    .Description("Display upcoming birthdays")
                    .Do(e => BirthdayHandler.GetUpcoming(e));
            });
        }
    }
}

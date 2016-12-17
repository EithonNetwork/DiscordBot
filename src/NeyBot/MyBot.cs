using Discord;
using Discord.Commands;
using NeyBot.Database;
using NeyBot.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot
{
    class MyBot
    {
        DiscordClient discord;

        public MyBot()
        {
            //Creates the Discord object and sets up logging (I guess?)
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            //Decides what prefix that the bot will listen to (The start of every command)
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            //Registers the commmands
            var commandHandler = new CommandRegisterHandler(discord);
            commandHandler.RegisterCommands();

            var initiationAfterConnection = new InitiationsAfterConnection();
            initiationAfterConnection.AwaitConnectionThenDo(discord);

            //Connects to the Discord servers it is added to
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(ConfigurationHandler.GetStringOption("BotToken"), TokenType.Bot);
            });

            
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

using Discord.Commands;
using NeyBot.Database;
using NeyBot.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    public class ReputationCommandHandler
    {

        private static bool IsAllowedToChangeReputation(CommandEventArgs e)
        {
            return CommandResourcesHandler.ExecutingUserHasRole(e, "Admin") || CommandResourcesHandler.ExecutingUserHasRole(e, "Moderator");
        }
        private static bool IsAllowedToResetUserReputation(CommandEventArgs e)
        {
            return CommandResourcesHandler.ExecutingUserHasRole(e, "Admin");
        }
        private static bool IsAllowedToResetAllReputation(CommandEventArgs e)
        {
            return CommandResourcesHandler.IsOwner(e);
        }

        public static async Task GetReputation(CommandEventArgs e)
        {
            var userParam = CommandResourcesHandler.GetArgument(e, "@<user>");
            string userIdString = CommandResourcesHandler.GetUserId(userParam);
            var userObject = CommandResourcesHandler.GetUser(e, userIdString);
            if (userObject == null)
            {
                await e.Channel.SendMessage($"Could not find user");
            }
            else
            {
                var reputation = ReputationDatabaseHandler.GetReputation(userIdString);
                if (reputation == null)
                {
                    await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** does not appear to have any reputation points.");
                }
                else { 
                await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** has {reputation.Points} reputation points.");
                }
            }
        }
        public static async Task Add(CommandEventArgs e)
        {
            if (IsAllowedToChangeReputation(e))
            {
                var userParam = CommandResourcesHandler.GetArgument(e, "@<user>");
                string userIdString = CommandResourcesHandler.GetUserId(userParam);
                var userObject = CommandResourcesHandler.GetUser(e, userIdString);
                if (userObject == null)
                {
                    await e.Channel.SendMessage($"Could not find user");
                }
                else
                {
                    var oldReputation = ReputationDatabaseHandler.GetReputation(userIdString);
                    if (oldReputation == null)
                    {
                        oldReputation = new Reputation() { UserId = userIdString, Username = userObject.Name, Points = 1 };

                        ReputationDatabaseHandler.CreateReputation(oldReputation);
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** received their first reputation point! :tada:");
                    }
                    else
                    {
                        var newReputationPoints = oldReputation.Points + 1;
                        var currentReputation = new Reputation() { Id = oldReputation.Id, UserId = userIdString, Username = oldReputation.Username, Points = newReputationPoints };
                        ReputationDatabaseHandler.UpdateReputation(currentReputation);
                        await e.Channel.SendMessage($"**Added 1 reputation to {userObject.Nickname ?? userObject.Name}** \n" +
                        $"New reputation: {newReputationPoints} *(was {oldReputation.Points})*");
                    }
                }
            }
            else
            {
                await e.Channel.SendMessage("Lacking required role to execute command");
            }

        }
        public static async Task Remove(CommandEventArgs e)
        {
            if (IsAllowedToChangeReputation(e))
            {
                var userParam = CommandResourcesHandler.GetArgument(e, "@<user>");
                string userIdString = CommandResourcesHandler.GetUserId(userParam);
                var userObject = CommandResourcesHandler.GetUser(e, userIdString);
                if (userObject == null)
                {
                    await e.Channel.SendMessage($"Could not find user");
                }
                else
                {
                    var oldReputation = ReputationDatabaseHandler.GetReputation(userIdString);
                    if (oldReputation == null)
                    {
                        oldReputation = new Reputation() { UserId = userIdString, Username = userObject.Name, Points = -1 };

                        ReputationDatabaseHandler.CreateReputation(oldReputation);
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** lost a reputation point \n" +
                        $"New reputation: -1 *(was 0)*");
                    }
                    else
                    {
                        var newReputationPoints = oldReputation.Points - 1;
                        var currentReputation = new Reputation() { Id = oldReputation.Id, UserId = userIdString, Username = oldReputation.Username, Points = newReputationPoints };
                        ReputationDatabaseHandler.UpdateReputation(currentReputation);
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** lost a reputation point \n" +
                        $"New reputation: {newReputationPoints} *(was {oldReputation.Points})*");
                    }
                }
            }
            else
            {
                await e.Channel.SendMessage("Lacking required role to execute command");
            }

        }

        internal static async Task ResetUser(CommandEventArgs e)
        {
            if (IsAllowedToResetUserReputation(e))
            {
                var userParam = CommandResourcesHandler.GetArgument(e, "@<user>");
                string userIdString = CommandResourcesHandler.GetUserId(userParam);
                var userObject = CommandResourcesHandler.GetUser(e, userIdString);
                if (userObject == null)
                {
                    await e.Channel.SendMessage($"Could not find user");
                }
                else
                {
                    var reputation = ReputationDatabaseHandler.GetReputation(userIdString);
                    if (reputation == null)
                    {
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name} Do not seem have any reputation points");
                    }
                    else
                    {
                        ReputationDatabaseHandler.DeleteUser(reputation);
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** had their reputation reset.");
                    }
                }
            }
            else
            {
                await e.Channel.SendMessage("Lacking required role to execute command");
            }
        }

        internal static async Task ResetAll(CommandEventArgs e)
        {
            if (IsAllowedToResetAllReputation(e))
            {
                ReputationDatabaseHandler.DeleteAll();
                await e.Channel.SendMessage($"**Everyone's reputation was reset.**");
            }
            else
            {
                await e.Channel.SendMessage("Lacking required role to execute command");
            }
        }

        public static async Task GetTopList(CommandEventArgs e)
        {
            var topList = ReputationDatabaseHandler.GetTop(10);
            StringBuilder topListMessage = new StringBuilder($"**These are the top {topList.Count()} users with the highest reputation:** \n\n");
            var i = 1;
            foreach (var reputation in topList)
            {
                topListMessage.Append($"**{i++}.** {reputation.Username} - {reputation.Points} Points \n");
            }
            await e.Channel.SendMessage(topListMessage.ToString());
        }
    }
}

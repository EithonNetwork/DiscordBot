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
    public class BirthdayCommandHandler
    {

        public static async Task Get(CommandEventArgs e)
        {
            var userParam = CommandResourcesHandler.GetArgument(e, "user");
            string userIdString = CommandResourcesHandler.GetUserId(userParam);
            var userObject = CommandResourcesHandler.GetUser(e, userIdString);
            if (userObject == null)
            {
                await e.Channel.SendMessage($"Could not find user");
            }
            else
            {
                var birthday = BirthdayDatabaseHandler.Get(userIdString);
                DateTimeOffset birthdayDate = DateTimeOffset.Parse(birthday.BirthdayDate);
                var birthdayMonthDay = birthdayDate.ToString("MMMM dd");
                if (birthday == null)
                {
                    await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** does not appear to have added their birthday.");
                }
                else
                {
                    await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}**'s birthday is {birthdayMonthDay}.");
                }
            }
        }
        public static async Task Set(CommandEventArgs e)
        {
            var userObject = e.User;
            var birthdayParam = CommandResourcesHandler.GetArgument(e, "date");
            DateTimeOffset birthdayDate;
            var success = CommandResourcesHandler.TryParseDate(birthdayParam, out birthdayDate);
            if (!success)
            {
                await e.Channel.SendMessage("Please enter the date argument in the following format: \n" +
"<year>-<month>-<day>. Example: !birthday set 2016-05-27"); return;
            }

            var birthdayDateString = birthdayDate.ToString("yyyy-MM-dd");
            var birthdayMonthDayString = birthdayDate.ToString("MM-dd");
            var userIdString = e.User.Id.ToString();
            var oldBirthday = BirthdayDatabaseHandler.Get(userIdString);
            if (oldBirthday == null)
            {
                var newBirthday = new Birthday() { UserId = userIdString, Username = userObject.Name, BirthdayDate = birthdayDateString };
                BirthdayDatabaseHandler.Set(newBirthday);
                //TODO: Add how long time there is until it's time to celebrate that birthday in this string somehow
                await e.Channel.SendMessage($"**Set birthday for {userObject.Nickname ?? userObject.Name}**\n" +
                $"Birthday set to: {birthdayMonthDayString}");
                await e.Message.Delete();
            }
            else
            {
                var updatedBirthday = new Birthday() { Id = oldBirthday.Id, UserId = userIdString, Username = oldBirthday.Username, BirthdayDate = birthdayDateString };
                BirthdayDatabaseHandler.Update(updatedBirthday);
                DateTimeOffset oldBirthdayDate = DateTimeOffset.Parse(oldBirthday.BirthdayDate);
                var oldBirthdayDateString = oldBirthdayDate.ToString("MM-dd");
                //TODO: Add how long time there is until it's time to celebrate that birthday in this string somehow
                await e.Channel.SendMessage($"**Updated birthday for {userObject.Nickname ?? userObject.Name}** \n" +
                $"Birthday set to: {birthdayMonthDayString} *(was {oldBirthdayDateString}).*");
                await e.Message.Delete();
            }

        }
        /*public static async Task UnsetBirthday(CommandEventArgs e)
        {
            if (IsAllowedToChangeReputation(e))
            {
                var userParam = CommandResourcesHandler.GetArgument(e, "user");
                string userIdString = CommandResourcesHandler.GetUserId(userParam);
                var userObject = CommandResourcesHandler.GetUser(e, userIdString);
                if (userObject == null)
                {
                    await e.Channel.SendMessage($"Could not find user");
                }
                else
                {
                    var oldReputation = BirthdayDatabaseHandler.GetReputation(userIdString);
                    if (oldReputation == null)
                    {
                        oldReputation = new Reputation() { UserId = userIdString, Username = userObject.Name, Points = -1 };

                        BirthdayDatabaseHandler.CreateReputation(oldReputation);
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** lost a reputation point \n" +
                        $"New reputation: -1 *(was 0)*");
                    }
                    else
                    {
                        var newReputationPoints = oldReputation.Points - 1;
                        var currentReputation = new Reputation() { Id = oldReputation.Id, UserId = userIdString, Username = oldReputation.Username, Points = newReputationPoints };
                        BirthdayDatabaseHandler.UpdateReputation(currentReputation);
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

        internal static async Task RemoveUserBirthday(CommandEventArgs e)
        {
            if (IsAllowedToResetUserReputation(e))
            {
                var userParam = CommandResourcesHandler.GetArgument(e, "user");
                string userIdString = CommandResourcesHandler.GetUserId(userParam);
                var userObject = CommandResourcesHandler.GetUser(e, userIdString);
                if (userObject == null)
                {
                    await e.Channel.SendMessage($"Could not find user");
                }
                else
                {
                    var reputation = BirthdayDatabaseHandler.GetReputation(userIdString);
                    if (reputation == null)
                    {
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name} Do not seem have any reputation points");
                    }
                    else
                    {
                        BirthdayDatabaseHandler.DeleteUser(reputation);
                        await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** had their reputation reset.");
                    }
                }
            }
            else
            {
                await e.Channel.SendMessage("Lacking required role to execute command");
            }
        }

        internal static async Task RemoveAllBirthdays(CommandEventArgs e)
        {
            if (IsAllowedToResetAllReputation(e))
            {
                BirthdayDatabaseHandler.DeleteAll();
                await e.Channel.SendMessage($"**Everyone's reputation was reset.**");
            }
            else
            {
                await e.Channel.SendMessage("Lacking required role to execute command");
            }
    }*/

        public static async Task GetUpcoming(CommandEventArgs e)
        {
            var upcomingBirthdays = BirthdayDatabaseHandler.GetUpcoming(10);
            StringBuilder topListMessage = new StringBuilder($"**These are the {upcomingBirthdays.Count()} closest upcoming birthdays:** \n\n");
            var i = 1;
            foreach (var birthday in upcomingBirthdays)
            {
                DateTimeOffset birthdayDate = DateTimeOffset.Parse(birthday.BirthdayDate);
                var birthdayMonthDay = birthdayDate.ToString("MMMM dd");
                topListMessage.Append($"**{i++}.** {birthday.Username} - {birthdayMonthDay} \n");
            }
            await e.Channel.SendMessage(topListMessage.ToString());
        }
    }
}

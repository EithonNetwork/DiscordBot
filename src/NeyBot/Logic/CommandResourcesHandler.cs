﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class CommandResourcesHandler
    {
        //Neylion's Id, currently not used anywhere for testing purposes
        private static ulong ownerId = ConfigurationHandler.GetUlongOption("NeylionID");
        //Add the role IDs that you have commands for here and add the variable to the "GetRoleBasedOnName" method as well.
        private static ulong administratorRoleId = ConfigurationHandler.GetUlongOption("Admin");
        private static ulong moderatorRoleId = ConfigurationHandler.GetUlongOption("Moderator");

        public static string GetUserId(string userParam)
        {
            var r = Regex.Match(userParam, "[0-9]+");
            return r.Groups[0].Value;
        }

        //This method gets a user based on it's ID string 
        public static User GetUser(CommandEventArgs e, string userIdString)
        {
            try
            {
                ulong userID = ulong.Parse(userIdString);
                var userObject = e.Server.GetUser(userID);
                return userObject;
            }
            catch (System.FormatException)
            {
                return null;
            }
        }

        //Gets the specific argument for the parameter requested (based on the parameter string). String must be equal to the one of the parameters in the command.
        public static string GetArgument(CommandEventArgs e, string parameterName)
        {
            var parameter = e.Command.Parameters.FirstOrDefault(p => p.Name == parameterName);
            if (parameter == null) throw new ArgumentException($"Paremeter \"{parameterName}\" not found");
            return e.Args[parameter.Id];
        }

        public static bool IsOwner(CommandEventArgs e)
        {
            if (e.User == null) return false;
            if (e.User.Id != ownerId) return false;
            return true;
        }

        //Currently not doing anything unique, but if we want to change something specific regarding the date format it is done here
        public static bool TryParseDate(string date, out DateTimeOffset dateValue)
        {
            return (DateTimeOffset.TryParse(date, out dateValue));
        }


        //Checks if user has specific role (based on the string name that is being sent in)
        public static bool ExecutingUserHasRole(CommandEventArgs e, string roleName)
        {
            var roleId = GetRoleIdBasedOnName(roleName);
            if (roleId == null) return false;
            if (e.User == null) return false;
            var role = e.Server.GetRole((ulong) roleId);
            if (role == null) return false;
            return e.User.HasRole(role);
        }

        
        private static ulong? GetRoleIdBasedOnName(string roleName)
        {
            //Add a line for all roles you have that you want to check for permissions on
            if (roleName == "Admin") return administratorRoleId; 
            if (roleName == "Moderator") return moderatorRoleId;
            return null;
        }

    }
    
}

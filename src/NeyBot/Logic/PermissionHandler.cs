using Discord;
using Discord.Commands;
using NeyBot.Database;
using NeyBot.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class PermissionHandler
    {
        private static bool IsAllowedToAssignRole(CommandEventArgs e, Role roleObject)
        {
            var roleIdString = roleObject.Id.ToString();
            var CanAssignRoleObject = CanAssignRoleDatabaseHandler.Get(roleIdString);

            if (!e.User.Roles.Contains<Role>(roleObject)) return false; 
            if (CanAssignRoleObject == null) return false;
            return true;
        }

        public static async Task AssignOwnRolePermission(CommandEventArgs e)
        {
            var roleParam = CommandResourcesHandler.GetArgument(e, "@<role>");
            string roleIdString = CommandResourcesHandler.GetMentionId(roleParam);
            var roleObject = CommandResourcesHandler.GetRole(e, roleIdString);

            if (roleObject == null) { await e.Channel.SendMessage($"Could not find role"); return; }

            var setOrUnsetParamString = CommandResourcesHandler.GetArgument(e, "<true/false>");
            bool setOrUnsetParam;

            if (!bool.TryParse(setOrUnsetParamString, out setOrUnsetParam))
            {
                await e.Channel.SendMessage($"Could not parse {setOrUnsetParamString} to true/false");
            }

            if (setOrUnsetParam) await AddAssignOwnRolePermission(e, roleObject);
            if (!setOrUnsetParam) await RemoveAssignOwnRolePermission(e, roleObject);
        }

        public static async Task AssignRoleToOther(CommandEventArgs e)
        {
            var assignOrUnAssignParam = CommandResourcesHandler.GetArgument(e, "assign/unassign");
            if (assignOrUnAssignParam == "assign") { await AddRoleToOther(e); return; }
            if (assignOrUnAssignParam == "unassign") { await RemoveRoleFromOther(e); return; }
            await e.Channel.SendMessage($"Could not interpret \"{assignOrUnAssignParam}\". Expected \"assign\" or \"unassign\"");
        }
        public static async Task AddRoleToOther(CommandEventArgs e)
        {
            var userParam = CommandResourcesHandler.GetArgument(e, "@<user>");
            string userIdString = CommandResourcesHandler.GetMentionId(userParam);
            var userObject = CommandResourcesHandler.GetUser(e, userIdString);

            var roleParam = CommandResourcesHandler.GetArgument(e, "@<role>");
            string roleIdString = CommandResourcesHandler.GetMentionId(roleParam);
            var roleObject = CommandResourcesHandler.GetRole(e, roleIdString);

            if (userObject == null) { await e.Channel.SendMessage($"Could not find user"); return; }
            if (roleObject == null) { await e.Channel.SendMessage($"Could not find role"); return; }
            if (!IsAllowedToAssignRole(e, roleObject)) { await e.Channel.SendMessage($"Lacking required permission to execute task"); return; }

            await userObject.AddRoles(roleObject);
            await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** was assigned the **{roleObject.Name}** role");
        }

        public static async Task RemoveRoleFromOther(CommandEventArgs e)
        {
            var userParam = CommandResourcesHandler.GetArgument(e, "@<user>");
            string userIdString = CommandResourcesHandler.GetMentionId(userParam);
            var userObject = CommandResourcesHandler.GetUser(e, userIdString);

            var roleParam = CommandResourcesHandler.GetArgument(e, "@<role>");
            string roleIdString = CommandResourcesHandler.GetMentionId(roleParam);
            var roleObject = CommandResourcesHandler.GetRole(e, roleIdString);

            if (!IsAllowedToAssignRole(e, roleObject)) { await e.Channel.SendMessage($"Lacking required permission to execute task"); return; }
            if (userObject == null) { await e.Channel.SendMessage($"Could not find user"); return; }
            if (roleObject == null) { await e.Channel.SendMessage($"Could not find role"); return; }

            await userObject.RemoveRoles(roleObject);
            await e.Channel.SendMessage($"**{userObject.Nickname ?? userObject.Name}** was removed from the **{roleObject.Name}** role");
        }

        private static async Task AddAssignOwnRolePermission(CommandEventArgs e, Role roleObject)
        {
            var roleIdString = roleObject.Id.ToString();
            var oldCanAssignRoleObject = CanAssignRoleDatabaseHandler.Get(roleIdString);
            if (oldCanAssignRoleObject == null)
            {
                var newCanAssignRoleObject = new CanAssignRole() { RoleId = roleIdString, RoleName = roleObject.Name };
                CanAssignRoleDatabaseHandler.Set(newCanAssignRoleObject);
                await e.Channel.SendMessage($"**{roleObject}** can now assign their own role to others through ``!assign @<user> @<rolename>``\n");
            }
            else
            {
                var updatedCanAssignRoleObject = new CanAssignRole() { Id = oldCanAssignRoleObject.Id, RoleId = roleIdString, RoleName = roleObject.Name };
                CanAssignRoleDatabaseHandler.Update(updatedCanAssignRoleObject);
                await e.Channel.SendMessage($"**{roleObject}** already have permission to assign their role to others but is now updated to current rolename.");
            }
        }

        private static async Task RemoveAssignOwnRolePermission(CommandEventArgs e, Role roleObject)
        {
            var roleIdString = roleObject.Id.ToString();
            var CanAssignRoleObject = CanAssignRoleDatabaseHandler.Get(roleIdString);
            if (CanAssignRoleObject == null)
            {
                await e.Channel.SendMessage($"{roleObject.Name} does not have that permission");
            }
            else
            {
                CanAssignRoleDatabaseHandler.Delete(CanAssignRoleObject);
                await e.Channel.SendMessage($"**{roleObject.Name}** does no longer have that permission");
            }
        }
    }
}

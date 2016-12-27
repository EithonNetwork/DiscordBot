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
        public static async Task AssignOwnRolePermission(CommandEventArgs e)
        {
            var roleParam = CommandResourcesHandler.GetArgument(e, "@<roleName>");
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

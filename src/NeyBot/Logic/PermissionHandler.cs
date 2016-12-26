using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class PermissionHandler
    {
        public async Task AssignOwnRolePermission(CommandEventArgs e)
        {
            var roleParam = CommandResourcesHandler.GetArgument(e, "@<roleName>");
            string roleIdString = CommandResourcesHandler.GetMentionId(roleParam);
            var roleObject = CommandResourcesHandler.GetRole(e, roleIdString);

            if (roleObject == null){ await e.Channel.SendMessage($"Could not find role"); return; }

        }
    }
}

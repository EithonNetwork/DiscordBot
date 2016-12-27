using Dapper;
using NeyBot.Database.Models;
using NeyBot.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Database
{
    class CanAssignRoleDatabaseHandler
    {
        private static string connectionString = ConfigurationHandler.GetStringOption("ConnectionString");
        public static CanAssignRole Get(string roleId)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                CanAssignRole canAssignRole = conn.Query<CanAssignRole>(
                    @"SELECT id Id, role_id RoleId, role_name RoleName FROM can_assign_role WHERE role_id = @RoleId",
                    new { RoleId = roleId })
                    .FirstOrDefault();
                return canAssignRole;
            }
        }

        public static int Set(CanAssignRole canAssignRole)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                int id = conn.Query<int>(
                    @"INSERT INTO can_assign_role (role_id, role_name) VALUES (@RoleId, @RoleName);
                    SELECT LAST_INSERT_ID()",
                    new { RoleId = canAssignRole.RoleId, RoleName = canAssignRole.RoleName })
                    .Single();
                return id;
            }
        }
        public static void Update(CanAssignRole canAssignRole)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(
                    @"UPDATE can_assign_role SET role_name=@RoleName WHERE id=@Id",
                    new { RoleName = canAssignRole.RoleName, Id = canAssignRole.Id });
            }
        }
        public static void Delete(CanAssignRole canAssignRole)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(@"
                    DELETE FROM can_assign_role WHERE role_id = @RoleId LIMIT 1",
                    new { RoleId = canAssignRole.RoleId });
            }
        }
        public static void DeleteAll()
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(
                    @"DELETE FROM can_assign_role");
            }
        }
    }
}


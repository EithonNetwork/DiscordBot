using Dapper;
using MySql.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using NeyBot.Logic;

namespace NeyBot.Database
{
    public class ReputationDatabaseHandler
    {
        private static string connectionString = ConfigurationHandler.GetStringOption("ConnectionString");
        public static Reputation GetReputation(string userId)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                Reputation reputation = conn.Query<Reputation>(
                    @"SELECT id Id, user_id UserId, username Username, points Points FROM reputation WHERE user_id = @UserId",
                    new { UserId = userId })
                    .FirstOrDefault();
                return reputation;
            }
        }

        public static int CreateReputation(Reputation reputation)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                int id = conn.Query<int>(
                    @"INSERT INTO reputation (user_id, username, points) VALUES (@UserId, @Username, @Points);
                    SELECT LAST_INSERT_ID()",
                    new { UserId = reputation.UserId, Username = reputation.Username, Points = reputation.Points })
                    .Single() ;
                return id;
            }
        }
        public static void UpdateReputation(Reputation reputation)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(
                    @"UPDATE reputation SET username=@Username, points=@Points WHERE id=@Id",
                    new { Username = reputation.Username, Points = reputation.Points, Id = reputation.Id });
            }
        }
        public static void DeleteUser(Reputation reputation)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(
                    @"DELETE FROM reputation WHERE user_id = @UserId LIMIT 1",
                    new { UserId = reputation.UserId });
            }
        }
        public static void DeleteAll()
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(
                    @"DELETE FROM reputation");
            }
        }
        public static IEnumerable<Reputation> GetTop(int limit)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                var reputations = conn.Query<Reputation>(@"
                SELECT 
                    id Id, 
                    user_id UserId, 
                    username Username, 
                    points Points 
                FROM reputation 
                ORDER BY points 
                DESC LIMIT @Limit",
                    new { Limit = limit });
                return reputations;
            }
        }
    }
}

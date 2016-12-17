using Dapper;
using MySql.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using NeyBot.Logic;

namespace NeyBot.Database
{
    public class BirthdayDatabaseHandler
    {
        private static string connectionString = ConfigurationHandler.GetStringOption("ConnectionString");
        public static Birthday Get(string userId)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                Birthday birthday = conn.Query<Birthday>(
                    @"SELECT id Id, user_id UserId, username Username, birthday BirthdayDate FROM birthdays WHERE user_id = @UserId",
                    new { UserId = userId })
                    .FirstOrDefault();
                return birthday;
            }
        }

        public static int Set(Birthday birthday)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                int id = conn.Query<int>(
                    @"INSERT INTO birthdays (user_id, username, birthday) VALUES (@UserId, @Username, @Birthday);
                    SELECT LAST_INSERT_ID()",
                    new { UserId = birthday.UserId, Username = birthday.Username, Birthday = birthday.BirthdayDate })
                    .Single() ;
                return id;
            }
        }
        public static void Update(Birthday birthday)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(
                    @"UPDATE birthdays SET username=@Username, birthday=@Birthday WHERE id=@Id",
                    new { Username = birthday.Username, Birthday = birthday.BirthdayDate, Id = birthday.Id });
            }
        }
        public static void Delete(Birthday reputation)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(@"
                    DELETE FROM reputation WHERE user_id = @UserId LIMIT 1",
                    new { UserId = reputation.UserId });
            }
        }
        public static void DeleteAll()
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(
                    @"DELETE FROM birthdays");
            }
        }
        public static IEnumerable<Birthday> GetUpcoming(int limit)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                var birthdays = conn.Query<Birthday>(@"
                    SELECT
                        id Id, 
                        user_id UserId, 
                        username Username, 
                        birthday BirthdayDate,
                        birthday + INTERVAL(YEAR(CURRENT_TIMESTAMP) - YEAR(birthday)) + 0 YEAR AS currbirthday,
                        birthday + INTERVAL(YEAR(CURRENT_TIMESTAMP) - YEAR(birthday)) + 1 YEAR AS nextbirthday
                    FROM birthdays
                    ORDER BY CASE
                        WHEN currbirthday >= CURRENT_TIMESTAMP THEN currbirthday
                        ELSE nextbirthday
                    END
                    LIMIT @Limit",
                    new { Limit = limit });
                return birthdays;
            }
        }
        public static IEnumerable<Birthday> GetTodays(int limit)
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                var birthdays = conn.Query<Birthday>(@"
                    SELECT
                        id Id, 
                        user_id UserId, 
                        username Username, 
                        birthday BirthdayDate
                    FROM birthdays
                    WHERE
                    	SUBSTRING(birthday, 6, 5) = SUBSTRING(CURRENT_DATE, 6, 5)");
                return birthdays;
            }
        }
    }
}

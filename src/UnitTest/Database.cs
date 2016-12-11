using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeyBot.Database;

namespace UnitTest
{
    [TestClass]
    public class Database
    {
        [TestMethod]
        public void GetEmptyReputation()
        {
            BirthdayDatabaseHandler.DeleteAll();
            var reputation = BirthdayDatabaseHandler.GetReputation("WillNotExist");
            Assert.IsNull(reputation);
        }
        [TestMethod]
        public void AddReputation()
        {
            BirthdayDatabaseHandler.DeleteAll();
            var reputation = CreateReputationPoco();
            Create(reputation);
        }

        [TestMethod]
        public void AddAndGet()
        {
            BirthdayDatabaseHandler.DeleteAll();
            var reputation = CreateReputationPoco();
            Create(reputation);

            var getrep = Get(reputation);
        }

        [TestMethod]
        public void Update()
        {
            BirthdayDatabaseHandler.DeleteAll();
            var reputation = CreateReputationPoco();
            Create(reputation);

            var getrep = Get(reputation);
            getrep = Update(reputation, "NewUserName", getrep.Points+5);
        }

        #region Supportive functions
        private Reputation CreateReputationPoco()
        {
            var reputation = new Reputation()
            {
                UserId = Guid.NewGuid().ToString().Substring(0, 32),
                Username = Guid.NewGuid().ToString().Substring(0, 32),
                Points = 0
            };

            return reputation;
        }

        private int Create(Reputation reputation)
        {
            var id = BirthdayDatabaseHandler.CreateReputation(reputation);
            return id;
        }

        private Reputation Get(Reputation inReputation)
        {
            var outReputation = BirthdayDatabaseHandler.GetReputation(inReputation.UserId);
            Assert.IsNotNull(outReputation);
            Assert.AreEqual(inReputation.Username, outReputation.Username);
            Assert.AreEqual(inReputation.Points, outReputation.Points);
            return outReputation;
        }

        private Reputation Update(Reputation old, string userName, int points)
        {
            var currentReputation = Get(old);
            currentReputation.Username = userName;
            currentReputation.Points = points;
            BirthdayDatabaseHandler.UpdateReputation(currentReputation);
            currentReputation = Get(currentReputation);
            Assert.AreEqual(currentReputation.Username, userName);
            Assert.AreEqual(currentReputation.Points, points);
            return currentReputation;
        }
        #endregion
    }
}

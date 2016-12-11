using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Database
{
    public class Reputation
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public int Points { get; set; }
    }
}

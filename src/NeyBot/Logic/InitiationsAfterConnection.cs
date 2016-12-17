using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class InitiationsAfterConnection
    {
        Timer _timer;
        Server _server;
        TimerHandler _timerHandler;

        public InitiationsAfterConnection()
        {

        }

        public void AwaitConnectionThenDo(DiscordClient discord)
        {
            _timer = new System.Threading.Timer(x =>
            {
                Initiations(discord);
            },
                null, TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);
        }

        public void Initiations(DiscordClient discord)
        {
            _server = CommandResourcesHandler.GetServer(discord);

            //Initiate functions here
            _timerHandler = new TimerHandler();
            _timerHandler.CongratulateBirthdaysTimer(_server, new TimeSpan(10, 30, 00));
        }
    }
}

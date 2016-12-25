using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class TimerHandler
    {
        Timer _timer;

        public TimerHandler()
        {

        }

        public TimeSpan CongratulateBirthdaysTimer(Server server, TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero) timeToGo += TimeSpan.FromHours(24);

            var bh = new BirthdayHandler();

            _timer = new System.Threading.Timer(x =>
            {
                bh.CongratulateBirthdays(server, x);
            },
                alertTime, timeToGo, Timeout.InfiniteTimeSpan);

            //var channelName = CommandResourcesHandler.GetChannel(server, "GeneralTextChannel");
            //channelName.SendMessage($"Started Timer. Time remaining: {timeToGo.Hours}:{timeToGo.Minutes}:{timeToGo.Seconds}");

            return timeToGo;
        }
    }
}

﻿using System;
using System.Diagnostics;
using YetAnotherRelogger.Helpers.Bot;

namespace YetAnotherRelogger.Helpers.Stats
{
    public class ChartStats
    {
        public ChartStats()
        {
            GoldStats = new Gold();
        }
        public Gold GoldStats;

        #region Gold Class
        public class Gold
        {
            public DateTime StartTime;
            public double Hours { get { return DateTime.Now.Subtract(StartTime).TotalSeconds / 3600; } }
            public double GoldPerHour
            {
                get
                {
                    var gph = (LastCoinage - StartCoinage)/Hours;
                    return double.IsNaN(gph) ? 0 : gph;
                }
            }

            public long LastGain { get; private set; }
            public long TotalGain { get; private set; }
            public long StartCoinage { get; private set; }
            public long LastCoinage { get; private set; }

            public Gold()
            {
                StartTime = DateTime.Now;
                LastCoinage = 0;
                LastGain = 0;
                TotalGain = 0;
                StartCoinage = 0;
                LastCoinage = 0;
            }
            public void Update(BotClass bot)
            {
                var coinage = bot.AntiIdle.Stats.Coinage;
                if (coinage > 0)
                {
                    LastGain = coinage - LastCoinage;
                    LastCoinage = coinage;
                    if (coinage < StartCoinage)
                    {
                        Logger.Instance.Write(bot,"[GoldPerHour] Reset! (Current coinage is below start coinage)");
                        StartCoinage = 0;
                        TotalGain = 0;
                    }
                    if (StartCoinage <= 0)
                    {
                        Logger.Instance.Write(bot, "[GoldPerHour] New start coinage: {0:N0}", coinage);
                        StartCoinage = coinage;
                        StartTime = DateTime.Now;
                    }
                    else
                    {
                        TotalGain += LastGain;
                        Debug.WriteLine("[GoldPerHour] <{0}> LastGain: {1:N0}, TotalGain: {2:N0}, GPH: {3}", bot.Name, LastGain, TotalGain, GoldPerHour);
                    }
                }
            }
        }
        #endregion
    }
}
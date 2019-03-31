using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordStreaks
{
    class Streaks
    {
        public int currentStreak { get; set; }
        public int maxStreak { get; set; }
        public int lastStreakRun { get; set; }

        public Streaks(int curStreak, int mxStreak, int time)
        {
            this.currentStreak = curStreak;
            this.maxStreak = mxStreak;
            this.lastStreakRun = time;
        }
        public void Streak(int newTime)
        {
            //check if it has been 24 hours
            if ( newTime - this.lastStreakRun < 86400)
            {
                //increase streak
                this.currentStreak++;
                if (this.currentStreak > this.maxStreak)
                {
                    //set max streak to current streak if larger
                    this.maxStreak = this.currentStreak;
                }
            } else if (newTime - this.lastStreakRun > 86400)
            {
                //reset current streak if its been over 24 hours
                this.currentStreak = 1;
            }
            //set the new time as last run time
            this.lastStreakRun = newTime;
        }
    }
}

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using System.IO;
using Newtonsoft.Json;

namespace DiscorsStreaks.Modules
{
    [Name("Streaks")]
    public class StreaksModule : ModuleBase<SocketCommandContext>
    {
        [Command("Streaks"), Alias("🔥")]
        [Summary("Keep track of your message streaks on the current server!\n" +
            "In order to use this command just type in the command once a day")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Streaks()
        {
            
            string User = Context.User.Username + "#" + Context.User.Discriminator + "-" + Context.Guild;
            string path = Directory.GetCurrentDirectory();
            string folderName = @"\json";
            string subdir = path + folderName;
            Console.WriteLine(subdir);
            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }
            Console.WriteLine(User);
            string FileName = subdir + @"\" + User + ".json";
            if (!File.Exists(FileName))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(FileName))
                {
                    // get unix time stamp
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    DiscordStreaks.Streaks CurrentStreakUser = new DiscordStreaks.Streaks(1, 1, unixTimestamp);
                    string output = JsonConvert.SerializeObject(CurrentStreakUser);
                    sw.WriteLine(output);
                    string reply = "Your first streak?! welcome to the Discord Streaks bot!";
                    ReplyAsync(reply);
                }
            }
            else if (File.Exists(FileName))
            {
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string jsonData = File.ReadAllText(FileName);
                var myDetails = JsonConvert.DeserializeObject<DiscordStreaks.Streaks>(jsonData);
                myDetails.Streak(unixTimestamp);
                string output = JsonConvert.SerializeObject(myDetails);
                File.Delete(FileName);
                using (StreamWriter sw = File.CreateText(FileName))
                {
                    sw.WriteLine(output);
                    if (myDetails.currentStreak is 1)
                    {
                        string reply = Context.User.Username + " welcome back! your current streak is sadly only " + myDetails.currentStreak + " now, it has been over 24 hours";
                        ReplyAsync(reply);
                    }
                    else if (myDetails.currentStreak > 1)
                    {
                        string reply = Context.User.Username + " welcome back! your current streak is " + myDetails.currentStreak + "!!";
                        ReplyAsync(reply);
                    }
                    
                }
            }
        }

        [Command("DeleteStreaks"), Alias("D🔥")]
        [Summary("Use this to delete and clear your streaks entirely")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DelteStreaks()
        {
            string User = Context.User.Username + "#" + Context.User.Discriminator + "-" + Context.Guild;
            string path = Directory.GetCurrentDirectory();
            string folderName = @"\json";
            string subdir = path + folderName;
            string FileName = subdir + @"\" + User + ".json";
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
                string reply = Context.User.Username + "Your streak file has been deleted, have a good day";
                ReplyAsync(reply);
            }
        }

        [Command("CheckStreaks"), Alias("C🔥")]
        [Summary("Use this to check on your streaks")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CheckStreaks()
        {
            
            string User = Context.User.Username + "#" + Context.User.Discriminator + "-" + Context.Guild;
            string path = Directory.GetCurrentDirectory();
            string folderName = @"\json";
            string subdir = path + folderName;
            string FileName = subdir + @"\" + User + ".json";

            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string jsonData = File.ReadAllText(FileName);
            var myDetails = JsonConvert.DeserializeObject<DiscordStreaks.Streaks>(jsonData);
            myDetails.Streak(unixTimestamp);
            string output = JsonConvert.SerializeObject(myDetails);
            int timeSinceStreakSecconds = unixTimestamp - myDetails.lastStreakRun;
            int timeSinceStreakMinutes = timeSinceStreakSecconds / 60;
            int timeSinceStreakHours = timeSinceStreakMinutes / 60;
            if (File.Exists(FileName))
            {
                string reply = Context.User.Username + "Your streak is currently " + myDetails.currentStreak + ". Your high score is " + myDetails.maxStreak + ". It has been " + timeSinceStreakHours + " hours sinse your last streak";
                ReplyAsync(reply);
            }
            else if (!File.Exists(FileName))
            {
                string reply = "You dont have any streaks on this server! Start a streak by using the '+!streaks'";
                ReplyAsync(reply);
            }
        }

        //[Group("set"), Name("Example")]
        //[RequireContext(ContextType.Guild)]
        //public class Set : ModuleBase
        //{
        //    [Command("nick"), Priority(1)]
        //    [Summary("Change your nickname to the specified text")]
        //    [RequireUserPermission(GuildPermission.ChangeNickname)]
        //    public Task Nick([Remainder]string name)
        //        => Nick(Context.User as SocketGuildUser, name);

        //    [Command("nick"), Priority(0)]
        //    [Summary("Change another user's nickname to the specified text")]
        //    [RequireUserPermission(GuildPermission.ManageNicknames)]
        //    public async Task Nick(SocketGuildUser user, [Remainder]string name)
        //    {
        //        await user.ModifyAsync(x => x.Nickname = name);
        //        await ReplyAsync($"{user.Mention} I changed your name to **{name}**");
        //    }
        //}
    }
}

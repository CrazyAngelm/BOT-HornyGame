using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace BOT_HornyGame.Modules
{
    public class RollModule : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        [Summary("Roll dice.")]
        public async Task RollAsync([Remainder][Summary("The text to roll")] string strRoll)
        {
            Random rand = new Random();
            int sum = 0;
            string str = string.Empty;

            string[] arrStr = strRoll.Split(' ');
            string comment = strRoll.Replace(arrStr[0], "");
            int[] nums = Array.ConvertAll(arrStr[0].Split('d'), int.Parse);

            if (nums.Length > 1)
            {
                str = string.Format("{0}d{1} = (", nums[0], nums[1]);
                for (int i = 0; i < nums[0]; i++)
                {
                    int arg = rand.Next(1, nums[1] + 1);
                    sum += arg;
                    str += arg + (i != nums[0] - 1 ? " + " : string.Empty);
                }
                str += ") = " + sum;
            }
            else if (nums.Length > 0)
            {
                int arg = rand.Next(1, nums[0] + 1);
                str = string.Format("1d{0} = {1}", arrStr[0], arg);
            }
            if (comment != null && comment != string.Empty) str += "\n" + comment;

            if (str != string.Empty)
            {
                await Context.Channel.SendMessageAsync(str).ConfigureAwait(false);
            }
        }
    }
}

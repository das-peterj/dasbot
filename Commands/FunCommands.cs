using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using HtmlAgilityPack;
using QuickChart;
using System;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    public class FunCommands : BaseCommandModule
    {

        [Command("ping")]
        [Description("Sends a pong back to the user along with the user's username. \n" +
            "This helps to identify whetever the bot's online and running.")]
        public async Task Ping(CommandContext ctx)
        {
            var user = ctx.Member.Username;
            await ctx.Channel.SendMessageAsync("Pong! " + user).ConfigureAwait(false);
        }

        [Command("addition")]
        [Description("Adds two numbers together and outputs the result of the two integers.")]
        public async Task Addition(CommandContext ctx,
            [Description("First integer")] int numberOne,
            [Description("Second integer")] int numberTwo)
        {
            await ctx.Channel.SendMessageAsync(
                numberOne + " + " + numberTwo + " = " + (numberOne + numberTwo).ToString())
                .ConfigureAwait(false);
        }

        [Command("8ball")]
        [Description("Answers the user's question. Uses an highly complicated Artifical Intelligence algorithm.")]
        public async Task EightBall(CommandContext ctx)
        {
            Random rnd = new Random();
            string[] prompt = {"Yes", "No", "Perhaps", "Possibly", "Probably not", "Not likely",
            "Most definitely", "Absolutely", "Dead wrong", "Nah not possible m8", "Absolutely not"};
            int x = rnd.Next(prompt.Length);
            await ctx.Channel.SendMessageAsync(prompt[x] + ".")
                .ConfigureAwait(false);
        }
    }
}
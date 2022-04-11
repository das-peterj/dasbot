﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using HtmlAgilityPack;
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

        [Command("add")]
        [Description("Adds two numbers together and outputs the result of the two integers.")]
        public async Task Add(CommandContext ctx,
            [Description("First integer")] int numberOne,
            [Description("Second integer")] int numberTwo)
        {
            await ctx.Channel.SendMessageAsync(
                numberOne + " + " + numberTwo + " = " + (numberOne + numberTwo).ToString())
                .ConfigureAwait(false);
        }

        [Command("isbelanorweigan")]
        [Description("Tells the user is belmont is a norweigan or not. May or may not be an inside joke.")]
        public async Task Belmont(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Belmont is in fact an norweigan, unfortunately.")
                .ConfigureAwait(false);
        }

        [Command("doesfryhaveabigpp")]
        [Description("Tells the user if fry has a big pp or not.")]
        public async Task Fry(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Fry does indeed have a pretty large pp.")
                .ConfigureAwait(false);
        }

        [Command("8ball")]
        [Description("Answers the user's question.")]
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
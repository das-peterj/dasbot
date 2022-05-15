using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

//using MahApps.Metro.Converters;
using System;
using System.Collections.Generic;
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
            //await ctx.Channel.SendMessageAsync("Pong! " + user + " your ping is ").ConfigureAwait(false);

            var inlineReplyMessage = await new DiscordMessageBuilder()
            .WithContent("Pong! " + ctx.Client.Ping + "ms")
            .WithReply(ctx.Message.Id, true)
            .SendAsync(ctx.Channel);
        }

        [Command("addition")]
        [Description("Adds two numbers together and outputs the result of the two integers.")]
        public async Task Addition(CommandContext ctx,
            [Description("First integer")] int numberOne,
            [Description("Second integer")] int numberTwo)
        {
            //await ctx.Channel.SendMessageAsync(
            //    numberOne + " + " + numberTwo + " = " + (numberOne + numberTwo).ToString())
            //    .ConfigureAwait(false);

            var inlineReplyMessage = await new DiscordMessageBuilder()
            .WithContent(numberOne + " + " + numberTwo + " = " + (numberOne + numberTwo).ToString())
            .WithReply(ctx.Message.Id, true)
            .SendAsync(ctx.Channel);
        }

        [Command("8ball")]
        [Description("Answers the user's question. Uses an highly complicated Artifical Intelligence algorithm.")]
        public async Task EightBall(CommandContext ctx)
        {
            Random rnd = new Random();
            string[] prompt = {"Yes", "No", "Perhaps", "Possibly", "Probably not", "Not likely",
            "Most definitely", "Absolutely", "Dead wrong", "Nah not possible m8", "Absolutely not"};
            int x = rnd.Next(prompt.Length);

            //await ctx.Channel.SendMessageAsync(prompt[x] + ".")
            //    .ConfigureAwait(false);

            var inlineReplyMessage = await new DiscordMessageBuilder()
                .WithContent(prompt[x] + ".")
                .WithReply(ctx.Message.Id, true)
                .SendAsync(ctx.Channel);
        }

        [Command("support")]
        [Description("Consider donating to help cover the cost of hosting the bot.")]
        public async Task Support(CommandContext ctx)
        {
            var myButton = new DiscordLinkButtonComponent("https://www.paypal.me/dasbomber", "🥰", false);
            var builder = new DiscordMessageBuilder();
            builder.WithContent("We appreciate you thinking about supporting us FeelsGoodMan").AddComponents(myButton);

            await ctx.Channel.SendMessageAsync(builder);
        }

        [Command("dropdown")]
        [Description("Test command to test dropdown menus")]
        public async Task Dropdown(CommandContext ctx)
        {
            var dropdownOptions = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("Dasbomber, The sexiest", "Das is indeed the sexiest out of the bunch"),
                new DiscordSelectComponentOption("Loitering, The wisest", "Loit is indeed the wisest out of the bunch"),
                new DiscordSelectComponentOption("Belmont, The oldest", "Bel is indeed the oldest out of the bunch"),
            };

            var dropdownMenu = new DiscordSelectComponent("dropdownMenu", null, dropdownOptions, false, 1, 1);

            var builder = new DiscordMessageBuilder().WithContent("Look, it's a dropdown menu containing facts.").AddComponents(dropdownMenu);
            await builder.SendAsync(ctx.Channel);

            ctx.Client.ComponentInteractionCreated += async (s, e) =>
            {
                //await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                //    new DiscordInteractionResponseBuilder().WithContent("Nämen tjenare"));
                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            };
        }

        /*
         Not currently working for whatever reason
        [Command("math")]
        [Description("Gives the user the option too choose which operation too use and what two operands.")]
        public async Task Math(CommandContext ctx, [Description("Operation to perform on the operands")] MathOperation operation, [Description("First operand")] double num1, [Description("Second operand")] double num2)
        {
            var result = 0.0;
            switch (operation)
            {
                case MathOperation.Add:
                    result = num1 + num2;
                    break;

                case MathOperation.Subtract:
                    result = num1 - num2;
                    break;

                case MathOperation.Multiply:
                    result = num1 * num2;
                    break;

                case MathOperation.Divide:
                    result = num1 / num2;
                    break;

                case MathOperation.Modulo:
                    result = num1 % num2;
                    break;
            }

            var emoji = DiscordEmoji.FromName(ctx.Client, ":1234:");
            await ctx.RespondAsync($"{emoji} The result is {result.ToString("#,##0.00")}");
        }
        */
    }
}
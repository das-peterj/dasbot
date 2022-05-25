using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

//using MahApps.Metro.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //var user = ctx.Member.Username;
            //await ctx.Channel.SendMessageAsync("Pong! " + user + " your ping is ").ConfigureAwait(false);

            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

            string timeSinceMsgSent = "<t:" + unixTime + ":R>";

            var inlineReplyMessage = await new DiscordMessageBuilder()
            .WithContent("Pong! " + ctx.Client.Ping + "ms" + "\n" + timeSinceMsgSent)
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
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

            string timeSinceMsgSent = "<t:" + unixTime + ":R>";

            var myButton = new DiscordLinkButtonComponent("https://www.paypal.me/dasbomber", "🥰", false);
            var builder = new DiscordMessageBuilder();
            builder.WithContent("We appreciate you thinking about supporting us FeelsGoodMan\n" + timeSinceMsgSent).AddComponents(myButton);

            await ctx.Channel.SendMessageAsync(builder);
        }

        [Command("dropdown")]
        [Description("Test command to test dropdown menus")]
        public async Task Dropdown(CommandContext ctx)
        {
            var dropdownOptions = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("Dasbomber, The Sexiest", "1", "Das is indeed the sexiest out of the bunch", true),
                new DiscordSelectComponentOption("Loitering, The Wisest", "2", "Loit is indeed the wisest out of the bunch"),
                new DiscordSelectComponentOption("Belmont, The Oldest", "3", "Bel is indeed the oldest out of the bunch"),
                new DiscordSelectComponentOption("Fryguy, The Coolest", "4", "Fry is indeed the coolest out of the bunch"),
                new DiscordSelectComponentOption("Falcon, The Alcohol Expert", "5", "Falcon is indeed the alcohol expert out of the bunch"),
                new DiscordSelectComponentOption("Basti, The Dumbest", "6", "Basti is indeed the dumbest out of the bunch"),
                new DiscordSelectComponentOption("Laquarix, The Cutest", "7", "Laq is indeed the cutest out of the bunch"),
                new DiscordSelectComponentOption("KingMadTheSad, The boring one", "8", "King is indeed the most boring one out of the bunch"),
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

        [Command("dice")]
        [Description("Rolls a dice based off the lower and upper limit chosen by the user")]
        public async Task RollDice(CommandContext ctx, int lowerLimit, int upperLimit)
        {
            if (lowerLimit > upperLimit)
            {
                await ctx.Channel.SendMessageAsync("Error, lowerlimit of " + lowerLimit + " can't be higher than upperLimit of " + upperLimit);
            }
            else
            {
                System.Random random = new System.Random();
                int randomNumber = random.Next(lowerLimit, upperLimit + 1); // +1 here because otherwise randomNumber cant ever be equal to upperLimit. Integers n dat

                var inlineReplyMessage = await new DiscordMessageBuilder()
                    .WithContent("Dice has rolled the number `" + randomNumber + "`. Thanks for playing!")
                    .WithReply(ctx.Message.Id, true)
                    .SendAsync(ctx.Channel);
            }
        }

        private DiscordEmoji[] emojiCache;

        [Command("poll")]
        [Description("Create a poll for a yes/no question")]
        public async Task PollMaker(CommandContext ctx,
            [Description("How long should the poll last?")] TimeSpan duration,
            [Description("What options should people have.")] params DiscordEmoji[] choices)
        {
            var clientInteractivity = ctx.Client.GetInteractivity();

            var promptQuestion = await ctx.Channel.SendMessageAsync("Type your question, must be longer than 5 characters and end with a \"?\" ");

            var msg = await clientInteractivity.WaitForMessageAsync(x => x.Content.Length > 5 && x.Content.Contains("?"));
            await ctx.Channel.DeleteMessageAsync(promptQuestion);

            if (msg.Result?.Content.ToLowerInvariant() != null)
            {
                var pollChoices = choices.Select(emoji => emoji.ToString());

                DateTime foo = DateTime.Now;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

                string checkDuration = "<t:" + unixTime + ":R>";

                var embed = new DiscordEmbedBuilder
                {
                    Title = msg.Result.Content.ToString(),
                    Description = string.Join(" ", pollChoices)
                };

                var pollStartMsg = await ctx.RespondAsync(embed);

                for (int i = 0; i < choices.Length; i++)
                {
                    await pollStartMsg.CreateReactionAsync(choices[i]);
                }

                var countdownTimer = await ctx.Channel.SendMessageAsync(checkDuration);
                var pollResults = await clientInteractivity.CollectReactionsAsync(pollStartMsg, duration);

                await ctx.Channel.DeleteMessageAsync(countdownTimer);
                var result = pollResults.Where(emoji => choices.Contains(emoji.Emoji)).Select(emoji => emoji.Emoji + ": " + emoji.Total);

                await ctx.Channel.SendMessageAsync(msg.Result.Content.ToString() + "\n" + result.ToString());
            }
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
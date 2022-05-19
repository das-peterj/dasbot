using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;

//using MahApps.Metro.Converters;
using System;
using System.Collections.Generic;
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
        [Cooldown(2, 30, CooldownBucketType.Guild)]
        public async Task PollMaker(CommandContext ctx, [Description("How long should the poll last?")] TimeSpan duration, [Description("Yes/No Question"), RemainingText] string question)
        {
            var client = ctx.Client;
            var clientInteractivity = client.GetInteractivity();

            if (!string.IsNullOrEmpty(question))
            {
                if (emojiCache == null)
                {
                    Console.Write("\n0\n");
                    emojiCache = new[]
                    {
                        DiscordEmoji.FromName(client, ":ballot_box_with_check:"),
                        DiscordEmoji.FromName(client, ":x:"),
                    };
                }

                // Making the poll and adding the user's question onto it
                var pollQuestion = new StringBuilder();
                pollQuestion.Append("**").Append("(Not working atm)Poll starting for: ").AppendLine("**");
                pollQuestion.Append(question);

                var pollStartMsg = await ctx.RespondAsync(pollQuestion.ToString());

                // Collecting the poll results
                var pollResults = await client.GetInteractivity().DoPollAsync(pollStartMsg, emojiCache, PollBehaviour.DeleteEmojis, duration);

                var votesYes = pollResults[0].Total;
                var votesNo = pollResults[1].Total;

                var pollResult = new StringBuilder();
                pollResult.AppendLine(question);
                pollResult.Append("Poll results are: ");
                pollResult.Append("**");

                if (votesYes < votesNo)
                {
                    pollResult.Append("NO");
                }
                else if (votesYes > votesNo)
                {
                    pollResult.Append("YES");
                }
                else
                {
                    pollResult.Append("Equal votes");
                }
                pollResult.Append("**");
                await ctx.RespondAsync(pollResult.ToString());
            }
            else
            {
                await ctx.RespondAsync("Error: The question can't be empty, stupid");
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
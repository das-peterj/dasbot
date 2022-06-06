using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

// https://dsharpplus.github.io/articles/commands/help_formatter.html

namespace DiscordBot_Dasbot.Core
{
    internal class CustomHelpFormatter : BaseHelpFormatter
    {
        protected DiscordEmbedBuilder embed;
        protected StringBuilder strBuilder;
        private DiscordEmbedFooter footer;
        private CommandContext context;

        //private List<DiscordSelectComponentOption> options = new List<DiscordSelectComponentOption>()
        //    {
        //        new DiscordSelectComponentOption("Test", "Teeest"),
        //    };

        public CustomHelpFormatter(CommandContext ctx) : base(ctx)
        {
            DateTime date = DateTime.Now;

            embed = new DiscordEmbedBuilder().WithFooter($"Support me at https://www.paypal.me/dasbomber\n{date}\nMade by Peter Jörgensen")
                .WithUrl("https://www.paypal.me/dasbomber");
            strBuilder = new StringBuilder();
            var channel = ctx.Channel;
            context = ctx;
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            embed.AddField(command.Name, command.Description);
            strBuilder.AppendLine($"{command.Name} - {command.Description}");
            //options = new List<DiscordSelectComponentOption>()
            //    {
            //        new DiscordSelectComponentOption(command.Name, command.Description),
            //    };

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
        {
            foreach (var cmd in cmds)
            {
                embed.AddField(cmd.Name, cmd.Description);
                strBuilder.AppendLine($"{cmd.Name} - {cmd.Description}");

                //options = new List<DiscordSelectComponentOption>()
                //{
                //    new DiscordSelectComponentOption(cmd.Name, cmd.Description),
                //};

                //Console.Write(cmd.Name + cmd.Description + "xxxxx\n");
            }

            return this;
        }

        public override CommandHelpMessage Build()
        {
            embed.Color = DiscordColor.Gold;
            embed.Title = "Dasbot's commands";

            //HelpCommandAsync();
            //WaitForHelp();
            return new CommandHelpMessage(embed: embed);
            //return new CommandHelpMessage(content: strBuilder.ToString());
        }

        public async Task HelpCommandAsync()
        {
            //DiscordSelectComponent dropdown = new DiscordSelectComponent("dropdown", null, options, false, 1, 1);

            ////var builder = new DiscordMessageBuilder().WithContent("Look below for the help list").AddComponents(dropdown);

            ////await builder.SendAsync(context.Channel);
            //var inlineReplyMessage = await new DiscordMessageBuilder()
            //.WithContent("help list").AddComponents(dropdown)
            //.WithReply(context.Message.Id, true)
            //.SendAsync(context.Channel);
        }

        public async Task WaitForHelp()
        {
            await Task.Delay(2000);
        }
    }
}
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Text;
using System.Linq;


// https://dsharpplus.github.io/articles/commands/help_formatter.html

namespace DiscordBot_Dasbot.Core
{
    internal class CustomHelpFormatter : BaseHelpFormatter
    {
        protected DiscordEmbedBuilder embed;
        protected StringBuilder strBuilder;
        DiscordEmbedFooter footer;
        public CustomHelpFormatter(CommandContext ctx) : base(ctx)
        {
            embed = new DiscordEmbedBuilder();
            strBuilder = new StringBuilder();
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            embed.AddField(command.Name, command.Description);
            strBuilder.AppendLine($"{command.Name} - {command.Description}");

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
        {
            foreach (var cmd in cmds)
            {
                embed.AddField(cmd.Name, cmd.Description);
                strBuilder.AppendLine($"{cmd.Name} - {cmd.Description}");
            }

            return this;
        }

        public override CommandHelpMessage Build()
        {
            embed.Color = DiscordColor.Azure;
            embed.Title = "Dasbot's commands";
            return new CommandHelpMessage(embed: embed);
            //return new CommandHelpMessage(content: strBuilder.ToString());
        }
    }
}
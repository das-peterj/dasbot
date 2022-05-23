using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    class AdminCommands : BaseCommandModule
    {
        [Hidden]
        [Command("setactivity")]
        [Description("Sets the bot activity status")]
        // Note that the input is from the console window
        private async Task SetActivity(CommandContext ctx)
        {
            if (ctx.User.Id == 235347702861398026)
            {
                DiscordActivity activity = new DiscordActivity();
                DiscordClient discord = ctx.Client;
                string input = Console.ReadLine();
                activity.ActivityType = ActivityType.ListeningTo;
                activity.Name = input;
                await discord.UpdateStatusAsync(activity);
                return;
            }
            else
            {
                return;
            }
        }


        [Command("infouser")]
        [Description("Gathers information regarding a user")]
        private async Task InfoUser(CommandContext ctx, DiscordMember member)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = "Information regarding " + member.Username,
                Description = "Account creation date: `" + member.CreationTimestamp.DateTime.ToString(CultureInfo.InvariantCulture) + "`\n" +
                "Joined " + ctx.Guild.Name + " on the `" + member.JoinedAt.DateTime.ToString(CultureInfo.InvariantCulture) + "`",
                Color = DiscordColor.Blurple,
            };

            // Adding Footer = "" to above doesn't work
            embed.WithFooter(ctx.Guild.Name + " " + ctx.Channel.Name + " " + DateTime.Now);

            await ctx.Channel.SendMessageAsync(embed);
        }
    }
}

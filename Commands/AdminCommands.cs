using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Globalization;
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
                Title = $"Information regarding {member.Username}#{member.Discriminator}",
                Description = $"Account creation date: `{member.CreationTimestamp.DateTime.ToString(CultureInfo.GetCultureInfo("SE"))}`\nJoined {ctx.Guild.Name} on the `{member.JoinedAt.DateTime.ToString(CultureInfo.GetCultureInfo("SE"))}`",
                Color = DiscordColor.Blurple,
            };

            embed.WithFooter($"{ctx.Guild.Name} {ctx.Channel.Name} {DateTime.Now}");
            embed.WithThumbnail(member.GetAvatarUrl(ImageFormat.Auto));

            await ctx.Channel.SendMessageAsync(embed);
        }


    }
}

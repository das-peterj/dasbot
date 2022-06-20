using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    class ConverterCommands : BaseCommandModule
    {
        [Command("convertTemp")]
        [Description("Convert fahrenheit/celsius to the opposite value")]
        public async Task ConvertTemperature(CommandContext ctx,
            [Description("What temperature scale would you like to convert to?")] String tempToConvertTo,
            [Description("Insert the value")] double tempValue)
        {

            if (tempToConvertTo.ToLower().Contains("celsius") || tempToConvertTo.ToLower().Contains("c"))
            {
                var convertedValue = string.Format("{0:0.00}", (tempValue - 32) * 5/9);
                await ctx.Channel.SendMessageAsync($"{tempValue} °F = {convertedValue} °C");
            } else if (tempToConvertTo.ToLower().Contains("fahrenheit") || tempToConvertTo.ToLower().Contains("f"))
            {
                var convertedValue = string.Format("{0:0.00}", (tempValue * 9 / 5) + 32);
                await ctx.Channel.SendMessageAsync($"{tempValue} °C = {convertedValue} °F");
            } else
            {
                await ctx.Channel.SendMessageAsync($"Error occured {ctx.Member.Username}, only fahrenheit/f || celsius/c are acceptable inputs.");
            }
        }

        [Command("convertWeight")]
        [Description("Convert pound/kilo to the opposite value")]
        public async Task ConvertWeight(CommandContext ctx,
            [Description("What weight scale would you like to convert to?")] String weightToConvertTo,
            [Description("Insert the value")] double weightValue)
        {
            if (weightToConvertTo.ToLower().Contains("kg") || weightToConvertTo.ToLower().Contains("kgs") || weightToConvertTo.ToLower().Contains("kilogram") || weightToConvertTo.ToLower().Contains("kilograms"))
            {
                var convertedValue = string.Format("{0:0.00}", weightValue / 2.205);
                await ctx.Channel.SendMessageAsync($"{weightValue} lbs = {convertedValue} kgs");
            } else if (weightToConvertTo.ToLower().Contains("lb") || weightToConvertTo.ToLower().Contains("lbs") || weightToConvertTo.ToLower().Contains("pound") || weightToConvertTo.ToLower().Contains("pounds"))
            {
                var convertedValue = string.Format("{0:0.00}", weightValue * 2.205);
                await ctx.Channel.SendMessageAsync($"{weightValue} kgs = {convertedValue} lbs");
            } else
            {
                await ctx.Channel.SendMessageAsync($"Error occured {ctx.Member.Username}, only kg/kgs/kilogram/kilograms || lb/lbs/pound/pounds are acceptable inputs.");
            }
        }
    }
}

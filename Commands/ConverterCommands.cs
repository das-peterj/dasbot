using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    internal class ConverterCommands : BaseCommandModule
    {
        [Command("convertTemp")]
        [Description("Convert fahrenheit/celsius to the opposite value")]
        public async Task ConvertTemperature(CommandContext ctx,
            [Description("What temperature scale would you like to convert from?")] String tempToConvertFrom,
            [Description("Insert the value")] double tempValue)
        {
            if (tempToConvertFrom.ToLower().Contains("celsius") || tempToConvertFrom.ToLower().Contains("c"))
            {
                var convertedValue = string.Format("{0:0.00}", (tempValue * 9 / 5) + 32);
                await ctx.Channel.SendMessageAsync($"{tempValue} °C = {convertedValue} °F");
            }
            else if (tempToConvertFrom.ToLower().Contains("fahrenheit") || tempToConvertFrom.ToLower().Contains("f"))
            {
                var convertedValue = string.Format("{0:0.00}", (tempValue - 32) * 5 / 9);
                await ctx.Channel.SendMessageAsync($"{tempValue} °F = {convertedValue} °C");
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"Error occured {ctx.Member.Username}, only fahrenheit/f || celsius/c are acceptable inputs.");
            }
        }

        [Command("convertWeight")]
        [Description("Convert pound/kilo to the opposite value")]
        public async Task ConvertWeight(CommandContext ctx,
            [Description("What weight scale would you like to convert to?")] String weightToConvertFrom,
            [Description("Insert the value")] double weightValue)
        {
            if (weightToConvertFrom.ToLower().Contains("kg") || weightToConvertFrom.ToLower().Contains("kgs") || weightToConvertFrom.ToLower().Contains("kilogram") || weightToConvertFrom.ToLower().Contains("kilograms"))
            {
                var convertedValue = string.Format("{0:0.00}", weightValue * 2.205);
                await ctx.Channel.SendMessageAsync($"{weightValue} kgs = {convertedValue} lbs");
            }
            else if (weightToConvertFrom.ToLower().Contains("lb") || weightToConvertFrom.ToLower().Contains("lbs") || weightToConvertFrom.ToLower().Contains("pound") || weightToConvertFrom.ToLower().Contains("pounds"))
            {
                var convertedValue = string.Format("{0:0.00}", weightValue / 2.205);
                await ctx.Channel.SendMessageAsync($"{weightValue} lbs = {convertedValue} kgs");
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"Error occured {ctx.Member.Username}, only kg/kgs/kilogram/kilograms || lb/lbs/pound/pounds are acceptable inputs.");
            }
        }

        [Command("convertHeight")]
        [Description("Convert feet/inches & cm/m to the opposite value")]
        public async Task ConvertHeight(CommandContext ctx,
            [Description("What value to convert from")] String heightToConvertFrom,
            [Description("Insert the value")] double heightValue)
        {
            if (heightToConvertFrom.ToLower().Contains("cm"))
            {
                var convertedValueInFeet = string.Format("{0:0.00}", heightValue / 30.48);
                if (double.Parse(convertedValueInFeet) < 1)
                {
                    var convertedValueInInches = string.Format("{0:0.00}", heightValue / 2.54);
                    await ctx.Channel.SendMessageAsync($"{heightValue} cm = {convertedValueInInches} inches");
                }
                else
                {
                    await ctx.Channel.SendMessageAsync($"{heightValue} cm = {convertedValueInFeet} ft");
                }
            }
            else if (heightToConvertFrom.ToLower().Contains("m"))
            {
                var convertedValueInFeet = string.Format("{0:0.00}", heightValue * 3.281);
                if (double.Parse(convertedValueInFeet) < 1)
                {
                    var convertedValueInInches = string.Format("{0:0.00}", heightValue / 39.37);
                    await ctx.Channel.SendMessageAsync($"{heightValue} m = {convertedValueInInches} inches");
                }
                else
                {
                    await ctx.Channel.SendMessageAsync($"{heightValue} m = {convertedValueInFeet} ft");
                }
            }


        }
    }
}
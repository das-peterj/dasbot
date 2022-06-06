using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Core
{
    public class MathOperationConverter : IArgumentConverter<MathOperation>
    {
        public Task<Optional<MathOperation>> ConvertAsync(string value, CommandContext ctx)
        {
            return value switch
            {
                "+" => Task.FromResult(Optional.FromValue(MathOperation.Add)),
                "-" => Task.FromResult(Optional.FromValue(MathOperation.Subtract)),
                "*" => Task.FromResult(Optional.FromValue(MathOperation.Multiply)),
                "/" => Task.FromResult(Optional.FromValue(MathOperation.Divide)),
                "%" => Task.FromResult(Optional.FromValue(MathOperation.Modulo)),
                _ => Task.FromResult(Optional.FromValue(MathOperation.Add))
            };
        }
    }
}

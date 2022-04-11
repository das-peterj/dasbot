using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    public class FunCommands : BaseCommandModule
    {
        private string[] powerIndex;
        private string[] powerName;
        private int[] powerPower;

        [Command("webtest")]
        public async Task WebTest(CommandContext ctx, string url)
        {
            var html = @url;

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);
            var nodeTitle = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            //var id = "example_wrapper";
            //var query = $"//body/div[@id='{id}']";
            var nodePowerName = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='example']");
            int indexId = 0;
            int indexName = 0;
            int indexPower = 0;

            //string test = "12345678";
            //Console.WriteLine("test: " + test.Length);

            foreach (HtmlNode row in nodePowerName.SelectNodes("//tr"))
            {
                //Console.WriteLine("New Row");

                foreach (HtmlNode cell in row.SelectNodes("//td"))
                {
                    // TODO: First cell.innertext shows length of 53, supposed to be either 1 or 2.
                    // possible it shows 53 because of the first 3 rows have emojis instead of numbers?
                    Console.WriteLine("test" + cell.InnerText.Length);

                    if (cell.InnerText.Length <= 2)
                    {
                        Console.WriteLine("id: " + cell.InnerText);
                        powerIndex[indexId] = cell.InnerText;
                        indexId += 1;
                        //await ctx.Channel.SendMessageAsync("cell: " + cell.InnerText);
                    }
                    if (cell.InnerText.Length >= 3)
                    {
                        Console.WriteLine("name: " + cell.InnerText);
                        powerName[indexName] = cell.InnerText;
                        indexName += 1;
                    }
                    if (cell.InnerText.Length >= 7)
                    {
                        Console.WriteLine("power: " + cell.InnerText);
                        powerPower[indexPower] = Int32.Parse(cell.InnerText);
                        indexPower += 1;
                    } else
                    {
                        Console.WriteLine("Something went wrong here.");
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");

            //for (int i = 0; i < powerIndex.Length; i++)
            //{
            //    Console.WriteLine("id: " + powerIndex[i] + " | name: " + powerName[i] + " | power: " + powerPower[i]);
            //}


            //Console.WriteLine("Node Name: " + nodeTitle.Name + "\n" + nodeTitle.OuterHtml);
            await ctx.Channel.SendMessageAsync("Node Name: " + nodeTitle.Name + "\n" + nodeTitle.OuterHtml + "\n" + "\n" + "\n");
            //await ctx.Channel.SendMessageAsync(nodePowerName);
        }

        [Command("ping")]
        [Description("Sends a pong back to the user along with the user's username. \n" +
            "This helps to identify whetever the bot's online and running.")]
        public async Task Ping(CommandContext ctx)
        {
            var user = ctx.Member.Username;
            await ctx.Channel.SendMessageAsync("Pong! " + user).ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers together and outputs the result of the two integers.")]
        public async Task Add(CommandContext ctx,
            [Description("First integer")] int numberOne,
            [Description("Second integer")] int numberTwo)
        {
            await ctx.Channel.SendMessageAsync(
                numberOne + " + " + numberTwo + " = " + (numberOne + numberTwo).ToString())
                .ConfigureAwait(false);
        }

        [Command("isbelanorweigan")]
        [Description("Tells the user is belmont is a norweigan or not. May or may not be an inside joke.")]
        public async Task Belmont(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Belmont is in fact an norweigan, unfortunately.")
                .ConfigureAwait(false);
        }

        [Command("doesfryhaveabigpp")]
        [Description("Tells the user if fry has a big pp or not.")]
        public async Task Fry(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Fry does indeed have a pretty large pp.")
                .ConfigureAwait(false);
        }

        [Command("8ball")]
        [Description("Answers the user's question.")]
        public async Task EightBall(CommandContext ctx)
        {
            Random rnd = new Random();
            string[] prompt = {"Yes", "No", "Perhaps", "Possibly", "Probably not", "Not likely",
            "Most definitely", "Absolutely", "Dead wrong", "Nah not possible m8", "Absolutely not"};
            int x = rnd.Next(prompt.Length);
            await ctx.Channel.SendMessageAsync(prompt[x] + ".")
                .ConfigureAwait(false);
        }
    }
}
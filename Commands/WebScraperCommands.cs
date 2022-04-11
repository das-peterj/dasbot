using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    public class WebScraperCommands : BaseCommandModule
    {
        private string[] powerIndex = new String[30];
        private string[] powerName = new String[30];
        private int[] powerPower = new int[30];

        private int indexId = 0;
        private int indexName = 0;
        private int indexPower = 0;

        int rankId = 1;

        //private bool hasCheckedCell = false;

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

            //string test = "12345678";
            //Console.WriteLine("test: " + test.Length);

            foreach (HtmlNode row in nodePowerName.SelectNodes("//tbody/tr"))
            {
                //Console.WriteLine("New Row");

                foreach (HtmlNode cell in row.SelectNodes("//td"))
                {
                    // TODO: First cell.innertext shows length of 53, supposed to be either 1 or 2.
                    // possible it shows 53 because of the first 3 rows have emojis instead of numbers?

                    string input = cell.InnerHtml.ToString().Trim();

                    if (cell.InnerHtml.ToString().Trim().Length > 30)
                    {
                        int inputIndex = input.IndexOf(" ");
                        if (inputIndex >= 0)
                        {
                            input = input.Substring(0, inputIndex);
                        }
                    }

                    if (input.Contains("<img"))
                    {
                        input = "" + rankId;
                        rankId++;
                    }
                    await ctx.Channel.SendMessageAsync(input);
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
    }
}
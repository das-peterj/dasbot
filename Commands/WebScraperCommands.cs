using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    public class WebScraperCommands : BaseCommandModule
    {
        private float[] powerIndex = new float[30];
        private string[] powerName = new String[30];
        private double[] powerPower = new double[30];

        private int indexId = 0;
        private int indexName = 0;
        private int indexPower = 0;

        private int rankId = 1;

        //private bool hasCheckedCell = false;

        [Command("gp")]
        public async Task CheckGuildPower(CommandContext ctx, string guildName)
        {
            for (int i = 0; i < powerIndex.Length; i++)
            {
                powerIndex[i] = 8008;
                powerName[i] = "Not Found";
                powerPower[i] = 8008.5;
            }

            var html = @"http://15650.gzidlerpg.appspot.com/web/scores?tid=220110001&guildTag=" + guildName;
            Console.WriteLine(html);
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);
            var nodeTitle = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            //var id = "example_wrapper";
            //var query = $"//body/div[@id='{id}']";
            var nodePowerName = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='example']");

            //string test = "12345678";
            //Console.WriteLine("test: " + test.Length);
            int count = 0;
            int countIndex = 0;
            int countId = 0;

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

                    if (count <= 3)
                    {
                        if (countIndex == 0)
                        {
                            powerIndex[countId] = int.Parse(input);
                            Console.Write(powerIndex[countId]);
                            countIndex++;
                        }
                        else if (countIndex == 1)
                        {
                            //if (input.Length < 15)
                            //{
                            //    for (int i = 0; i < 15 - input.Length; i++)
                            //    {
                            //        string space = " ";
                            //        input += space;
                            //    }
                            //}
                            input += "\t";
                            powerName[countId] = input;
                            Console.Write(powerName[countId]);
                            countIndex++;
                        }
                        else if (countIndex == 2)
                        {
                            powerPower[countId] = double.Parse(string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", input));
                            Console.Write(powerPower[countId]);
                            countIndex++;
                        }
                        if (countId > 31) { break; }
                        count++;
                        //Console.WriteLine("countId" + countId);
                        if (count == 3)
                        {
                            if (countId < 30)
                            {
                                count = 0;
                                countIndex = 0;
                                countId++;
                            }
                        }
                    }
                }
                break;
            }

            double guildTotalPower = 0;
            for (int i = 0; i < powerPower.Length; i++)
            {
                guildTotalPower += powerPower[i];
            }

            char pad = ' ';
            //string formattedGuildTotPower = guildTotalPower.ToString();
            string formattedGuildTotPower = string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", guildTotalPower);

            var embed = new DiscordEmbedBuilder
            {
                Title = guildName + " | " + formattedGuildTotPower + " total guild power",
                Description = "`Rank " + powerIndex[0] + "  | " + powerName[0].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[0]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[1] + "  | " + powerName[1].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[1]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[2] + "  | " + powerName[2].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[2]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[3] + "  | " + powerName[3].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[3]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[4] + "  | " + powerName[4].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[4]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[5] + "  | " + powerName[5].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[5]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[6] + "  | " + powerName[6].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[6]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[7] + "  | " + powerName[7].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[7]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[8] + "  | " + powerName[8].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[8]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[9] + " | " + powerName[9].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[9]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[11] + " | " + powerName[11].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[11]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[10] + " | " + powerName[10].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[10]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[12] + " | " + powerName[12].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[12]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[13] + " | " + powerName[13].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[13]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[14] + " | " + powerName[14].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[14]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[15] + " | " + powerName[15].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[15]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[16] + " | " + powerName[16].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[16]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[17] + " | " + powerName[17].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[17]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[18] + " | " + powerName[18].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[18]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[19] + " | " + powerName[19].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[19]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[20] + " | " + powerName[20].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[20]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[21] + " | " + powerName[21].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[21]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[22] + " | " + powerName[22].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[22]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[23] + " | " + powerName[23].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[23]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[24] + " | " + powerName[24].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[24]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[25] + " | " + powerName[25].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[25]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[26] + " | " + powerName[26].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[26]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[27] + " | " + powerName[27].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[27]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[28] + " | " + powerName[28].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[28]) + "`" + System.Environment.NewLine +
                "`Rank " + powerIndex[29] + " | " + powerName[29].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[29]) + "`" + System.Environment.NewLine,
            };

            await ctx.Channel.SendMessageAsync(embed);
            powerIndex = new float[30];
            powerName = new String[30];
            powerPower = new double[30];
            countId = 0;
            //for (int i = 0; i < powerIndex.Length; i++)
            //{
            //    await ctx.Channel.SendMessageAsync("Rank " + powerIndex[i] + " | " + powerName[i] + " | Power: " + powerPower[i]);
            //}

            //Console.WriteLine("Node Name: " + nodeTitle.Name + "\n" + nodeTitle.OuterHtml);
            //await ctx.Channel.SendMessageAsync("Node Name: " + nodeTitle.Name + "\n" + nodeTitle.OuterHtml + "\n" + "\n" + "\n");
            //await ctx.Channel.SendMessageAsync(nodePowerName);
        }
    }

    public sealed class DiscordEmbed
    {
        public string Title { get; }
        public string Description { get; }
        public IReadOnlyList<DiscordEmbedField> Fields { get; }
    }
}
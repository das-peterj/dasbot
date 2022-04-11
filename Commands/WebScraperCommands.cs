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

        private int rankId = 1;

        [Command("gp")]
        public async Task CheckGuildPower(CommandContext ctx, string guildName)
        {
            for (int i = 0; i < powerIndex.Length; i++)
            {
                powerIndex[i] = 80085;
                powerName[i] = "Not Found | Doublecheck you used the correct Guildtag";
                powerPower[i] = 80085;
            }

            var html = @"http://15650.gzidlerpg.appspot.com/web/scores?tid=220110001&guildTag=" + guildName;
            Console.WriteLine(html);
            HtmlWeb web = new HtmlWeb();

            LoadGuildInfo(guildName, ctx);

            var htmlDoc = web.Load(html);
            var nodeTitle = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            var nodePowerName = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='example']");

            int count = 0;
            int countIndex = 0;
            int countId = 0;
            int amountOfGuildMembers = 0;

            foreach (HtmlNode row in nodePowerName.SelectNodes("//tbody/tr"))
            {
                for (int i = 0; i < row.InnerLength; i++)
                {
                    amountOfGuildMembers++;
                }
                Console.WriteLine("Amount of guild members " + amountOfGuildMembers);

                if (amountOfGuildMembers < 500)
                {
                    foreach (HtmlNode cell in row.SelectNodes("//td"))
                    {
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
            }

            if (amountOfGuildMembers < 500)
            {
                double guildTotalPower = 0;
                for (int i = 0; i < powerPower.Length; i++)
                {
                    guildTotalPower += powerPower[i];
                }

                char pad = ' ';
                string formattedGuildTotPower = string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", guildTotalPower);

                var embedGuildInfo = new DiscordEmbedBuilder
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
                    "`Rank " + powerIndex[10] + " | " + powerName[10].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[10]) + "`" + System.Environment.NewLine +
                    "`Rank " + powerIndex[11] + " | " + powerName[11].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[11]) + "`" + System.Environment.NewLine +
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

                await ctx.Channel.SendMessageAsync(embedGuildInfo);
                powerIndex = new float[30];
                powerName = new String[30];
                powerPower = new double[30];
                countId = 0;
                rankId = 1;
            }
            else
            {
                var embedError = new DiscordEmbedBuilder
                {
                    Title = "Something went wrong",
                    Description = "Double check you used the correct guildtag, it's CaSe-SeNsItIvE.\n If you used the correct guildtag, contact Dasbomber#7777 with details."
                };

                await ctx.Channel.SendMessageAsync(embedError);
            }
        }
        public async void LoadGuildInfo(string guildName, CommandContext ctx)
        {
            var embedLoadingGuildInfo = new DiscordEmbedBuilder
            {
                Title = "Loading " + guildName + " members. Wait a few seconds."
            };

            DiscordMessage msg = await ctx.Channel.SendMessageAsync(embedLoadingGuildInfo);
            await Task.Delay(8000);
            await ctx.Channel.DeleteMessageAsync(msg);

        }
    }
}
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace DiscordBot_Dasbot.Commands
{
    public class WebScraperCommands : BaseCommandModule
    {
        #region gp variables

        private float[] powerIndex = new float[30];
        private string[] powerName = new String[30];
        private double[] powerPower = new double[30];
        private int rankId = 1;

        #endregion gp variables

        #region gpcpr variables

        private float[] powerGuildOneIndex = new float[30];
        private string[] powerGuildOneName = new String[30];
        private double[] powerGuildOnePower = new double[30];
        private float[] powerGuildTwoIndex = new float[30];
        private string[] powerGuildTwoName = new String[30];
        private double[] powerGuildTwoPower = new double[30];
        private int amountOfHtmlCharactersGuildOne = 0;
        private int amountOfHtmlCharactersGuildTwo = 0;
        private bool hasRanCheckForGuildOne = false;

        #endregion gpcpr variables

        [Command("gpcpr")]
        public async Task CheckGuildsPower(CommandContext ctx, string guildOne, string guildTwo)
        {
            await GatherGuildInfoForComparisionAsync(ctx, guildOne, powerGuildOnePower, amountOfHtmlCharactersGuildOne);
            await GatherGuildInfoForComparisionAsync(ctx, guildTwo, powerGuildTwoPower, amountOfHtmlCharactersGuildTwo);

            double guildOneTotalPower = 0;
            for (int i = 0; i < powerGuildOnePower.Length; i++)
            {
                guildOneTotalPower += powerGuildOnePower[i];
            }

            double guildTwoTotalPower = 0;
            for (int i = 0; i < powerGuildTwoPower.Length; i++)
            {
                guildTwoTotalPower += powerGuildTwoPower[i];
            }

            char pad = ' ';
            string formattedGuildOneTotPower = string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", guildOneTotalPower);
            string formattedGuildTwoTotPower = string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", guildTwoTotalPower);
            string guildPowerDifference = ""; // in %
            string theStrongerGuild = "";
            string theWeakerGuild = "";
            CultureInfo ci = new CultureInfo("en-us");

            if (guildOneTotalPower >= guildTwoTotalPower)
            {
                guildPowerDifference = ((guildOneTotalPower/100) / (guildTwoTotalPower / 100)).ToString("##%", ci);
                theStrongerGuild = guildOne;
                theWeakerGuild = guildTwo;
            }
            else if (guildTwoTotalPower > guildOneTotalPower) {
                guildPowerDifference = ((guildTwoTotalPower / 100) / (guildOneTotalPower / 100)).ToString("##%", ci);
                theStrongerGuild = guildTwo;
                theWeakerGuild = guildOne;
            }
            int indexToRemove = 1;
            string formattedGuildPowerDifference = guildPowerDifference.Substring(indexToRemove);


            var embedGuildsComparision = new DiscordEmbedBuilder
            {
                Title = guildTwo.PadRight(10, pad) + " versus ".PadRight(10, pad) + guildOne + " ".PadRight(10, pad) + "Power Difference".PadRight(5, pad),
                Description =
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[0]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[0]).PadRight(5, pad) + "` | ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[0] - powerGuildOnePower[0])).PadRight(5, pad) + System.Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[1]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[1]).PadRight(5, pad) + "` | ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[1] - powerGuildOnePower[1])).PadRight(5, pad) + System.Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[2]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[2]).PadRight(5, pad) + "` | ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[2] - powerGuildOnePower[2])).PadRight(5, pad) + System.Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[3]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[3]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[3] - powerGuildOnePower[3])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[4]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[4]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[4] - powerGuildOnePower[4])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[5]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[5]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[5] - powerGuildOnePower[5])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[6]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[6]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[6] - powerGuildOnePower[6])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[7]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[7]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[7] - powerGuildOnePower[7])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[8]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[8]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[8] - powerGuildOnePower[8])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[9]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[9]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[9] - powerGuildOnePower[9])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[10]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[10]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[10] - powerGuildOnePower[10])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[11]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[11]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[11] - powerGuildOnePower[11])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[12]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[12]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[12] - powerGuildOnePower[12])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[13]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[13]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[13] - powerGuildOnePower[13])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[14]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[14]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[14] - powerGuildOnePower[14])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[15]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[15]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[15] - powerGuildOnePower[15])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[16]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[16]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[16] - powerGuildOnePower[16])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[17]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[17]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[17] - powerGuildOnePower[17])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[18]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[18]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[18] - powerGuildOnePower[18])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[19]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[19]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[19] - powerGuildOnePower[19])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[20]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[20]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[20] - powerGuildOnePower[20])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[21]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[21]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[21] - powerGuildOnePower[21])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[22]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[22]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[22] - powerGuildOnePower[22])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[23]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[23]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[23] - powerGuildOnePower[23])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[24]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[24]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[24] - powerGuildOnePower[24])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[25]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[25]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[25] - powerGuildOnePower[25])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[26]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[26]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[26] - powerGuildOnePower[26])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[27]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[27]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[27] - powerGuildOnePower[27])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[28]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[28]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[28] - powerGuildOnePower[28])) + Environment.NewLine +
                "`" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildTwoPower[29]).PadRight(5, pad) + " ".PadRight(5, pad) + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerGuildOnePower[29]) + "` | " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (powerGuildTwoPower[29] - powerGuildOnePower[29])) + Environment.NewLine +
                Environment.NewLine + guildTwo + ": " + formattedGuildTwoTotPower + " 🏆 | " + guildOne + ": " + formattedGuildOneTotPower + " 🏆" + Environment.NewLine +
                "Total guild power difference: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", (guildTwoTotalPower - guildOneTotalPower)) + Environment.NewLine +
                theStrongerGuild + " is " + formattedGuildPowerDifference + " stronger than " + theWeakerGuild
            };

            await ctx.Channel.SendMessageAsync(embedGuildsComparision);
            amountOfHtmlCharactersGuildOne = 0;
            amountOfHtmlCharactersGuildTwo = 0;
            powerGuildOnePower = new double[30];
            powerGuildTwoPower = new double[30];
        }

        [Command("gp")]
        public async Task CheckGuildPower(CommandContext ctx, string guildName)
        {
            for (int i = 0; i < powerIndex.Length; i++)
            {
                powerIndex[i] = 80085;
                powerName[i] = "Member doesn't exist, " + guildName + " aren't 30/30";
                powerPower[i] = 80085;
            }

            var html = @"http://15650.gzidlerpg.appspot.com/web/scores?tid=220110001&guildTag=" + guildName;
            //Console.WriteLine(html);
            HtmlWeb web = new HtmlWeb();

            LoadGuildInfo(guildName, ctx);

            var htmlDoc = web.Load(html);
            var nodeTitle = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            var nodePowerName = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='example']");

            int count = 0;
            int countIndex = 0;
            int countId = 0;
            int amountOfHtmlCharacters = 0;

            foreach (HtmlNode row in nodePowerName.SelectNodes("//tbody/tr"))
            {
                for (int i = 0; i < row.InnerLength; i++)
                {
                    amountOfHtmlCharacters++;
                }
                //Console.WriteLine("Amount of guild members " + amountOfHtmlCharacters);

                if (amountOfHtmlCharacters < 500)
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

            if (amountOfHtmlCharacters < 500)
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
                    Description =
                    powerIndex[0] + "  | " + powerName[0].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[0]) + "`" + System.Environment.NewLine +
                    powerIndex[1] + "  | " + powerName[1].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[1]) + "`" + System.Environment.NewLine +
                    powerIndex[2] + "  | " + powerName[2].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[2]) + "`" + System.Environment.NewLine +
                    powerIndex[3] + "  | " + powerName[3].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[3]) + "`" + System.Environment.NewLine +
                    powerIndex[4] + "  | " + powerName[4].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[4]) + "`" + System.Environment.NewLine +
                    powerIndex[5] + "  | " + powerName[5].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[5]) + "`" + System.Environment.NewLine +
                    powerIndex[6] + "  | " + powerName[6].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[6]) + "`" + System.Environment.NewLine +
                    powerIndex[7] + "  | " + powerName[7].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[7]) + "`" + System.Environment.NewLine +
                    powerIndex[8] + "  | " + powerName[8].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[8]) + "`" + System.Environment.NewLine +
                    powerIndex[9] + "  | " + powerName[9].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[9]) + "`" + System.Environment.NewLine +
                    powerIndex[10] + " | " + powerName[10].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[10]) + "`" + System.Environment.NewLine +
                    powerIndex[11] + " | " + powerName[11].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[11]) + "`" + System.Environment.NewLine +
                    powerIndex[12] + " | " + powerName[12].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[12]) + "`" + System.Environment.NewLine +
                    powerIndex[13] + " | " + powerName[13].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[13]) + "`" + System.Environment.NewLine +
                    powerIndex[14] + " | " + powerName[14].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[14]) + "`" + System.Environment.NewLine +
                    powerIndex[15] + " | " + powerName[15].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[15]) + "`" + System.Environment.NewLine +
                    powerIndex[16] + " | " + powerName[16].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[16]) + "`" + System.Environment.NewLine +
                    powerIndex[17] + " | " + powerName[17].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[17]) + "`" + System.Environment.NewLine +
                    powerIndex[18] + " | " + powerName[18].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[18]) + "`" + System.Environment.NewLine +
                    powerIndex[19] + " | " + powerName[19].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[19]) + "`" + System.Environment.NewLine +
                    powerIndex[20] + " | " + powerName[20].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[20]) + "`" + System.Environment.NewLine +
                    powerIndex[21] + " | " + powerName[21].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[21]) + "`" + System.Environment.NewLine +
                    powerIndex[22] + " | " + powerName[22].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[22]) + "`" + System.Environment.NewLine +
                    powerIndex[23] + " | " + powerName[23].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[23]) + "`" + System.Environment.NewLine +
                    powerIndex[24] + " | " + powerName[24].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[24]) + "`" + System.Environment.NewLine +
                    powerIndex[25] + " | " + powerName[25].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[25]) + "`" + System.Environment.NewLine +
                    powerIndex[26] + " | " + powerName[26].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[26]) + "`" + System.Environment.NewLine +
                    powerIndex[27] + " | " + powerName[27].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[27]) + "`" + System.Environment.NewLine +
                    powerIndex[28] + " | " + powerName[28].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[28]) + "`" + System.Environment.NewLine +
                    powerIndex[29] + " | " + powerName[29].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[29]) + "`" + System.Environment.NewLine,
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
                    Description = "Double check you used the correct guildtag " + guildName + ", it's CaSe-SeNsItIvE.\n If you used the correct guildtag, contact Dasbomber#7777 with details."
                };

                await ctx.Channel.SendMessageAsync(embedError);
            }
        }

        public async Task GatherGuildInfoForComparisionAsync(CommandContext ctx, string guildName, double[] guildPower, int amountOfHtmlCharacters)
        {
            var html = @"http://15650.gzidlerpg.appspot.com/web/scores?tid=220110001&guildTag=" + guildName;
            //Console.WriteLine(html);
            HtmlWeb web = new HtmlWeb();

            LoadGuildInfo(guildName, ctx);

            var htmlDoc = web.Load(html);
            var nodeTitle = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            var nodePowerName = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='example']");

            int count = 0;
            int countIndex = 0;
            int countId = 0;
            int amountOfHtmlCharactersLocal = amountOfHtmlCharacters;

            foreach (HtmlNode row in nodePowerName.SelectNodes("//tbody/tr"))
            {
                for (int i = 0; i < row.InnerLength; i++)
                {
                    amountOfHtmlCharactersLocal++;
                }
                //Console.WriteLine("Amount of guild members " + amountOfHtmlCharacters);

                if (!hasRanCheckForGuildOne)
                {
                    amountOfHtmlCharactersGuildOne = amountOfHtmlCharactersLocal;
                    hasRanCheckForGuildOne = true;
                }
                else if (hasRanCheckForGuildOne)
                {
                    amountOfHtmlCharactersGuildTwo = amountOfHtmlCharactersLocal;
                    hasRanCheckForGuildOne = false;
                }

                if (amountOfHtmlCharactersLocal < 500)
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
                                countIndex++;
                            }
                            else if (countIndex == 1)
                            {
                                countIndex++;
                            }
                            else if (countIndex == 2)
                            {
                                guildPower[countId] = double.Parse(string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", input));
                                //Console.Write(powerPower[countId]);
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
            if (amountOfHtmlCharactersLocal > 500)
            {
                var embedError = new DiscordEmbedBuilder
                {
                    Title = "Something went wrong",
                    Description = "Double check you used the correct guildtag: " + guildName + ", it's CaSe-SeNsItIvE.\n If you used the correct guildtag, contact Dasbomber#7777 with details."
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
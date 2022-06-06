using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GoogleSheetsHelper;
using HtmlAgilityPack;
using QuickChart;
using System;
using System.Collections.Generic;
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

        private double[] powerGuildOnePower = new double[30];
        private double[] powerGuildTwoPower = new double[30];
        private int amountOfHtmlCharactersGuildOne = 0;
        private int amountOfHtmlCharactersGuildTwo = 0;
        private bool hasRanCheckForGuildOne = false;

        #endregion gpcpr variables

        [Command("gpcpr")]
        [Description("Compares two guilds members against eachother and display the power differences in a embed and a chart. Be wary of CaSe-SenSItiVIty.")]
        public async Task CheckGuildsPower(CommandContext ctx,
            [Description("Guildtag of the first guild")] string guildOne,
            [Description("Guildtag of the second guild")] string guildTwo)
        {
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

            string timeSinceMsgSent = $"<t:{unixTime}:R>";

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
                guildPowerDifference = ((guildOneTotalPower / 100) / (guildTwoTotalPower / 100)).ToString("##%", ci);
                theStrongerGuild = guildOne;
                theWeakerGuild = guildTwo;
            }
            else if (guildTwoTotalPower > guildOneTotalPower)
            {
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
                theStrongerGuild + " has " + formattedGuildPowerDifference + " more 🏆 than " + theWeakerGuild + "\n" + timeSinceMsgSent
            };

            await ctx.Channel.SendMessageAsync(embedGuildsComparision);
            amountOfHtmlCharactersGuildOne = 0;
            amountOfHtmlCharactersGuildTwo = 0;

            // TODO: Implement chart functionality. Display a chart showing each member's power from both guilds.

            var gsh = new GoogleSheetsHelper.GoogleSheetsHelper("discorddasbot-969affff0feb.json", "1dn3R45adg6wwxASBXBvKT5ZEylvSfQgBbk7V4IS8Zto");

            #region LongList

            var row1 = new GoogleSheetRow();
            var row2 = new GoogleSheetRow();
            var row3 = new GoogleSheetRow();
            var row4 = new GoogleSheetRow();
            var row5 = new GoogleSheetRow();
            var row6 = new GoogleSheetRow();
            var row7 = new GoogleSheetRow();
            var row8 = new GoogleSheetRow();
            var row9 = new GoogleSheetRow();
            var row10 = new GoogleSheetRow();
            var row11 = new GoogleSheetRow();
            var row12 = new GoogleSheetRow();
            var row13 = new GoogleSheetRow();
            var row14 = new GoogleSheetRow();
            var row15 = new GoogleSheetRow();
            var row16 = new GoogleSheetRow();
            var row17 = new GoogleSheetRow();
            var row18 = new GoogleSheetRow();
            var row19 = new GoogleSheetRow();
            var row20 = new GoogleSheetRow();
            var row21 = new GoogleSheetRow();
            var row22 = new GoogleSheetRow();
            var row23 = new GoogleSheetRow();
            var row24 = new GoogleSheetRow();
            var row25 = new GoogleSheetRow();
            var row26 = new GoogleSheetRow();
            var row27 = new GoogleSheetRow();
            var row28 = new GoogleSheetRow();
            var row29 = new GoogleSheetRow();
            var row30 = new GoogleSheetRow();
            var row31 = new GoogleSheetRow();
            var rowLabels = new GoogleSheetRow();

            var cellRank = new GoogleSheetCell() { CellValue = "Rank" };
            var cellRankIndex1 = new GoogleSheetCell() { CellValue = "1" };
            var cellRankIndex2 = new GoogleSheetCell() { CellValue = "2" };
            var cellRankIndex3 = new GoogleSheetCell() { CellValue = "3" };
            var cellRankIndex4 = new GoogleSheetCell() { CellValue = "4" };
            var cellRankIndex5 = new GoogleSheetCell() { CellValue = "5" };
            var cellRankIndex6 = new GoogleSheetCell() { CellValue = "6" };
            var cellRankIndex7 = new GoogleSheetCell() { CellValue = "7" };
            var cellRankIndex8 = new GoogleSheetCell() { CellValue = "8" };
            var cellRankIndex9 = new GoogleSheetCell() { CellValue = "9" };
            var cellRankIndex10 = new GoogleSheetCell() { CellValue = "10" };
            var cellRankIndex11 = new GoogleSheetCell() { CellValue = "11" };
            var cellRankIndex12 = new GoogleSheetCell() { CellValue = "12" };
            var cellRankIndex13 = new GoogleSheetCell() { CellValue = "13" };
            var cellRankIndex14 = new GoogleSheetCell() { CellValue = "14" };
            var cellRankIndex15 = new GoogleSheetCell() { CellValue = "15" };
            var cellRankIndex16 = new GoogleSheetCell() { CellValue = "16" };
            var cellRankIndex17 = new GoogleSheetCell() { CellValue = "17" };
            var cellRankIndex18 = new GoogleSheetCell() { CellValue = "18" };
            var cellRankIndex19 = new GoogleSheetCell() { CellValue = "19" };
            var cellRankIndex20 = new GoogleSheetCell() { CellValue = "20" };
            var cellRankIndex21 = new GoogleSheetCell() { CellValue = "21" };
            var cellRankIndex22 = new GoogleSheetCell() { CellValue = "22" };
            var cellRankIndex23 = new GoogleSheetCell() { CellValue = "23" };
            var cellRankIndex24 = new GoogleSheetCell() { CellValue = "24" };
            var cellRankIndex25 = new GoogleSheetCell() { CellValue = "25" };
            var cellRankIndex26 = new GoogleSheetCell() { CellValue = "26" };
            var cellRankIndex27 = new GoogleSheetCell() { CellValue = "27" };
            var cellRankIndex28 = new GoogleSheetCell() { CellValue = "28" };
            var cellRankIndex29 = new GoogleSheetCell() { CellValue = "29" };
            var cellRankIndex30 = new GoogleSheetCell() { CellValue = "30" };

            var cell1 = new GoogleSheetCell() { CellValue = guildOne };
            var cell2 = new GoogleSheetCell() { CellValue = guildTwo };

            var cell3 = new GoogleSheetCell() { CellValue = powerGuildOnePower[0].ToString() };
            var cell4 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[0].ToString() };
            var cell5 = new GoogleSheetCell() { CellValue = powerGuildOnePower[1].ToString() };
            var cell6 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[1].ToString() };
            var cell7 = new GoogleSheetCell() { CellValue = powerGuildOnePower[2].ToString() };
            var cell8 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[2].ToString() };
            var cell9 = new GoogleSheetCell() { CellValue = powerGuildOnePower[3].ToString() };
            var cell10 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[3].ToString() };
            var cell11 = new GoogleSheetCell() { CellValue = powerGuildOnePower[4].ToString() };
            var cell12 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[4].ToString() };
            var cell13 = new GoogleSheetCell() { CellValue = powerGuildOnePower[5].ToString() };
            var cell14 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[5].ToString() };
            var cell15 = new GoogleSheetCell() { CellValue = powerGuildOnePower[6].ToString() };
            var cell16 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[6].ToString() };
            var cell17 = new GoogleSheetCell() { CellValue = powerGuildOnePower[7].ToString() };
            var cell18 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[7].ToString() };
            var cell19 = new GoogleSheetCell() { CellValue = powerGuildOnePower[8].ToString() };
            var cell20 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[8].ToString() };
            var cell21 = new GoogleSheetCell() { CellValue = powerGuildOnePower[9].ToString() };
            var cell22 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[9].ToString() };
            var cell23 = new GoogleSheetCell() { CellValue = powerGuildOnePower[10].ToString() };
            var cell24 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[10].ToString() };
            var cell25 = new GoogleSheetCell() { CellValue = powerGuildOnePower[11].ToString() };
            var cell26 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[11].ToString() };
            var cell27 = new GoogleSheetCell() { CellValue = powerGuildOnePower[12].ToString() };
            var cell28 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[12].ToString() };
            var cell29 = new GoogleSheetCell() { CellValue = powerGuildOnePower[13].ToString() };
            var cell30 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[13].ToString() };
            var cell31 = new GoogleSheetCell() { CellValue = powerGuildOnePower[14].ToString() };
            var cell32 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[14].ToString() };
            var cell33 = new GoogleSheetCell() { CellValue = powerGuildOnePower[15].ToString() };
            var cell34 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[15].ToString() };
            var cell35 = new GoogleSheetCell() { CellValue = powerGuildOnePower[16].ToString() };
            var cell36 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[16].ToString() };
            var cell37 = new GoogleSheetCell() { CellValue = powerGuildOnePower[17].ToString() };
            var cell38 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[17].ToString() };
            var cell39 = new GoogleSheetCell() { CellValue = powerGuildOnePower[18].ToString() };
            var cell40 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[18].ToString() };
            var cell41 = new GoogleSheetCell() { CellValue = powerGuildOnePower[19].ToString() };
            var cell42 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[19].ToString() };
            var cell43 = new GoogleSheetCell() { CellValue = powerGuildOnePower[20].ToString() };
            var cell44 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[20].ToString() };
            var cell45 = new GoogleSheetCell() { CellValue = powerGuildOnePower[21].ToString() };
            var cell46 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[21].ToString() };
            var cell47 = new GoogleSheetCell() { CellValue = powerGuildOnePower[22].ToString() };
            var cell48 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[22].ToString() };
            var cell49 = new GoogleSheetCell() { CellValue = powerGuildOnePower[23].ToString() };
            var cell50 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[23].ToString() };
            var cell51 = new GoogleSheetCell() { CellValue = powerGuildOnePower[24].ToString() };
            var cell52 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[24].ToString() };
            var cell53 = new GoogleSheetCell() { CellValue = powerGuildOnePower[25].ToString() };
            var cell54 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[25].ToString() };
            var cell55 = new GoogleSheetCell() { CellValue = powerGuildOnePower[26].ToString() };
            var cell56 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[26].ToString() };
            var cell57 = new GoogleSheetCell() { CellValue = powerGuildOnePower[27].ToString() };
            var cell58 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[27].ToString() };
            var cell59 = new GoogleSheetCell() { CellValue = powerGuildOnePower[28].ToString() };
            var cell60 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[28].ToString() };
            var cell61 = new GoogleSheetCell() { CellValue = powerGuildOnePower[29].ToString() };
            var cell62 = new GoogleSheetCell() { CellValue = powerGuildTwoPower[29].ToString() };

            row1.Cells.AddRange(new List<GoogleSheetCell>() { cellRank, cell1, cell2 });
            row2.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex1, cell3, cell4 });
            row3.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex2, cell5, cell6 });
            row4.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex3, cell7, cell8 });
            row5.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex4, cell9, cell10 });
            row6.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex5, cell11, cell12 });
            row7.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex6, cell13, cell14 });
            row8.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex7, cell15, cell16 });
            row9.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex8, cell17, cell18 });
            row10.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex9, cell19, cell20 });
            row11.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex10, cell21, cell22 });
            row12.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex11, cell23, cell24 });
            row13.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex12, cell25, cell26 });
            row14.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex13, cell27, cell28 });
            row15.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex14, cell29, cell30 });
            row16.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex15, cell31, cell32 });
            row17.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex16, cell33, cell34 });
            row18.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex17, cell35, cell36 });
            row19.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex18, cell37, cell38 });
            row20.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex19, cell39, cell40 });
            row21.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex20, cell41, cell42 });
            row22.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex21, cell43, cell44 });
            row23.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex22, cell45, cell46 });
            row24.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex23, cell47, cell48 });
            row25.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex24, cell49, cell50 });
            row26.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex25, cell51, cell52 });
            row27.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex26, cell53, cell54 });
            row28.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex27, cell55, cell56 });
            row29.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex28, cell57, cell58 });
            row30.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex29, cell59, cell60 });
            row31.Cells.AddRange(new List<GoogleSheetCell>() { cellRankIndex30, cell61, cell62 });

            var rows = new List<GoogleSheetRow>() { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13, row14, row15, row16, row17, row18, row19, row20, row21, row22, row23, row24, row25, row26, row27, row28, row29, row30, row31 };

            gsh.AddCells(new GoogleSheetParameters() { SheetName = "Sheet44", RangeColumnStart = 1, RangeRowStart = 1 }, rows);

            #endregion LongList

            powerGuildOnePower = new double[30];
            powerGuildTwoPower = new double[30];

            var config = @"{
                  type: 'bar',
                  options: {
                    plugins: {
                      googleSheets: {
                        sheetUrl: 'https://docs.google.com/spreadsheets/d/1dn3R45adg6wwxASBXBvKT5ZEylvSfQgBbk7V4IS8Zto/edit?usp=sharing',
                        labelColumn: 'Rank',
                        dataColumns: ['placeHolder1', 'placeHolder2'],
                      },
                    },
                    legend: {
                      display: false,
                    },
                    title: {
                      align: 'end',
                      display: true,
                      position: 'top',
                      text: 'placeHolder3 (blue) versus placeHolder4 (orange)',
                    },
                  },
                }";

            var result1 = config.Replace("placeHolder1", guildOne);
            var result2 = result1.Replace("placeHolder2", guildTwo);

            var result3 = result2.Replace("placeHolder3", guildOne);
            var result4 = result3.Replace("placeHolder4", guildTwo);
            Chart qc = new Chart();

            qc.Width = 750;
            qc.Height = 500;
            qc.Config = result4;

            await ctx.Channel.SendMessageAsync(qc.GetShortUrl());
        }

        [Command("gp")]
        [Description("Gathers the list and powers of one guild. Be wary of CasE-SENsiTiVity.")]
        public async Task CheckGuildPower(CommandContext ctx, [Description("Guildtag of the guild")] string guildName)
        {
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

            string timeSinceMsgSent = "<t:" + unixTime + ":R>";

            for (int i = 0; i < powerIndex.Length; i++)
            {
                powerIndex[i] = 8008;
                powerName[i] = "N/A";
                powerPower[i] = 8008;
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
                    "`" + powerIndex[0] + "  | " + powerName[0].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[0]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[1] + "  | " + powerName[1].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[1]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[2] + "  | " + powerName[2].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[2]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[3] + "  | " + powerName[3].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[3]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[4] + "  | " + powerName[4].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[4]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[5] + "  | " + powerName[5].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[5]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[6] + "  | " + powerName[6].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[6]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[7] + "  | " + powerName[7].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[7]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[8] + "  | " + powerName[8].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[8]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[9] + "  | " + powerName[9].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[9]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[10] + " | " + powerName[10].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[10]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[11] + " | " + powerName[11].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[11]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[12] + " | " + powerName[12].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[12]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[13] + " | " + powerName[13].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[13]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[14] + " | " + powerName[14].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[14]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[15] + " | " + powerName[15].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[15]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[16] + " | " + powerName[16].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[16]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[17] + " | " + powerName[17].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[17]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[18] + " | " + powerName[18].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[18]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[19] + " | " + powerName[19].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[19]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[20] + " | " + powerName[20].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[20]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[21] + " | " + powerName[21].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[21]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[22] + " | " + powerName[22].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[22]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[23] + " | " + powerName[23].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[23]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[24] + " | " + powerName[24].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[24]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[25] + " | " + powerName[25].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[25]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[26] + " | " + powerName[26].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[26]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[27] + " | " + powerName[27].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[27]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[28] + " | " + powerName[28].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[28]) + "`" + System.Environment.NewLine +
                    "`" + powerIndex[29] + " | " + powerName[29].PadRight(20, pad) + " | Power: " + string.Format(System.Globalization.CultureInfo.GetCultureInfo("EN-US"), "{0:0,0}", powerPower[29]) + "`" + System.Environment.NewLine +
                    "\n" + timeSinceMsgSent
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
                    Description = $"1. Double check you used the correct guildtag: {guildName}, it's CaSe-SeNsItIvE.\n2. " +
                    $"Check that the website is online and actually displaying values. " +
                    $"Sometimes it doesn't work and displays -1 power for each member.\n" +
                    $"You can check it here http://15650.gzidlerpg.appspot.com/web/scores?tid=228310001 , " +
                    $"and enter the 3-character Guild Tag (case sensitive).This error might only occur after GW has ended and should only last for approx 1hr." +
                    $"\n\nIf you've checked the above, contact Dasbomber#7777 with details."
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
                    Description = $"1. Double check you used the correct guildtag: {guildName}, it's CaSe-SeNsItIvE.\n" +
                    $"2. Check that the website is online and actually displaying values. " +
                    $"Sometimes it doesn't work and displays -1 power for each member.\n" +
                    $"You can check it here http://15650.gzidlerpg.appspot.com/web/scores?tid=228310001 , and enter the 3-character Guild Tag (case sensitive). " +
                    $"This error might only occur after GW has ended and should only last for approx 1hr.\n\n" +
                    $"If you've checked the above, contact Dasbomber#7777 with details."
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
# Dasbot

Dasbot is an discord bot made with C# and DSharp. Intended to be a fun project made by Peter Jörgensen in which he explores the world of making your very own bot.

The bot's prefix is **%** which is how you communicate with the bot. %help to see all available commands.

## Features

<bold>Webscraper</bold>

Webscraper commands that pulls game data from http://15650.gzidlerpg.appspot.com/web/scores?tid=220110001&guildTag=TRB and displays it inside an embed on discord.

We use this in our server too compare different guilds within the game "Crush Them All".

This is relevant since there are weekly Guild-Wars where two guilds face off versus eachother and seeing how the enemy guild fare off versus your own helps with assessing the war situation.

<hr>
<h3> <%gpcpr TRB EXO> Command Demonstration</h3>
 
```java
  
%gpcpr - the prefix command to start comparing two guilds with eachother. It accepts 2 parameters.
TRB - A 3-letter guildtag that you want too use in the comparision
EXO - A 3-letter guildtag that you want too use in the comparision
  
```

![Screenshoot showing the initial comparision of the two guilds member's powers](https://i.imgur.com/iTNvRn8.png)

Pulls the relevant data from the 2 guildtags the user entered. It then puts that info into a nicely made embed-message that will be displayed in discord.

![Screenshoot showing a chart based off the data pulled earlier](https://i.imgur.com/EhmlLjV.png)

The data that was pulled earlier will now be sent to a remote Google Sheets file which is needed for the API "QuickChart". QuickChart is able to make nice looking charts based on data in sheets/excel files. Through some configuration we then generate a QuickChart link that the bot outputs, which results in the chart above shown in the imgur link.
  
  <hr>

## Applications/Software used
<ul>
  <li>Git</li>
  <li>Github</li>
  <li>Visual Studio</li>
  <li>Discord Developer Portal</li>
  <li>Debian</li>
</ul>
  
[SomethingHost](https://something.host/en/) (Where I host the bot 24/7 and use their remote Linux terminal to operate the bot)
  
[QuickChart](https://quickchart.io/) (API to automatically generate charts based off data in a Sheets/Excel file)
  
[Google's API for C# Development](https://developers.google.com/docs/api/quickstart/dotnet) (Used here to automatically insert data into a Sheets file)

[DSharp](https://github.com/DSharpPlus/DSharpPlus) (The Discord API used for Dasbot)

  
Last updated: 2022-05-12


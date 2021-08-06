# MonkeyButler

This is a Discord bot working with Final Fantasy XIV data. Built for the Twilight Knights Free Company.

## Current commands

| Command | Example | Description |
| - | - | - |
| Search | !search Jolinar Cast Diabolos | Searches the Lodestone for characters and returns top five results. |
| Verify | !verify Jolinar Cast | Verifies the user as a member of the server's Free Company, according to Lodestone. |

## Built with

* [Discord.NET](https://docs.stillu.cc/index.html)
* [.NET 5](https://dotnet.microsoft.com/)
* [xUnit](https://xunit.net/)
* [Juval Lowe's IDesign](http://www.idesign.net/)
* [xivapi.com](http://xivapi.com)
* [Redis](https://redis.io/)

## Repository structure

* **MonkeyButler** - Main client app handling connection to Discord and providing ASP.NET Core endpoints.
* **MonkeyButler.Business** - Component for handling business logic and flow for each request & command.
* **MonkeyButler.Data.Api** - Component for integrating with APIs, such as XivApi.com.
* **MonkeyButler.Data.Storage** - Component for handling persistent storage, currently with Redis.

## How to get started

To compile and run this code yourself, you can either load `MonkeyButler.sln` in Visual Studio or use the [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/).

This code is expecting two secret configuration values that is not included in this repository. It is highly encouraged to use user secrets (or environment variables in production) or add these values to `appsettings.json` in /src/MonkeyButler.

```json
{
  "Discord": {
    "Token": "discord-bot-token"
  },
  "XivApi": {
    "Key": "xivapi-private-key"
  }
}
```

The discord bot token can be obtained by creating your own bot application in the [Discord Developer Portal](https://discord.com/developers/applications) and adding the bot to your Discord server.

The xivapi.com key can be obtained from the [xivapi.com account page](https://xivapi.com/account).

With these configuration values loaded and database configured, either run `MonkeyButler` in Visual Studio or execute the following dotnet command in the `src\MonkeyButler` directory:

```cmd
...src\MonkeyButler> dotnet run
```

## Adding this bot to your server

This bot currently requires the following permissions for full functionality:
* Send Messages
* Embed Links
* Add Reactions
* Manage Roles (For automatically adding roles to verified members)
* Manage Nicknames (For automatically changing nicknames to in-game names)

These permissions are compiled into this URL for your use, just replace the `CLIENT_ID` with the client id of your bot.

```
https://discord.com/api/oauth2/authorize?client_id=CLIENT_ID&permissions=402671680&scope=bot
```

## Sample app execution

Running the application will automatically create a socket connection with Discord and start accepting commands. In Discord in the same channel as the Monkey Butler bot, execute commands with the command prefix, by default '`!`'.

```
User
> !search Jolinar Cast Diabolos

Monkey Butler
| Jolinar Cast
|
| Diabolos (Crystal)
| 
| Hyur Highlander
| Lv80 Gladiator / Paladin
| <Twilight Knights>
```

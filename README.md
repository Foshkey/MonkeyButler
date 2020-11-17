# MonkeyButler

This is a Discord bot working with Final Fantasy XIV data. Built for the Twilight Knights Free Company.

## Current commands

| Command | Example | Description |
| - | - | - |
| Search | !search Jolinar Cast Diabolos | Searches the Lodestone for characters and returns top five results. |
| Verify | !verify Jolinar Cast | Verifies the user as a member of the server's Free Company, according to Lodestone. |

## Built with

* [Discord.NET](https://docs.stillu.cc/index.html)
* [ASP.NET Core 3.1](https://dotnet.microsoft.com/)
* [xUnit](https://xunit.net/)
* [Juval Lowe's IDesign](http://www.idesign.net/)
* [xivapi.com](http://xivapi.com)

## Repository structure

* **MonkeyButler** - Main client app handling connection to Discord and providing ASP.NET Core endpoints.
* **MonkeyButler.Business** - Component for handling business logic and flow for each request & command.
* **MonkeyButler.Data** - Component for integrating with any external dependencies, such as xivapi.com.

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
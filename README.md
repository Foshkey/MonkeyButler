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

This bot requires two external keys that you must obtain yourself if you want to run or develop with this bot:

- Discord Bot Token
  - Create your own bot application in the [Discord Developer Portal](https://discord.com/developers/applications).
- XIVAPI.com key
  - Sign in and obtain the key from the [xivapi.com account page](https://xivapi.com/account).

### Running with Docker

Monkey Butler has its own image [on Docker Hub](https://hub.docker.com/repository/docker/foshkey/monkey-butler) kept in sync with the master branch of this repository. It is recommended to use [docker-compose](https://docs.docker.com/compose/) to compose up both Monkey Butler and Redis containers together.

The docker-compose.yml on the root of this repository is for development purposes. For a production environment, it is recommended to use the following example docker-compose.yml:

```yml
version: '3.9'

services:
  redis:
    image: redis
    restart: unless-stopped
    volumes:
      - redis_prod:/data
    command: redis-server --appendonly yes
  monkey-butler:
    image: foshkey/monkey-butler:master
    restart: unless-stopped
    ports:
      - "25565:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - Redis__ConnectionString=redis
      - Discord__Token=YOUR DISCORD TOKEN
      - XivApi__Key=YOUR XIVAPI KEY
volumes:
  redis_prod:
```

Note: replace `YOUR DISCORD TOKEN` and `YOUR XIVAPI KEY`. Once that is done, run `docker-compose up --detach` in the same directory as this yml file to start both of these containers. This will effectively persist redis data to the `redis_prod` volume and restart the containers if your machine ever restarts.

### Development

To compile and run this code yourself, you can either load `MonkeyButler.sln` in Visual Studio or use the [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/).

This code is expecting two secret configuration values (Outlined above) that is not included in this repository. It is highly encouraged to use user secrets (or environment variables in production) or add these values to `appsettings.json` in /src/MonkeyButler.

```json
{
  "Discord": {
    "Token": "YOUR DISCORD TOKEN"
  },
  "XivApi": {
    "Key": "YOUR XIVAPI KEY"
  }
}
```

Note: replace `YOUR DISCORD TOKEN` and `YOUR XIVAPI KEY`.

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

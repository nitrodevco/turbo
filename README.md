# Turbo Emulator
[![image](https://img.shields.io/badge/VERSION-0.0.1-success.svg?style=for-the-badge)](#)
[![image](https://img.shields.io/badge/STATUS-UNSTABLE-red.svg?style=for-the-badge)](#)
[![image](https://img.shields.io/discord/557240155040251905?label=Discord&style=for-the-badge)](https://discord.gg/BzfFsTp)

## What is Turbo?
Turbo is a brand new Habbo Emulator by Krews written in .NET 5!

We created Turbo because we felt other emulators were outdated. The current emulators are based on years old code and are far behind the current coding standards. We see that they are difficult to maintain and things quickly break down.

Turbo focuses on the SOLID principles. This increases maintainability, flexibility and testability, which is very important.

## Core Features
- Multi-revision support (we provide a default revision plugin for **PRODUCTION-201611291003-338511768**)
- Multiple database support (default: MySQL/MariaDB)
- Code first database migrations
- Plugin system
- REST API
- Logging (log to anywhere you want using [Serilog Sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks))

## Build Using
- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0 ".NET 5 SDK")
- [Microsoft Abstractions Dependency Injection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection?view=dotnet-plat-ext-5.0)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [XUnit](https://xunit.net/), [Moq](https://github.com/moq/moq4) and [Autofixture](https://github.com/AutoFixture/AutoFixture) for unit testing
- [DotNetty](https://github.com/Azure/DotNetty) for networking

## License
Turbo is released under the [GNU General Public License v3](https://www.gnu.org/licenses/gpl-3.0.txt).

## Download

Compiled Download: https://git.krews.org/turbo/turbo-emulator/-/releases

## Branches
There will be 2 branches of the Arcturus Morningstar emulator:

`master` - The master branch will be the stable branch. Everything here has been tested on a live hotel and contains no known problems.

`dev` - The dev branch will be the unstable branch. This one is the most up to date, but things may not work as intended.

There is no set timeframe on when new versions will be released or when the stable branch will be updated.

## Plugins
### Why always make things as plugins?
1. Other people will see that plugins are the normal way of adding custom features
2. Plugins can be added and removed at the hotel owner's choice, it makes customizing the hotel easier
3. Developers will be able to read plugin source code to learn how to make their own plugins, without the need to look in complicated source code

### Official plugins ##
You can find official plugins (including the default revision plugin) at the following URL: 

[View the respository here.](https://git.krews.org/turbo/official-plugins)

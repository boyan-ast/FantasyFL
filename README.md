# FantasyFL

## :pencil: Project Introduction ##
 
**FantasyFL** (Fantasy First League) is my project for the **SoftUni** course [**ASP.NET Core**](https://softuni.bg/trainings/3593/csharp-web-basics-basics-january-2022).

## :soccer: Project Overview ##

FantasyFL (Fantasy First League) is a game that puts you in a role of Fantasy manager of Bulgarian First Football League players. You create your own Fantasy Team and pick a squad of 15 players from the season 2021/2022. They score points for your team according to their performance in the matches. Points are awarded to players based on different match events. Your team's points will be scored by your Starting XI for the match round - "Gameweek".

The rules of the game are described in detail on the _**Rules**_ page.

## :floppy_disk: Data ##

The data related to First Bulgarian League is provided by [API-Football](https://www.api-football.com/).

## :flashlight: How Project Works ##

**Guests**
* Can register and log in using a local account
* Can access the Welcome and Rules pages as well as the pages related to Bulgarian First Leagues (_Results_, _Fixtures_, _Teams_, _Players_) 

**Users**
* Pick their initial squad of Bulgarian First League players
* Choose starting lineup for the upcomming Gameweek
* Can make one transfer before the start date of each Gameweek
* Can compete with all registered participants in the game in the global league
* Can create and join leagues to compete with friends and other users
* Have access to all the data related to Bulgarian First League (_Results_, _Fixtures_, _Teams_, _Players_)
* Can access the Gameweek Statistics of all players part of their Fantasy Team

**Admin**
* Is responsible for importing the data after the end of each week Gameweek and marking it as finished
* Can import data directly from [API-Football](https://www.api-football.com/)
* Can import data from Azure Blob Storage and local files

**Running the project for the firs time**
* The default provider of [API-Football](https://www.api-football.com/) data in the project is Azure Blob Storage
* The data provider can be modified by changing the registered service for external data
* All data needed for the start of the Bulgarian First League season 2021/2022 will be seeded (_Teams_, _Players_, _Gameweeks_, _Fixtures_, _Stadiums_)
* For the purpose of demonstrating the functionallity, it will be simulated that the upcomming Gameweek is number 20 (the first in 2022)
* In order to test the admin functionality of the project, the the following credentials are provided - Username: Admin, Password: admin123

## :pencil: Unit Tests Code Coverage ##

![unitTestCodeCoverage](https://user-images.githubusercontent.com/62556633/162568840-df70387d-cbe0-4140-ac89-18d1b86fa529.png)

## :hammer: Built With ##

* ASP.NET Core 6.0
* EF Core 6.0
* Newtonsoft.Json
* AutoMapper
* AutoFixture
* MockQueryable
* Moq
* xUnit
* Selenium
* Azure App Service
* Azure Blob Storage
* Azure Identity
* Azure Key Vault
* Bootstrap
* JavaScript
* jQuery
* Sweet Alert
* StyleCop Analyzers

## :link: Link ##

https://fantasyfl.azurewebsites.net/

## :floppy_disk: Database Diagram ##

![database-diagram](https://user-images.githubusercontent.com/62556633/162214252-db4c9924-594f-437a-a3fe-b12c9cf7e0c1.png)

## Template Authors ##

- [Nikolay Kostov](https://github.com/NikolayIT)
- [Vladislav Karamfilov](https://github.com/vladislav-karamfilov)
- [Stoyan Shopov](https://github.com/StoyanShopov)

## License ##

This project is licensed under the [MIT License](LICENSE).

**The project is only for educational purposes.**

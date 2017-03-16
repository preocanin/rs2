# rs2
This is a facultcy project made for learning purposes. The project represents an example of usage ASP.NET Core with PostgreSQL database, custom authentication using JWT and Angular.

## How to build & run locally

#### Prerequisites
1. Install Node.js (v6.9 is recommended, but 4.6 should be sufficient).
2. Install bower with `npm install -g bower`
3. Install PostgreSQL server to your machine than create database and a user of your choice (you can use db_setup.py from tools folder for that)
4. Change `database_url` string in `rs2/appsettings.json`
5. Install .NET Core 1.1

#### Building
1. Download the repo to you machine and navigate with the comand line to it.
2. cd to rs2 folder and execute `bower install` -- to install dependencies for client
3. Execute `dotnet restore` -- to resotre NuGet packages

#### Run 
1. In the rs2 folder execute `dotnet run`


License
----

MIT
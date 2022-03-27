
# My Restaurants

 Backend API for to search for a restaurant,search for restauraent to check if available on any date and time, scrap data from a restaurants details data website and insert it to DB.

## Installation

Use the [package manager console](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell) to install My Restaurants api. 

### Alternatively you can also clone the [Repository](https://github.com/sinantok/aspnetcore-webapi-template).

1. Clone this Repository and Extract it to a Folder.
2. Change the Connection Strings for the "DefaultConnection" and "IdentityConnection" in the appsettings.json
3. Change the Connection Strings for the "RedisConnectionString" with your Redis server connection string
4. Run the following commands on Visual Studio Package Manager Console in the Projecct's Directory.
```bash
dotnet restore
```
```bash
update-database -c IdentityContext
```
```bash
update-database -c ApplicationDbContext
```
Run the Solution using Visual Studio

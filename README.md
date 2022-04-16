
# Find my favourite Restaurants

A searching system Backend API for a restaurants, Following are the features,
1. Search a Restaurent by the Restaurant Name
2. Search for restauraent to check if available on any specific date and time,
3. Scrap data from a restaurants details data website and insert it to DB.[From Here](https://gist.githubusercontent.com/seahyc/7ee4da8a3fb75a13739bdf5549172b1f/raw/f1c3084250b1cb263198e433ae36ba8d7a0d9ea9/hours.csv)

## Installation

Use the [package manager console](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell) to install My Restaurants api. 

### Alternatively you can also clone the [Repository](https://github.com/sarkeranik/full-stack-18Oct2021-backend).

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

### Swagger
You can view endpoints with swagger
![image](https://user-images.githubusercontent.com/65606710/160292141-97e53c94-e598-4a7e-8830-571f98ff9b31.png)
### HealthCheck
You can check the status of the services with HealthCheck

### Default Roles & Credentials
As soon you build and run your application, default users and roles get added to the database.

Default Roles:
- SuperAdmin
- Admin
- Moderator
- Basic

Here are the credentials for the default user.
- Email - superadmin@gmail.com  / Password - 123Pa$$word!

You can use these default credentials to generate valid JWTokens at the ../api/account/authenticate endpoint.

## Restaurants Features
1. Get all Restaurants Names
2. Check If the restaurant is open by Restaurants Name and Date-Time,
3. Add Restaurant to the user's Favorite Collection.

## Technologies
- ASP.NET Core 5 WebApi
- .NET Core 3.1
- REST Standards
- GraphQL
- MSSQL
- Microsoft Identity
- Redis
- SeriLog(seq)
- AutoMapper
- Smtp / Mailkit
- Swagger Open Api
- Health Checks

## Features
- [x] Net Core
- [x] N-Tier Architecture
- [x] Restful
- [x] GraphQl
- [x] Entity Framework Core - Code First
- [x] Repository Pattern - Generic
- [x] UnitOfWork
- [x] Redis Caching
- [x] Response Wrappers
- [x] Microsoft Identity with JWT Authentication
- [x] Role based Authorization
- [x] Identity Seeding
- [x] Database Seeding
- [x] Custom Exception Handling Middlewares
- [x] Serilog
- [x] Automapper
- [x] Swagger UI
- [x] Healthchecks
- [x] SMTP / Mailkit / Sendgrid Email Service
- [x] Complete User Management Module (Register / Generate Token / Forgot Password / Confirmation Mail)
- [x] User Auditing

## Prerequisites
- Visual Studio 2019 Community and above
- .NET Core 5 SDK and above
- Basic Understanding of Architectures and Clean Code Principles



# interview-project

For this project I used C# ASP.NET Core (Model-View-Controller) with database logic (SQL Server)

.NET Version 8.0

This project uses Entity Framework Core with a code-first approach to manage the database schema. 
Instead of manually writing SQL scripts, the database structure is defined through C# model classes and automatically applied via EF Core migrations.

1. Clone the repository to your local machine.
2. Make sure you have the following tools installed:

   - .NET SDK (for compiling and running the application).
   - Visual Studio for editing and running the project.
   - SQL Server or another database server.

3. Once you've cloned the project, open a command prompt or terminal, navigate to the project directory, and restore the NuGet packages with this command: dotnet restore.
4. For local development you should have in the appsettings.json file the following connection string:

   "ConnectionStrings": {
   "DefaultConnection": "Server=(localdb)\\MSSqlLocalDb;Database=InterviewDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }

5. Now, build the project to make sure everything is working with command: dotnet build.
6. Run the following command to apply the migrations: dotnet ef database update.
7. Once everything is set up, you can start the application using the following command: dotnet run.
8. You can now navigate to the port specified in your web browser.


The SQL scripts to create the database with database-first approach: 

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AppUsers] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AppUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Companies] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [StockTicker] nvarchar(max) NOT NULL,
    [Exchange] nvarchar(max) NOT NULL,
    [ISIN] nvarchar(max) NOT NULL,
    [WebsiteURL] nvarchar(max) NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250406104756_InitialCreate', N'9.0.3');

COMMIT;
GO
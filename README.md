# interview-project

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

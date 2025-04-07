using interview_project.Database;
using Microsoft.EntityFrameworkCore;

namespace interview_project.Tests
{
    public static class GetDbContextAndSeed
    {
        public static AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            DbSeeder.SeedData(context);

            return context;
        }
    }
}
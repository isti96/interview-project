using AutoMapper;
using interview_project.Controllers;
using interview_project.Models;
using interview_project.Services;

namespace interview_project.Tests
{
    public static class TestSetupHelper
    {
        public static (HomeController, CompanyService) GetHomeControllerWithDbContext()
        {
            var context = GetDbContextAndSeed.GetInMemoryDbContext();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();

            var companyService = new CompanyService(context);

            var controller = new HomeController(context, mapper, companyService);

            return (controller, companyService);
        }
    }
}
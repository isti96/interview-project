using interview_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace interview_project.Tests.Tests
{
    public class CreateCompanyTests
    {
        [Fact]
        public async Task CreateCompany_ReturnsCreatedCompanyAndRedirect_WhenISINIsCorrectAndUnique()
        {
            var (controller, companyService) = TestSetupHelper.GetHomeControllerWithDbContext();

            CompanyModel companyModel = new() { Id = 76, Name = "Silcotub", StockTicker = "SCT", Exchange = "London", ISIN = "GR343949394" };

            var result = await controller.Create(companyModel, CancellationToken.None);

            var addedCompany = await companyService.GetCompanyById(76, CancellationToken.None);

            Assert.NotNull(addedCompany);
            Assert.Equal("Silcotub", addedCompany.Name);
            Assert.Equal(76, addedCompany.Id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task CreateCompany_SetsErrorMessageAndRedirects_WhenISINIsCorrectAndNotUnique()
        {
            var (controller, _) = TestSetupHelper.GetHomeControllerWithDbContext();

            CompanyModel companyModel = new() { Id = 76, Name = "Silcotub", StockTicker = "SCT", Exchange = "London", ISIN = "DE000PAH0038" };

            var result = await controller.Create(companyModel, CancellationToken.None);

            Assert.Equal("A company already exists with this ISIN number!", controller.ViewData["ErrorMessage"]);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateCompany", viewResult.ViewName);
        }

        [Fact]
        public async Task CreateCompany_SetsErrorMessageAndRedirects_WhenISINIsNotCorrectButUnique()
        {
            var (controller, _) = TestSetupHelper.GetHomeControllerWithDbContext();

            CompanyModel companyModel = new() { Id = 76, Name = "Silcotub", StockTicker = "SCT", Exchange = "London", ISIN = "D3000PAH0038" };

            var result = await controller.Create(companyModel, CancellationToken.None);

            Assert.Equal("ISIN must start with two letters.", controller.ViewData["ErrorMessage"]);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateCompany", viewResult.ViewName);
        }
    }
}
using interview_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace interview_project.Tests.Tests
{
    public class UpdateCompanyTests
    {
        [Fact]
        public async Task UpdateCompany_ReturnsUpdatedCompanyAndRedirect_WhenISINIsCorrectAndUnique()
        {
            var (controller, companyService) = TestSetupHelper.GetHomeControllerWithDbContext();

            CompanyModel companyModel = new() { Id = 3, Name = "Silcotub", StockTicker = "SCT", Exchange = "London", ISIN = "GR343949394" };

            var result = await controller.Update(companyModel.Id, companyModel, CancellationToken.None);

            var updatedCompany = await companyService.GetCompanyById(3, CancellationToken.None);

            Assert.NotNull(updatedCompany);
            Assert.Equal("Silcotub", updatedCompany.Name);
            Assert.Equal(3, updatedCompany.Id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task UpdateCompany_SetsErrorMessageAndRedirects_WhenISINIsCorrectAndNotUnique()
        {
            var (controller, _) = TestSetupHelper.GetHomeControllerWithDbContext();

            CompanyModel companyModel = new() { Id = 4, Name = "Silcotub", StockTicker = "SCT", Exchange = "London", ISIN = "US1104193065" };

            var result = await controller.Update(companyModel.Id, companyModel, CancellationToken.None);

            Assert.Equal("A company already exists with this ISIN number!", controller.ViewData["ErrorMessage"]);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Details", viewResult.ViewName);
        }

        [Fact]
        public async Task UpdateCompany_SetsErrorMessageAndRedirects_WhenISINIsNotCorrectButUnique()
        {
            var (controller, _) = TestSetupHelper.GetHomeControllerWithDbContext();

            CompanyModel companyModel = new() { Id = 1, Name = "Silcotub", StockTicker = "SCT", Exchange = "London", ISIN = "D3000PAH0038" };

            var result = await controller.Update(companyModel.Id, companyModel, CancellationToken.None);

            Assert.Equal("ISIN must start with two letters.", controller.ViewData["ErrorMessage"]);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Details", viewResult.ViewName);
        }
    }
}
namespace interview_project.Tests.Tests
{
    public class GetAllCompaniesTests
    {
        [Fact]
        public async Task GetAllCompaniesAsync_ReturnsAllCompanies()
        {
            var (controller, companyService) = TestSetupHelper.GetHomeControllerWithDbContext();

            var result = await companyService.GetAllCompanies(null, null, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Contains(result, c => c.Name == "Heineken NV");
        }
    }
}
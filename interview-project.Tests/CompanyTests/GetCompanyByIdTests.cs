namespace interview_project.Tests.Tests
{
    public class GetCompanyByIdTests
    {
        [Fact]
        public async Task GetCompanyByIdAsync_ReturnsCompany_WhenCompanyExists()
        {
            var (_, companyService) = TestSetupHelper.GetHomeControllerWithDbContext();

            var result = await companyService.GetCompanyById(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Apple Inc.", result.Name);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ReturnsNull_WhenCompanyDoesNotExist()
        {
            var (_, companyService) = TestSetupHelper.GetHomeControllerWithDbContext();

            var result = await companyService.GetCompanyById(94738, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
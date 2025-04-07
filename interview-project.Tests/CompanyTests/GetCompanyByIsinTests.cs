namespace interview_project.Tests
{
    public class GetCompanyByIsinTests
    {
        [Fact]
        public async Task GetCompanyByIsinAsync_ReturnsCompany_WhenCompanyExists()
        {
            var (_, companyService) = TestSetupHelper.GetHomeControllerWithDbContext();

            var result = await companyService.GetCompanyByISIN("DE000PAH0038", CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("DE000PAH0038", result.ISIN);
        }

        [Fact]
        public async Task GetCompanyByIsinAsync_ReturnsNull_WhenCompanyDoesNotExist()
        {
            var (_, companyService) = TestSetupHelper.GetHomeControllerWithDbContext();

            var result = await companyService.GetCompanyByISIN("DE000PDSH0038", CancellationToken.None);

            Assert.Null(result);
        }
    }
}
using interview_project.Database.Entities;

namespace interview_project.Database
{
    public static class DbSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(
                    new Company { Name = "Apple Inc.", Exchange = "NASDAQ", StockTicker = "AAPL", ISIN = "US0378331005", WebsiteURL = "http://www.apple.com" },
                    new Company { Name = "British Airways Plc", Exchange = "Pink Sheets", StockTicker = "BAIRY", ISIN = "US1104193065", WebsiteURL = "" },
                    new Company { Name = "Heineken NV", Exchange = "Euronext Amsterdam", StockTicker = "HEIA", ISIN = "NL0000009165", WebsiteURL = "" },
                    new Company { Name = "Panasonic Corp", Exchange = "Tokyo Stock Exchange", StockTicker = "6752", ISIN = "JP3866800000", WebsiteURL = "http://www.panasonic.co.jp" },
                    new Company { Name = "Porsche Automobil", Exchange = "Deutsche Börse", StockTicker = "PAH3", ISIN = "DE000PAH0038", WebsiteURL = "https://www.porsche.com/" }
                );

                context.SaveChanges();
            }
        }
    }
}
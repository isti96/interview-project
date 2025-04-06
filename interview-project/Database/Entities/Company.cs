namespace interview_project.Database.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StockTicker { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string ISIN { get; set; } = string.Empty;
        public string? WebsiteURL { get; set; }
    }
}
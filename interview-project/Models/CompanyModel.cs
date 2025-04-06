using System.ComponentModel.DataAnnotations;

namespace interview_project.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string StockTicker { get; set; } = string.Empty;
        [Required]
        public string Exchange { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[A-Za-z]{2}.*$", ErrorMessage = "The first two characters of the ISIN must be letters / non numeric.")]
        public string ISIN { get; set; } = string.Empty;
        public string? WebsiteURL { get; set; }
    }
}

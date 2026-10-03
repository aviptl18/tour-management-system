namespace Tour_Module.DTOs.Option
{
    public class OptionDto
    {
        public int OptionId { get; set; }

        public string OptionName { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public string Currency { get; set; } = "INR";

        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
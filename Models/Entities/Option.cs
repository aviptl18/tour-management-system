namespace Tour_Module.Models.Entities
{
    public class Option
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
    }

}

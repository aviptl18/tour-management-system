using System.ComponentModel.DataAnnotations;

namespace Tour_Module.Models.Entities
{
    public class LocationImage
    {
        [Key]
        public int ImageId { get; set; }
        public int LocationId { get; set; }
        public string ImageUrl { get; set; }

    }
}

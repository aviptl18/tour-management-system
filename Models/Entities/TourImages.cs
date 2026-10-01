using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Module.Models.Entities
{
    public class TourImages
    {
        public int TourImageId { get; set; }
        public int TourId { get; set; }
        public int ImageId { get; set; }
        public bool IsCoverImage { get; set; }

        // Navigation

        public Tour? Tour { get; set; }

        [ForeignKey(nameof(ImageId))]
        public LocationImage? LocationImage { get; set; }
    }
}

using System;

namespace Tour_Module.Models.Entities
{
    public class TourAllowedOptions
    {
        // =========================
        // COMPOSITE PRIMARY KEY
        // =========================

        public int TourId { get; set; }
        public int OptionId { get; set; }

        // =========================
        // EXTRA COLUMNS
        // =========================

        public bool IsDefault { get; set; }
        public DateTime CreatedDate { get; set; }

        // =========================
        // (OPTIONAL) NAVIGATION PROPERTIES
        // =========================
        // You can add these later if needed
        // public Tour Tour { get; set; }
        // public OptionDefinition OptionDefinition { get; set; }
    }
}

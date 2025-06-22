namespace GYMNETIC.Core.Models
{
    public class GCCategory
    {
        public required string Id { get; set; } // Unique identifier for the category
        public required string Name { get; set; } // Name of the exercise category, e.g., "Strength", "Cardio"
        public string? Slug { get; set; } // Optional description of the category
        public bool IsActive { get; set; } = true; // Indicates if the category is active or not
    }
}

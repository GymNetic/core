namespace GYMNETIC.Core.Models
{
    public class ExerciseCategory
    {
        public required string Id { get; set; } // Unique identifier for the category
        public required string Name { get; set; } // Name of the exercise category, e.g., "Strength", "Cardio"
        public string? Slug { get; set; } // Optional description of the category
        public string? ImageUrl { get; set; } // Optional URL for an image representing the category
        public bool IsActive { get; set; } = true; // Indicates if the category is active or not
    }
}

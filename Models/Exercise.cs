namespace GYMNETIC.Core.Models
{
    public class Exercise
    {
        public required int Id { get; set; }
        public required string Slug { get; set; } // Unique identifier for the exercise, typically a URL-friendly string
        public required string Link { get; set; } 
        public required string Type { get; set; } // e.g., "Strength", "Cardio", "Flexibility"
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public required string ExerciseCategoryId { get; set; } // e.g., "Strength", "Cardio", etc.
        public required ExerciseCategory ExerciseCategory { get; set; } // Navigation property to the exercise category
        public int DurationInSeconds { get; set; } = 0; // Duration of the exercise in seconds
        public string? Equipment { get; set; } // Equipment required for the exercise, if any
        public string? TargetMuscleGroup { get; set; } // Primary muscle group targeted by the exercise
        public int TrainingPlanId { get; set; }
        public TrainingPlan TrainingPlan { get; set; } = null!;

    }
}

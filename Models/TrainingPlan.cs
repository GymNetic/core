using GYMNETIC.Core.Models;

public class TrainingPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ExerciseItem> ExerciseItems { get; set; }
    public List<Exercise> Exercises { get; set; } // 1:N

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Customer Customer { get; set; } = null!;

    public TrainingPlan()
    {
        ExerciseItems = new List<ExerciseItem>();
        Exercises = new List<Exercise>();
    }
}
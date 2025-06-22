namespace GYMNETIC.Core.Models
{
    public class ExerciseItem
    {
        public int Id { get; set; } // Unique identifier for the exercise item
        public required string Exercise { get; set; }
        public int Sets { get; set; } = 1; // Number of sets for the exercise
        public int Repetitions { get; set; } = 1; // Number of repetitions per set
        public int Day { get; set; } // Day of the week (1-7, where 1 is Monday and 7 is Sunday)
        public int ListaEIFId { get; set; }
        public ListaEIF ListaEIF { get; set; } = null!;

    }
}

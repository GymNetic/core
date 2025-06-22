// Models/ListaEIF.cs
namespace GYMNETIC.Core.Models
{
    public class ListaEIF
    {
        public int Id { get; set; }
        public Customer Customer { get; set; } = null!;
        // 1:N
        public ICollection<ExerciseItem> ExerciseItems { get; set; }

        public ListaEIF()
        {
            ExerciseItems = new List<ExerciseItem>();
        }
    }
}
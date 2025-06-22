namespace GYMNETIC.Core.Models
{
    public class NutriPlan
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int? NutricionistaId { get; set; }
        public int StaffId { get; set; } // FK
        public Staff Staff { get; set; } = null!;
        public bool Ativo { get; set; } = true;
        public int AvNutriId { get; set; } // FK
        public AvNutri AvNutri { get; set; } = null!;

        public Customer? Customer { get; set; }
        // Propriedades de navegação
        public ICollection<Customer> Customers { get; set; }

        public NutriPlan()
        {
            Customers = new List<Customer>();
        }
    }
}
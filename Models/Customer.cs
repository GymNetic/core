// Models/Customer.cs
namespace GYMNETIC.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Address { get; set; }
        public string? PostCode { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? LegalName { get; set; }
        public string? NIF { get; set; }

        // Propriedades de associação
        public int MonthlyPlanId { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipEndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CancellationDate { get; set; }

        // 1:1
        public NutriPlan? NutriPlan { get; set; }
        public TrainingPlan? TrainingPlan { get; set; }
        public ListaEIF? ListaEIF { get; set; }

        // 1:N
        public ICollection<AvNutri> AvNutri { get; set; }
        public ICollection<AvFisica> AvFisicas { get; set; }
        public ICollection<GroupClasses> GroupClasses { get; set; } = new List<GroupClasses>();
        public ICollection<GCBooking> GCBookings { get; set; } = new List<GCBooking>();
        
        public Customer()
        {
            AvNutri = new List<AvNutri>();
            AvFisicas = new List<AvFisica>();
            GroupClasses = new List<GroupClasses>();
        }
    }
}
namespace GYMNETIC.Core.Models
{
    public class Staff
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Type { get; set; }
        public ICollection<AvNutri> AvNutri { get; set; } = new List<AvNutri>();
        public ICollection<AvFisica> AvFisica { get; set; } = new List<AvFisica>();
        public ICollection<GroupClasses> GroupClasses { get; set; } = new List<GroupClasses>();
        public ICollection<NutriPlan> NutriPlans { get; set; } = new List<NutriPlan>();

        public ICollection<GCBooking> GCBookings { get; set; } = new List<GCBooking>();
    }
}

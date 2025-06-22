namespace GYMNETIC.Core.Models
{
    public class GroupClasses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int DurationInMinutes { get; set; }
        public string Materials { get; set; }
        public int MaxParticipants { get; set; }
        public bool IsActive { get; set; }
        public string InstructorID { get; set; }
        public string? InstructorPhotoUrl { get; set; }
        public string Type { get; set; }

        public int StaffId { get; set; }
        public Staff Staff { get; set; } = null!;
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();

        // Relação 1:N com GCEvent
        public ICollection<GCEvent> Events { get; set; } = new List<GCEvent>();
    }
}
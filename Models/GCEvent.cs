namespace GYMNETIC.Core.Models
{
    public class GCEvent
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Room { get; set; }
        public bool IsCancelled { get; set; } = false;
        public int CurrentParticipants { get; set; }

        // FK para GroupClasses
        public int GroupClassesId { get; set; }
        public GroupClasses GroupClasses { get; set; } = null!;
        public ICollection<GCBooking> GCBookings { get; set; } = new List<GCBooking>();
    }
}
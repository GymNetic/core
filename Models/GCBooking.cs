namespace GYMNETIC.Core.Models
{
    public class GCBooking
    {
        public int Id { get; set; }

        // Relacionamento com o evento
        public int GCEventId { get; set; }
        public GCEvent GCEvent { get; set; } = null!;

        // Relacionamento com o  Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        // Relacionamento com o Staff
        public int StaffId { get; set; }
        public Staff Staff { get; set; } = null!;

        // Data da inscrição
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        // Status da inscrição (opcional)
        public string Status { get; set; } = "Pending";
    }
}
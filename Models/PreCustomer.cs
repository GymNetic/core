namespace GYMNETIC.Core.Models
{
    public class PreCustomer : Customer
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string AccessCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false;
        public required int MonthlyPlanId { get; set; }
        public MonthlyPlans MonthlyPlan { get; set; } = null!;
    }
}
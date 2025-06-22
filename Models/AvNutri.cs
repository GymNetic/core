namespace GYMNETIC.Core.Models;

public class AvNutri
{
    public int Id { get; set; } // Unique identifier for the nutrition assessment
    public int StaffId { get; set; } // FK para Staff
    public Customer Customer { get; set; } = null!; // Reference to the customer associated with the assessment
    public Staff Staff { get; set; } = null!; // Reference to the staff member who conducted the assessment
    public ICollection<NutriPlan> NutriPlans { get; set; } = new List<NutriPlan>();
    public int CustomerId { get; set; }

}
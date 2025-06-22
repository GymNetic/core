namespace GYMNETIC.Core.Models;

public class AvFisica
{
    public int Id { get; set; }
    public int StaffId { get; set; } // FK para Staff
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public Staff Staff { get; set; } = null!; // Navegação para Staff
}
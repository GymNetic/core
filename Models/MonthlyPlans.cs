namespace GYMNETIC.Core.Models
{
    public class MonthlyPlans
    {
        public int Id { get; set; } 
        public required string PlanName { get; set; }
        public  string? Description { get; set; }
        public  decimal? Price { get; set; }
        public string? Features { get; set; }
        public int DaysToEnter { get; set; }
        public DateTime TimetoEnter { get; set; }
        public DateTime Timetoleave { get; set; }
    }
}

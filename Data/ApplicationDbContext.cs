using GYMNETIC.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GYMNETIC.Core.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);

        }
        base.OnConfiguring(optionsBuilder);
    }

    // Define DbSets for your entities here

    public DbSet<Customer> Customer { get; set; }

    public DbSet<NutriPlan> NutriPlan { get; set; }

    public DbSet<Exercise> Exercise { get; set; }

    public DbSet<TrainingPlan> TrainingPlan { get; set; }

    public DbSet<ExerciseCategory> ExerciseCategory { get; set; }

    public DbSet<ExerciseItem> ExerciseItem { get; set; }

    public DbSet<GCCategory> GCCategory  { get; set; }

    public DbSet<GCEvent> GCEvent { get; set; }

    public DbSet<GroupClasses> GroupClasses { get; set; }

    public DbSet<GCBooking> GCBooking { get; set; }

    public DbSet<MonthlyPlans> MonthlyPlans { get; set; }

    public DbSet<Notifications> Notifications { get; set; }

    public DbSet<PreCustomer> PreCustomer { get; set; }

    public DbSet<Staff> Staff { get; set; }

    public DbSet<ListaEIF> ListaEif { get; set; }
    
    public ICollection<GCBooking> GCBookings { get; set; } = new List<GCBooking>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
    // PreCustomer herda de Customer (TPH por padrão)
    builder.Entity<PreCustomer>().HasBaseType<Customer>();

    // Customer -> NutriPlan (1:1)
    builder.Entity<Customer>()
        .HasOne(c => c.NutriPlan)
        .WithOne(np => np.Customer)
        .HasForeignKey<NutriPlan>(np => np.Id);

    // Customer -> TrainingPlan (1:1)
    builder.Entity<Customer>()
        .HasOne(c => c.TrainingPlan)
        .WithOne(tp => tp.Customer)
        .HasForeignKey<TrainingPlan>(tp => tp.Id);

    // Customer -> ListaEIF (1:1)
    builder.Entity<Customer>()
        .HasOne(c => c.ListaEIF)
        .WithOne(leif => leif.Customer)
        .HasForeignKey<ListaEIF>(leif => leif.Id);

    // ListaEIF -> ExerciseItem (1:N)
    builder.Entity<ListaEIF>()
        .HasMany(leif => leif.ExerciseItems)
        .WithOne(ei => ei.ListaEIF)
        .HasForeignKey(ei => ei.ListaEIFId);

    // TrainingPlan -> Exercise (1:N)
    builder.Entity<TrainingPlan>()
        .HasMany(tp => tp.Exercises)
        .WithOne(e => e.TrainingPlan)
        .HasForeignKey(e => e.TrainingPlanId);

    // Customer -> AvNutri (1:N)
    builder.Entity<Customer>()
        .HasMany(c => c.AvNutri)
        .WithOne(an => an.Customer)
        .HasForeignKey(an => an.CustomerId);

    // AvNutri -> Staff (N:1)
    builder.Entity<AvNutri>()
        .HasOne(an => an.Staff)
        .WithMany(s => s.AvNutri)
        .HasForeignKey(an => an.StaffId);

    // Customer -> AvFisica (1:N)
    builder.Entity<Customer>()
        .HasMany(c => c.AvFisicas)
        .WithOne(af => af.Customer)
        .HasForeignKey(af => af.CustomerId);

    // AvFisica -> Staff (N:1)
    builder.Entity<AvFisica>()
        .HasOne(af => af.Staff)
        .WithMany(s => s.AvFisica)
        .HasForeignKey(af => af.StaffId);

    // GroupClasses -> Staff (N:1)
    builder.Entity<GroupClasses>()
        .HasOne(gc => gc.Staff)
        .WithMany(s => s.GroupClasses)
        .HasForeignKey(gc => gc.StaffId);

    // GroupClasses <-> Customer (N:N)
    builder.Entity<GroupClasses>()
        .HasMany(gc => gc.Customers)
        .WithMany(c => c.GroupClasses)
        .UsingEntity(j => j.ToTable("GroupClassesCustomers"));
     
    
    builder.Entity<NutriPlan>()
        .HasOne(np => np.AvNutri)
        .WithMany(an => an.NutriPlans)
        .HasForeignKey(np => np.AvNutriId);
    
    builder.Entity<NutriPlan>()
        .HasOne(np => np.Staff)
        .WithMany(s => s.NutriPlans)
        .HasForeignKey(np => np.StaffId);
    
    builder.Entity<GroupClasses>()
        .HasMany(gc => gc.Events)
        .WithOne(e => e.GroupClasses)
        .HasForeignKey(e => e.GroupClassesId);
    
    // GCBooking -> GCEvent (N:1)
    builder.Entity<GCBooking>()
        .HasOne(b => b.GCEvent)
        .WithMany(e => e.GCBookings)
        .HasForeignKey(b => b.GCEventId);
    
    // GCBooking -> Customer (N:1)
    builder.Entity<GCBooking>()
        .HasOne(b => b.Customer)
        .WithMany(c => c.GCBookings)
        .HasForeignKey(b => b.CustomerId);
    
    // GCBooking -> Staff (N:1)
    builder.Entity<GCBooking>()
        .HasOne(b => b.Staff)
        .WithMany(s => s.GCBookings)
        .HasForeignKey(b => b.StaffId);
    
    // PreCustomer -> MonthlyPlans (N:1, obrigatório)
    builder.Entity<PreCustomer>()
        .HasOne(pc => pc.MonthlyPlan)
        .WithMany()
        .HasForeignKey(pc => pc.MonthlyPlanId)
        .IsRequired();
    }
    

}
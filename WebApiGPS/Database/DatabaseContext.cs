namespace WebApiGPS.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}
        public DbSet<Tracker>? Trackers { get; set; }
        public DbSet<Car>? Cars { get; set; }
        public DbSet<Geoposition>? Geopositions { get; set; }
        public DbSet<Charge>? Charges { get; set; }
        public DbSet<Person>? Persons { get; set; }
    }
}

using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD.Context
{
    public class TripContext : DbContext
    {
        public TripContext(DbContextOptions<TripContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientTrip> ClientTrips { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryTrip> CountryTrips { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CountryTrip>()
                .HasKey(ct => new { ct.IdCountry, ct.IdTrip });

            modelBuilder.Entity<Trip>()
                .HasKey(t => t.IdTrip);
            
            modelBuilder.Entity<Client>()
                .HasKey(c => c.IdClient);

            modelBuilder.Entity<ClientTrip>()
                .HasKey(ct => new { ct.IdClient, ct.IdTrip });

            modelBuilder.Entity<Country>()
                .HasKey(c => c.IdCountry);
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MHealth.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<UserModel>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { 
        }

        //public DbSet<LocationModel> Locations { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookingModel>()
                .HasKey(e => new { e.Id, e.UserId, e.StaffId });

            modelBuilder.Entity<UserModel>()
                .HasMany<BookingModel>()
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserModel>()
                .HasMany<BookingModel>()
                .WithOne()
                .HasForeignKey(e => e.StaffId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<LocationModel>()
            //    .HasMany<BookingModel>()
            //    .WithOne()
            //    .HasForeignKey(e => e.LocationId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

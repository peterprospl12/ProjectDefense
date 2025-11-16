using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<LecturerAvailability> LecturerAvailabilities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<StudentBlock> StudentBlocks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
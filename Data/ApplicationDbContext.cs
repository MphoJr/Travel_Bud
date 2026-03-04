using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Travel_Bud.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Travel_Bud.Models.Bus> Buses { get; set; }
        public DbSet<Travel_Bud.Models.Route> Routes { get; set; }
        public DbSet<Travel_Bud.Models.Bookings> Bookings { get; set; }

        public DbSet<Travel_Bud.Models.Admin> Admins { get; set; }  

    }
}

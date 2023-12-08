using Microsoft.EntityFrameworkCore;
using SunnyVilla_VallaAPI.Models;

namespace SunnyVilla_VallaAPI.Data
{
    public class ApllicationDbContext : DbContext
    {
        public ApllicationDbContext(DbContextOptions<ApllicationDbContext>options) 
            :base(options)
        {
            
        }
        public DbSet <Villa> Villas{ get; set; }
    }
}

using CV_Handling_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CV_Handling_API.Data
{
    public class CVHandlingDBContext : DbContext
    {
        public CVHandlingDBContext(DbContextOptions<CVHandlingDBContext> options) : base(options)
        {

        }

        public DbSet<Person>Persons { get; set; }

        public DbSet<Education> Educations { get; set; }

        public DbSet<Experience> Experiences { get; set; }
    }
}

using System.Data.Entity;
using Intelectah.Models;

namespace Intelectah.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public DbSet<Fabricante> Fabricantes { get; set; }

        // ...outros DbSets futuramente...
    }
}
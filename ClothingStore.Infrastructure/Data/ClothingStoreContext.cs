using ClothingStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClothingStore.Infrastructure.Data
{
    public partial class ClothingStoreContext : DbContext
    {
        public ClothingStoreContext()
        {
        }
        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> options)
               : base(options)
        {
        }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Security> Security { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}

using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.Constants;

namespace DAL.Context
{
    public class VocaquillDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Query> Queries { get; set; }

        public VocaquillDbContext(DbContextOptions<VocaquillDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Query>()
                .HasOne(q => q.User)
                .WithMany(u => u.Queries)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

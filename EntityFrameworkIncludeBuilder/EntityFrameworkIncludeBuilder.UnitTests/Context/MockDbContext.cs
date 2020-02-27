using EntityFrameworkIncludeBuilder.UnitTests.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkIncludeBuilder.UnitTests.Context
{
    internal class MockDbContext : DbContext
    {
        public MockDbContext(DbContextOptions<MockDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.FirstName).IsRequired().HasMaxLength(128);
            modelBuilder.Entity<User>().Property(x => x.LastName).IsRequired().HasMaxLength(128);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Favorites)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>()
                .HasMany(x => x.MyPosts)
                .WithOne(x => x.CreatedBy)
                .HasForeignKey(x => x.CreatedById);

            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().Property(x => x.Title).IsRequired().HasMaxLength(128);
            modelBuilder.Entity<Post>()
                .HasOne(x => x.CreatedBy)
                .WithMany(x => x.MyPosts)
                .HasForeignKey(x => x.CreatedById);
            modelBuilder.Entity<Post>()
                .HasMany(x => x.Favorites)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<Favorite>().HasKey(x => new { x.UserId, x.PostId });
        }
    }
}

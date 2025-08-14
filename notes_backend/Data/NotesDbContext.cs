using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Data
{
    /// <summary>
    /// Entity Framework Core database context for the notes application.
    /// Manages Users and Notes sets and configures relationships and indexes.
    /// </summary>
    public class NotesDbContext : DbContext
    {
        /// <summary>
        /// Users table.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Notes table.
        /// </summary>
        public DbSet<Note> Notes => Set<Note>();

        /// <summary>
        /// Constructs the DbContext with specified options.
        /// </summary>
        /// <param name="options">Configured DbContext options</param>
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(254);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.HasMany(u => u.Notes)
                      .WithOne(n => n.User!)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Note entity
            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(n => n.Content).IsRequired();
                entity.Property(n => n.Title).HasMaxLength(200);
            });
        }

        /// <summary>
        /// Automatically update CreatedAtUtc/UpdatedAtUtc for tracked entities that have those properties.
        /// </summary>
        private void ApplyTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is User || e.Entity is Note)
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is User user)
                {
                    if (entry.State == EntityState.Added)
                    {
                        user.CreatedAtUtc = now;
                    }
                    user.UpdatedAtUtc = now;
                }
                else if (entry.Entity is Note note)
                {
                    if (entry.State == EntityState.Added)
                    {
                        note.CreatedAtUtc = now;
                    }
                    note.UpdatedAtUtc = now;
                }
            }
        }

        /// <inheritdoc />
        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

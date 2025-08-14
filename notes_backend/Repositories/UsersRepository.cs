using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnet.Data;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Repositories
{
    /// <summary>
    /// EF Core implementation of IUsersRepository.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly NotesDbContext _db;

        /// <summary>
        /// Initializes the repository with the database context.
        /// </summary>
        /// <param name="db">NotesDbContext instance.</param>
        public UsersRepository(NotesDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task<User> CreateAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
            return user;
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return _db.Users
                .Include(u => u.Notes)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            return _db.Users
                .Include(u => u.Notes)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username, ct);
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetAllAsync(int skip = 0, int take = 100, CancellationToken ct = default)
        {
            take = Math.Clamp(take, 1, 200);
            return await _db.Users
                .Include(u => u.Notes)
                .AsNoTracking()
                .OrderBy(u => u.Username)
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct);
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (entity != null)
            {
                _db.Users.Remove(entity);
                await _db.SaveChangesAsync(ct);
            }
        }
    }
}

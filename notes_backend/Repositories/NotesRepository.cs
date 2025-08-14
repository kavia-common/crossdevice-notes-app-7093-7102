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
    /// EF Core implementation of INotesRepository.
    /// </summary>
    public class NotesRepository : INotesRepository
    {
        private readonly NotesDbContext _db;

        /// <summary>
        /// Initializes the repository with the database context.
        /// </summary>
        /// <param name="db">NotesDbContext instance.</param>
        public NotesRepository(NotesDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task<Note> CreateAsync(Note note, CancellationToken ct = default)
        {
            _db.Notes.Add(note);
            await _db.SaveChangesAsync(ct);
            return note;
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return _db.Notes
                .Include(n => n.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task<IEnumerable<Note>> GetByUserAsync(Guid userId, int skip = 0, int take = 100, CancellationToken ct = default)
        {
            take = Math.Clamp(take, 1, 200);
            return await _db.Notes
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.UpdatedAtUtc)
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct);
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task UpdateAsync(Note note, CancellationToken ct = default)
        {
            _db.Notes.Update(note);
            await _db.SaveChangesAsync(ct);
        }

        // PUBLIC_INTERFACE
        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id, ct);
            if (entity != null)
            {
                _db.Notes.Remove(entity);
                await _db.SaveChangesAsync(ct);
            }
        }
    }
}

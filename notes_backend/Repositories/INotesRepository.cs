using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dotnet.Models;

namespace dotnet.Repositories
{
    /// <summary>
    /// Repository contract for note data operations.
    /// </summary>
    public interface INotesRepository
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="note">Note entity to create.</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Created note entity.</returns>
        Task<Note> CreateAsync(Note note, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieves a note by primary key.
        /// </summary>
        /// <param name="id">Note identifier.</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Note if found; otherwise null.</returns>
        Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Lists notes for a specific user with pagination and most-recent-first sorting.
        /// </summary>
        /// <param name="userId">Owner user id.</param>
        /// <param name="skip">Number of notes to skip.</param>
        /// <param name="take">Number of notes to take (max page size).</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Enumerable of notes.</returns>
        Task<IEnumerable<Note>> GetByUserAsync(Guid userId, int skip = 0, int take = 100, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Updates a note entity.
        /// </summary>
        /// <param name="note">Note to update.</param>
        /// <param name="ct">CancellationToken</param>
        Task UpdateAsync(Note note, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Deletes a note by id.
        /// </summary>
        /// <param name="id">Note id.</param>
        /// <param name="ct">CancellationToken</param>
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}

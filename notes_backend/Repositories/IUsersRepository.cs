using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dotnet.Models;

namespace dotnet.Repositories
{
    /// <summary>
    /// Repository contract for user data operations.
    /// </summary>
    public interface IUsersRepository
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">User entity to create.</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Created user entity.</returns>
        Task<User> CreateAsync(User user, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieves a user by primary key.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>User if found; otherwise null.</returns>
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">Unique username.</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>User if found; otherwise null.</returns>
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Lists users with pagination.
        /// </summary>
        /// <param name="skip">Number of users to skip.</param>
        /// <param name="take">Number of users to take (max page size).</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Enumerable of users.</returns>
        Task<IEnumerable<User>> GetAllAsync(int skip = 0, int take = 100, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Updates a user entity.
        /// </summary>
        /// <param name="user">User to update.</param>
        /// <param name="ct">CancellationToken</param>
        Task UpdateAsync(User user, CancellationToken ct = default);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="ct">CancellationToken</param>
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Models
{
    /// <summary>
    /// Represents an application user with authentication data and related notes.
    /// </summary>
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        /// <summary>
        /// Primary key identifier for the user.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Unique username for login and identification.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Unique email address for the user.
        /// </summary>
        [Required]
        [MaxLength(254)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password for authentication.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp for when the user was created (UTC).
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp for when the user was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property: Notes that belong to this user.
        /// </summary>
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}

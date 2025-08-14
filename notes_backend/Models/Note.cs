using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Models
{
    /// <summary>
    /// Represents a note created by a user with content and timestamps.
    /// </summary>
    [Index(nameof(UserId))]
    public class Note
    {
        /// <summary>
        /// Primary key identifier for the note.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Optional title of the note.
        /// </summary>
        [MaxLength(200)]
        public string? Title { get; set; }

        /// <summary>
        /// Body/content of the note.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Owner foreign key.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Navigation property: Owner user.
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Timestamp for when the note was created (UTC).
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp for when the note was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}

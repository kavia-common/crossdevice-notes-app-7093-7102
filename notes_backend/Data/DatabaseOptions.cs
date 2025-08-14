namespace dotnet.Data
{
    /// <summary>
    /// Database configuration options that may be provided via environment variables or appsettings.
    /// DB_PROVIDER and DB_CONNECTION_STRING env vars take precedence.
    /// </summary>
    public class DatabaseOptions
    {
        /// <summary>
        /// The database provider to use (e.g., "Sqlite"). Currently only "Sqlite" is supported by default.
        /// </summary>
        public string Provider { get; set; } = "Sqlite";

        /// <summary>
        /// The database connection string appropriate for the chosen provider.
        /// For Sqlite, a typical value is "Data Source=notes.db".
        /// </summary>
        public string ConnectionString { get; set; } = "Data Source=notes.db";
    }
}

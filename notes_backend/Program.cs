using System;
using dotnet.Data;
using dotnet.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

// Bind database options (env vars take precedence)
var dbProvider = Environment.GetEnvironmentVariable("DB_PROVIDER")
    ?? builder.Configuration["Database:Provider"]
    ?? "Sqlite";

var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["Database:ConnectionString"]
    ?? "Data Source=notes.db";

// Register EF Core DbContext
builder.Services.AddDbContext<NotesDbContext>(options =>
{
    if (dbProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(dbConnectionString);
    }
    else
    {
        // NOTE: For now, only Sqlite is supported by default. Add other providers as needed.
        throw new InvalidOperationException($"Unsupported DB_PROVIDER: {dbProvider}. Supported: Sqlite");
    }
});

// Register repositories (data-access patterns)
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure database is created (for initial scaffolding; replace with migrations if needed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    db.Database.EnsureCreated();
}

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Health check endpoint
// PUBLIC_INTERFACE
app.MapGet("/", () => new { message = "Healthy" })
   .WithTags("System");

// PUBLIC_INTERFACE
/// <summary>
/// Database health endpoint to verify connectivity.
/// </summary>
/// <returns>JSON with provider and basic status.</returns>
app.MapGet("/db/health", (NotesDbContext dbCtx) =>
{
    // simple query to touch the database
    var canConnect = dbCtx.Database.CanConnect();
    return Results.Ok(new
    {
        provider = dbProvider,
        connected = canConnect
    });
})
.WithSummary("Database health")
.WithDescription("Checks that the API can connect to the configured database.")
.WithTags("System");

app.Run();

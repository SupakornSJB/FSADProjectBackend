using FSADProjectBackend.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace FSADProjectBackend.Contexts;

public class MongoDbContext: DbContext
{
    public DbSet<Problem> Problems { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Problem>().ToCollection("Problems");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMongoDB("mongodb://mongo:27017", "Database");  // Todo: Change to use environment variable
    }
}
using FSADProjectBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FSADProjectBackend.Contexts;

public class PgDbContext(DbContextOptions<PgDbContext> options) : DbContext(options)
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}
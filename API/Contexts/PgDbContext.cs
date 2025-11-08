using FSADProjectBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FSADProjectBackend.Contexts;

public class PgDbContext(DbContextOptions<PgDbContext> options) : DbContext(options)
{
    public DbSet<ProblemSolver> ProblemSolvers { get; set; }
    public DbSet<UserProblemCommentVoteMapping> UserCommentVoteMappings { get; set; }
    public DbSet<UserProblemSolverMapping> UserProblemSolverMappings { get; set; }
    public DbSet<UserProblemVoteMapping> UserProblemVoteMappings { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ProblemTagMapping> ProblemTagMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProblemCommentVoteMapping>()
            .HasKey(x => new { x.UserSubject, x.ProblemId, x.CommentId });
        
        // Problem Solver Mappings
        modelBuilder.Entity<UserProblemSolverMapping>()
            .HasKey(x => new { x.ProblemSolverId, x.UserSubject });
        modelBuilder.Entity<UserProblemSolverMapping>()
            .HasOne(x => x.ProblemSolver)
            .WithMany(x => x.MembersMapping)
            .HasForeignKey(x => x.ProblemSolverId);
        
        modelBuilder.Entity<UserProblemVoteMapping>()
            .HasKey(x => new { x.UserSubject, x.ProblemId });
        
        // Tag Mapping
        modelBuilder.Entity<ProblemTagMapping>()
            .HasKey(x => new { x.Tag.Id, x.ProblemId });
        modelBuilder.Entity<ProblemTagMapping>()
            .HasOne(x => x.Tag)
            .WithMany(x => x.ProblemTagMappings)
            .HasForeignKey(x => x.TagId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}
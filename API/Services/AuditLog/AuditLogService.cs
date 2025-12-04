using FSADProjectBackend.Contexts;
using FSADProjectBackend.Models;
using FSADProjectBackend.Services.AuditLog;
using Microsoft.EntityFrameworkCore;

public class AuditLogService : IAuditLogService
{
    private static MongoDbContext _mongoDbContext;
    
    public AuditLogService(MongoDbContext mongoDbContext) 
    {
        _mongoDbContext = mongoDbContext;
    }

    public async Task CreateAsync(AuditLog log)
    {
        log.CreatedAt = DateTime.UtcNow;
        _mongoDbContext.AuditLogs.Add(log);
        await _mongoDbContext.SaveChangesAsync();
    }

    public async Task<AuditLog?> GetByIdAsync(string id)
    {
        return await _mongoDbContext.AuditLogs.FindAsync(id);
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _mongoDbContext.AuditLogs
            .OrderBy(x => x.CreatedAt).ToListAsync();
    }

    public async Task<List<AuditLog>> GetPagedAsync(int page, int pageSize)
    {
        return await _mongoDbContext.AuditLogs
                                .OrderBy(x => x.CreatedAt)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var log = await GetByIdAsync(id);
        if (log == null) return;
        _mongoDbContext.Remove(log);
    }
}

namespace FSADProjectBackend.Services.AuditLog;

public interface IAuditLogService
{
    Task CreateAsync(Models.AuditLog log);
    Task<Models.AuditLog?> GetByIdAsync(string id);
    Task<List<Models.AuditLog>> GetAllAsync();
    Task<List<Models.AuditLog>> GetPagedAsync(int page, int pageSize);
    Task DeleteAsync(string id);
}
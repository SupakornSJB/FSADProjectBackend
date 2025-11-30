using FSADProjectBackend.Models;
using FSADProjectBackend.Services.AuditLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AuditLogController : ControllerBase
{
    private readonly IAuditLogService _service;

    public AuditLogController(IAuditLogService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AuditLog log)
    {
        await _service.CreateAsync(log);
        return Ok(new { message = "Audit log created." });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuditLog>> GetById(string id)
    {
        var log = await _service.GetByIdAsync(id);
        if (log == null) return NotFound();
        return Ok(log);
    }

    [HttpGet]
    public async Task<ActionResult<List<AuditLog>>> GetAll()
    {
        var logs = await _service.GetAllAsync();
        return Ok(logs);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<List<AuditLog>>> GetPaged(
        int page = 1, int pageSize = 20)
    {
        var logs = await _service.GetPagedAsync(page, pageSize);
        return Ok(logs);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { message = "Audit log deleted." });
    }
}

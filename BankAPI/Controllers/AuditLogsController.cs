using BankAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly BankDbContext context;

        public AuditLogsController(BankDbContext context)
        {
            this.context = context;
        }
        [HttpGet("audit")]
        public async Task<IActionResult> GetAuditLogs()
        {
            var auditLogs = await context.AuditLogs.ToListAsync();
            return Ok(auditLogs);
        }
    }
}

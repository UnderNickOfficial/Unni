using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unni.Infrastructure.Logger.Services;
using Unni.Infrastructure.Logger.Services.Interfaces;
using Unni.Samples.WebApiApp.Contexts;

namespace Unni.Samples.WebApiApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DatabaseLoggerController : ControllerBase
    {
        private readonly IDatabaseLoggerService<UnniDbContext> _databaseLoggerService;

        public DatabaseLoggerController(IDatabaseLoggerService<UnniDbContext> databaseLoggerService)
        {
            _databaseLoggerService = databaseLoggerService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateTextException()
        {
            await _databaseLoggerService.CreateAsync(new Infrastructure.Logger.Models.AppException()
            {
                InnerException= "Test inner exception",
                Message = "Test exception message",
            }, "testId");
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;

namespace Receivables.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceivablesController : ControllerBase
    {
        private readonly IReceivableService _receivableService;

        public ReceivablesController(IReceivableService receivableService)
        {
            _receivableService = receivableService;
        }

        [HttpGet(Name = "SummaryStatitics")]
        public async Task<IActionResult> GetSummaryStatistics()
        {
            var dto = await _receivableService.GetSummaryStatistics();
            return Ok(dto);
        }

        [HttpPost(Name = "Receivables")]
        public async Task<IActionResult> Post([FromBody] IEnumerable<ReceivableDTO> receivableDTOs)
        {
            try
            {
                await _receivableService.AddReceivables(receivableDTOs);
                return Ok("Receivables added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to add receivables: " + ex.Message);
            }
        }
    }
}
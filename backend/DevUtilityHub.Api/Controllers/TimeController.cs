using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevUtilityHub.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly ITimestampService _service;

        public TimeController(ITimestampService service) => _service = service;

        [HttpPost("convert")]
        public IActionResult Convert([FromBody] TimeRequest request)
            => Ok(_service.Convert(request));
    }
}

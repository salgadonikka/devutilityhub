using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevUtilityHub.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FormatController : ControllerBase
	{
		private readonly IFormatService _service;

		public FormatController(IFormatService service) => _service = service;

		[HttpPost("process")]
		public IActionResult Process([FromBody] FormatRequest request)
			=> Ok(_service.Process(request));
	}
}

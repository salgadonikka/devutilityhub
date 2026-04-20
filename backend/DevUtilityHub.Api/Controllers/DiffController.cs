using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevUtilityHub.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DiffController : ControllerBase
	{
		private readonly IDiffService _service;

		public DiffController(IDiffService service) => _service = service;

		[HttpPost("compare")]
		public IActionResult Compare([FromBody] DiffRequest request)
			=> Ok(_service.Compare(request));
	}
}

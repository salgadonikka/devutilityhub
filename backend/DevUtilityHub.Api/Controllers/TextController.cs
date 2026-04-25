using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevUtilityHub.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TextController : ControllerBase
	{
		private readonly ITextTransformService _service;

		public TextController(ITextTransformService service) => _service = service;

		[HttpPost("transform")]
		public IActionResult Transform([FromBody] TextTransformRequest request)
			=> Ok(_service.Transform(request));
	}
}

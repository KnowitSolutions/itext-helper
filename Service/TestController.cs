using Microsoft.AspNetCore.Mvc;

namespace Service
{
	public class TestController : ControllerBase
    {
		[HttpGet, Route("api/test")]
		public IActionResult Test()
		{
			return Ok("Test called");
		}
    }
}

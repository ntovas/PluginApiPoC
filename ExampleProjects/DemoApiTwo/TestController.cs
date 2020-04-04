using System;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiTwo
{
	[ApiController]
	[Route("[controller]")]
	public class TestController : ControllerBase
	{
		public IActionResult Get()
		{
			return Ok("DemoApiTwo");
		}
	}
}

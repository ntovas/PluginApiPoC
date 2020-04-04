using System;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiOne
{
	[ApiController]
	[Route("[controller]")]
	public class HomeController : ControllerBase
	{
		public IActionResult Get()
		{
			return Ok("DemoApiOne");
		}
	}
}

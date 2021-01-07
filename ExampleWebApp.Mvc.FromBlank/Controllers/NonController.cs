using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

[NonController]
public class NonController : Controller
{
		private IWebHostEnvironment _env;
		private ILogger<FileController> _logger;

		public NonController(IWebHostEnvironment env, ILogger<FileController> logger)
		{
				_env = env;
				_logger = logger;
		}

		public IActionResult Index()
		{
				return Ok("Hello from NonController.");
		}
}

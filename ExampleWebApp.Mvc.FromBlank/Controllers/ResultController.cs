using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

public class User
{
		public string Name { get; set; }
		public int Age { get; set; }
}

public class ResultController : Controller
{

		private IWebHostEnvironment _env;
		private ILogger<FileController> _logger;

		public ResultController(IWebHostEnvironment env, ILogger<FileController> logger)
		{
				_env = env;
				_logger = logger;
		}

		public IActionResult Index()
		{
				return Ok("Hello");
		}

		public User SerializedObject()
		{
				User user = new User
				{
						Name = "User name",
						Age = 20
				};

				return user;
		}

		public (string, bool) Cortege()
		{
				return ("Hello Cortege!", true);
		}


		[NonAction]
		public IActionResult NonAction()
		{
				return Ok("Non Action");
		}

		[ActionName("ChangedActionName")]
		public IActionResult SomeActionName()
		{
				return Ok("SomeActionName::ChangedActionName");
		}
}

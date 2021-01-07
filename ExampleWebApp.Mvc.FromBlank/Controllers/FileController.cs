using System;
using System.IO;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

public class FileController : Controller
{
		private IWebHostEnvironment _env;
		private ILogger<FileController> _logger;

		public FileController(IWebHostEnvironment env, ILogger<FileController> logger)
		{
				_env = env;
				_logger = logger;
		}

		public IActionResult Index()
		{
				return Ok("FileController.");
		}

		public FileContentResult GetFileContent()
		{
				byte[] fileContent = new byte[8] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

				FileContentResult result = new FileContentResult(fileContent, "application/octet-stream");

				return result;
		}

		public FileStreamResult GetFileStream()
		{
			FileStream stream = System.IO.File.OpenRead("./wwwroot/file.bin");

			FileStreamResult result = new FileStreamResult(stream, "application/octet-stream");

			return result;
		}

		public VirtualFileResult GetVirtualFile()
		{
				VirtualFileResult result = new VirtualFileResult("~/file.bin", "application/octet-stream");

				return result;
		}

		public PhysicalFileResult GetPhysicalFile()
		{
				_logger.LogInformation(_env.WebRootPath);

				PhysicalFileResult result = new PhysicalFileResult($"{_env.WebRootPath}/file.bin", "application/octet-stream");

				return result;
		}
}

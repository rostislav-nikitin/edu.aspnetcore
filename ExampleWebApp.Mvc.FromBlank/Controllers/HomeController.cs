using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using ExampleWebApp.Mvc.FromBlank.Models;

namespace ExampleWebApp.Mvc.FromBlank.Controllers
{
    public class HomeController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly List<InfoModel> _models;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
			_models = new List<InfoModel>()
			{
				new InfoModel() { CustomProperty = "Some text #1" },
				new InfoModel() { CustomProperty = "Some text #2" },
				new InfoModel() { CustomProperty = "Some text #3" },
				new InfoModel() { CustomProperty = "Some text #4" }
			};
		}

		public string IndexAsString()
		{
			_logger.LogInformation("Home::IndexAsString was executed");

			return _models[0].CustomProperty;
		}

		public IActionResult IndexByViewData()
		{
			_logger.LogInformation("Home::IndexByViewData was executed");

			ViewData["CustomKey"] = "ViewData[\"CustomKey\"]";
			ViewBag.CustomProperty = "ViewBag.CustomProperty";

			return View("IndexByViewData");
		}

		public IActionResult Index()
		{
			_logger.LogInformation("Home::Index was executed");

			return View(_models);
		}

		public IActionResult AcceptByParameters(string customProperty)
		{
			return Ok(customProperty);
		}

		public IActionResult AcceptByModel(InfoModel model)
		{
			return View("Index", new List<InfoModel> { model });
		}

		public IActionResult AcceptByModelsList(InfoModel[] models)
		{
			return View("Index", new List<InfoModel>(models));
		}

		public IActionResult InlineDemo()
		{
			return View();
		}

		public IActionResult DependencyInjection()
		{
			return View();
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace ExampleWebApp.Mvc.FromBlank.Models
{
    public class InfoModel
    {
			public string CustomProperty { get; set; }

			public string CurrentTime
			{
					get
					{
							return DateTime.UtcNow.ToString();
					}
			}
    }
}

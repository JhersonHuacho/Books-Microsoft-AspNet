using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoggingController : ControllerBase
	{
		private readonly ILogger<LoggingController> _logger;

		public LoggingController(ILogger<LoggingController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[Route("structured-logging")]
		public ActionResult GetStructuredLoggingSample()
		{
			_logger.LogInformation("This is a logging message with args: Today is { Week }. It is { Time }.", DateTime.Now.DayOfWeek, DateTime.Now.ToLongTimeString());
			_logger.LogInformation($"This is a logging message with stringconcatenation: Today is { DateTime.Now.DayOfWeek }. It is{DateTime.Now.ToLongTimeString()}.");
			
			return Ok("This is to test the difference between structuredlogging and string concatenation.");
		}
	}
}

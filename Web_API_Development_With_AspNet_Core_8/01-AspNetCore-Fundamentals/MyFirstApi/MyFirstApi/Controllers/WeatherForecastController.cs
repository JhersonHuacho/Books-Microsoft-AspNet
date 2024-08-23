using Microsoft.AspNetCore.Mvc;

namespace MyFirstApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			_logger.Log(LogLevel.Information, "This is a logging message.");
			_logger.LogInformation("This is a logging message.");
			_logger.LogTrace("This is a trace message.");
			_logger.LogInformation($"This is a logging message with stringconcatenation: Today is {DateTime.Now.DayOfWeek}. It is {DateTime.Now.ToLongTimeString()}.");

			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}
}

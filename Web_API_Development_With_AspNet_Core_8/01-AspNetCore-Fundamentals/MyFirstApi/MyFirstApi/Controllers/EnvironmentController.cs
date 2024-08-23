using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EnvironmentController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public EnvironmentController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("database-configuration")]
		public ActionResult GetDatabaseConfiguration()
		{
			var type = _configuration["Database:Type"];
			var connectionString = _configuration["Database:ConnectionString"];
			return Ok(new { Type = type, ConnectionString = connectionString });
		}
	}
}

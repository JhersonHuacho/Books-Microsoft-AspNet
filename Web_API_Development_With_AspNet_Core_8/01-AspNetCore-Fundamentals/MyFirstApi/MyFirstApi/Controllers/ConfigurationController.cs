using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyFirstApi.Models;
using System.Xml.Linq;

namespace MyFirstApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConfigurationController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IOptions<DatabaseOption> _options;
		private readonly IOptionsSnapshot<DatabaseOption> _optionsSnapshot;
		private readonly IOptionsMonitor<DatabaseOption> _optionsMonitor;
		private readonly IOptionsSnapshot<DatabaseOptions> _namedOptionsSnapshot;


		public ConfigurationController(IConfiguration configuration, IOptions<DatabaseOption> options, IOptionsSnapshot<DatabaseOption> optionsSnapshot, IOptionsMonitor<DatabaseOption> optionsMonitor, IOptionsSnapshot<DatabaseOptions> namedOptionsSnapshot)
		{
			_configuration = configuration;
			_options = options;
			_optionsSnapshot = optionsSnapshot;
			_optionsMonitor = optionsMonitor;
			_namedOptionsSnapshot = namedOptionsSnapshot;
		}

		[HttpGet]
		[Route("my-key")]
		public ActionResult GetMyKey()
		{
			var myKey = _configuration["my-key"];
			return Ok(myKey);
		}

		[HttpGet]
		[Route("database-configuration")]
		public ActionResult GetDatabaseConfiguration()
		{
			var type = _configuration["Database:Type"];
			var connectionString = _configuration["Database:ConnectionString"];
			return Ok(new { Type = type, ConnectionString = connectionString });
		}

		[HttpGet]
		[Route("database-configuration-with-bind")]
		public ActionResult GetDatabaseConfigurationWithBind()
		{
			var databaseOption = new DatabaseOption();
			// The `SectionName` is defined in the `DatabaseOption` class, which shows the section name in the `appsettings.json` file.
			_configuration.GetSection(DatabaseOption.SectionName).Bind(databaseOption);
			// You can also use the code below to achieve the same result
			// configuration.Bind(DatabaseOption.SectionName, databaseOption);
			return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
		}

		[HttpGet]
		[Route("database-configuration-with-generic-type")]
		public ActionResult GetDatabaseConfigurationWithGenericType()
		{
			var databaseOption = _configuration.GetSection(DatabaseOption.SectionName).Get<DatabaseOption>();

			return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
		}

		[HttpGet]
		[Route("database-configuration-with-ioptions")]
		public ActionResult GetDatabaseConfigurationWithIOptions()
		{
			var databaseOption = _options.Value;

			return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
		}

		[HttpGet]
		[Route("database-configuration-with-ioptions-snapshot")]
		public ActionResult GetDatabaseConfigurationWithIOptionsSnapshot()
		{
			var databaseOption = _optionsSnapshot.Value;

			return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
		}

		[HttpGet]
		[Route("database-configuration-with-ioptions-monitor")]
		public ActionResult GetDatabaseConfigurationWithIOptionsMonitor()
		{
			var databaseOption = _optionsMonitor.CurrentValue;

			return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
		}

		[HttpGet]
		[Route("database-configuration-with-named-options")]
		public ActionResult GetDatabaseConfigurationWithNamedOptions()
		{
			var systemDatabaseOption = _namedOptionsSnapshot.Get(DatabaseOptions.SectionName);
			var businessDatabaseOption = _namedOptionsSnapshot.Get(DatabaseOptions.BusinessDatabaseSectionName);

			return Ok(new {
				SystemDatabaseOption = systemDatabaseOption, 
				BusinessDatabaseOption  = businessDatabaseOption 
			});
		}		
	}
}

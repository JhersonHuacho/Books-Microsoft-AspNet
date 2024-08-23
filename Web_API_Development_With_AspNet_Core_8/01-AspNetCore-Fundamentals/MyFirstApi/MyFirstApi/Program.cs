using MyFirstApi;
using MyFirstApi.Services;
using Serilog;
using Serilog.Formatting.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
// Logs
builder.Logging.ClearProviders();
#region Logging
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

//var logger = new LoggerConfiguration().WriteTo
//	.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt"), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90)
//	.WriteTo.Console(new JsonFormatter())
//	.CreateLogger();
//builder.Logging.AddSerilog(logger);

var loggerWithSeq = new LoggerConfiguration().WriteTo
	.File(
		formatter: new JsonFormatter(),
		Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt"),
		rollingInterval: RollingInterval.Day,
		retainedFileCountLimit: 90)
	.WriteTo.Console(new JsonFormatter())
	.WriteTo.Seq("http://localhost:5341")
	.CreateLogger();
builder.Logging.AddSerilog(loggerWithSeq);
#endregion

#region Configuration
//// Register the DatabaseOption class as a configuration object.
//// This line must be added before the `builder.Build()` method.
//builder.Services.Configure<DatabaseOption>(builder.Configuration.GetSection("DatabaseOptions"));

//// named options
//builder.Services.Configure<DatabaseOptions>(
//	DatabaseOptions.SystemDatabaseSectionName,
//	builder.Configuration.GetSection($"{DatabaseOptions.SectionName}: {DatabaseOptions.SystemDatabaseSectionName}"));

//builder.Services.Configure<DatabaseOptions>(
//	DatabaseOptions.BusinessDatabaseSectionName,
//	builder.Configuration.GetSection($"{DatabaseOptions.SectionName}: {DatabaseOptions.BusinessDatabaseSectionName}"));

builder.Services.AddConfig(builder.Configuration);
#endregion

#region Services
// Add services to the container.
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IDemoService, DemoService>();

//builder.Services.AddScoped<IScopedService, ScopedService>();
//builder.Services.AddTransient<ITransientService, TransientService>();
//builder.Services.AddSingleton<ISingletonService, SingletonService>();
// Group registration
builder.Services.AddLifetimeServices();
#endregion


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region Middleware
app.Map("/lottery", app =>
{
	var random = new Random();
	var luckyNumber = random.Next(1, 6);

	app.UseWhen(context => context.Request.QueryString.Value == $"?{luckyNumber.ToString()}", app =>
	{
		app.Run(async context =>
		{
			await context.Response.WriteAsync($"You win! You got thelucky number { luckyNumber}! random{random}");
		});
	});

	app.UseWhen(context => string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app =>
	{
		app.Use(async (context, next) =>
		{
			var number = random.Next(1, 6);
			context.Request.Headers.TryAdd("number", number.ToString());
			await next(context);
		});
		app.UseWhen(context => context.Request.Headers["number"] == luckyNumber.ToString(), app =>
		{
			app.Run(async context =>
			{
				await context.Response.WriteAsync($"You win! You gotthe lucky number {luckyNumber}!");
			});
		});
	});

	app.Run(async context =>
	{
		var number = "";
		if (context.Request.QueryString.HasValue)
		{
			number = context.Request.QueryString.Value?.Replace("?", "");
		}
		else
		{
			number = context.Request.Headers["number"];
		}

		await context.Response.WriteAsync($"Your number is {number}.Try again! luckyNumber {luckyNumber}");
	});
});

app.Run(async context =>
{
	await context.Response.WriteAsync($"Use the /lottery URL to play.You can choose your number with the format /lottery?1.");
});
#endregion

app.Run();

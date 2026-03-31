using ComputingServers.Application.Abstractions;
using ComputingServers.Infrastructure.Services;
using Serilog;
using Serilog.Events;
using Serilog.Settings.Configuration;
using Serilog.Sinks.SystemConsole.Themes;

namespace ComputingServersApp.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddLoggerService(this IServiceCollection services, WebApplicationBuilder builder)
	{
		var defaultOutput = "{Timestamp:HH:mm:ss} [{Level}] [{CallerClass}: in {CallerMember}() {CallerLine}]: {Message}{NewLine}{Exception}";
		var appDir = AppContext.BaseDirectory;

		Log.Logger = new LoggerConfiguration()
			.ReadFrom.Configuration(builder.Configuration, new ConfigurationReaderOptions
			{
				SectionName = "Serilog"
			})
			.Enrich.FromLogContext()
			// console settings
			.WriteTo.Logger(loggerConfig =>
				loggerConfig
					.Filter.ByExcluding(e => e.Properties.ContainsKey("Subfolder") || e.Properties.ContainsKey("Subfolder_Daily"))
					.WriteTo.Console(
						theme: AnsiConsoleTheme.Sixteen,
						outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] <s:{CallerClass} in {CallerMember}()> {Message:lj}{NewLine}{Exception}"))
			// error settings
			.WriteTo.Logger(loggerConfig =>
			{
				loggerConfig
					.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
					.WriteTo.Map(logEvent => logEvent.Timestamp.Date,
					(date, writeTo) => writeTo.File(
							path: Path.Combine(appDir, "logs", $"{date:yyyy-MM-dd}/error_log.txt"),
							outputTemplate: defaultOutput));
			})
			// general log settings
			.WriteTo.Logger(loggerConfig =>
			{
				loggerConfig
					.Filter.ByExcluding(e => e.Level == LogEventLevel.Error || e.Properties.ContainsKey("Subfolder") || e.Properties.ContainsKey("Subfolder_Daily"))
					.WriteTo.Map(logEvent => logEvent.Timestamp.Date,
						(date, writeTo) => writeTo.File(
							path: Path.Combine(appDir, "logs", $"{date:yyyy-MM-dd}/logs.txt"),
							outputTemplate: defaultOutput));
			})
			.WriteTo.Logger(loggerConfig =>
			{
				loggerConfig
			.Filter.ByIncludingOnly(e => e.Properties.ContainsKey("Subfolder_Daily"))
			.WriteTo.Map(
				keyPropertyName: "Subfolder_Daily_FileName",
				defaultKey: "GeneralDaily_default.txt",
				configure: (key, wt) =>
				{
					wt.File(
						path: Path.Combine(appDir, "logs", $"{DateTime.Now:yyyy-MM-dd}/{key}"),
						outputTemplate: "{Timestamp:HH:mm:ss} [{CallerClass}: in {CallerMember}() {CallerLine}]: {Message}{NewLine}{Exception}"
					);
				});
			})
			.Enrich.WithMachineName()
			.CreateLogger();

		builder.Host.UseSerilog();

		services.AddSingleton<ILoggerManager, SerilogManager>();
	}
}

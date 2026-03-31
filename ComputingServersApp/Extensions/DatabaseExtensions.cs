using ComputingServers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputingServersApp.Extensions;

public static class DatabaseExtensions
{
	public static async Task ApplyMigrationsAsync(this WebApplication app)
	{
		using IServiceScope scope = app.Services.CreateScope();
		await using AppDbContext applicationDbContext =
			scope.ServiceProvider.GetRequiredService<AppDbContext>();

		try
		{
			await applicationDbContext.Database.MigrateAsync();
			app.Logger.LogInformation("Database migrations applied successfully.");
		}
#pragma warning disable S2139
		catch (Exception e)
#pragma warning restore S2139
		{
			app.Logger.LogError(e, "An error occurred while applying database migrations.");
			throw;
		}
	}
}

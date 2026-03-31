using ComputingServers.Application;
using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Repositories;
using ComputingServers.Infrastructure.Persistence;
using ComputingServers.Infrastructure.Repositories;
using ComputingServers.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ComputingServers.Infrastructure;

public static class Infrastructure
{
	public static void Configure(IServiceCollection services, IConfiguration configuration)
	{
		string databaseConnectionString = configuration.GetConnectionString("Database")!;

		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(databaseConnectionString));

		services.AddScoped<IServerRepository, ServerRepository>();

		services.AddScoped<IQuartzService, QuartzService>();
		services.AddQuartz(q =>
		{
			q.UseMicrosoftDependencyInjectionJobFactory();
		});

		services.AddQuartzHostedService(options =>
		{
			options.WaitForJobsToComplete = true;
		});

		services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssemblies(ApplicationAssemblyReference.Assembly);
		});
	}
}

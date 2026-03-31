using ComputingServers.Application.Abstractions;
using ComputingServers.Domain.Entities;
using ComputingServers.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ComputingServers.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
	internal DbSet<Server> Servers { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(Schemas.Servers);

		modelBuilder.ApplyConfiguration(new ServerConfiguration());
	}

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return base.SaveChangesAsync(cancellationToken);
	}
}

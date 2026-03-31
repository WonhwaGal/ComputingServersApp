using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Entities;
using ComputingServers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ComputingServers.Infrastructure.Repositories;

public class ServerRepository(AppDbContext appDbContext) : IServerRepository
{
	public void Add(Server server)
	{
		appDbContext.Servers.Add(server);
	}

	public async Task<List<Server>> GetAvailableAsync(Expression<Func<Server, bool>> filter, bool readOnly = false)
	{
		return await (readOnly ? appDbContext.Servers.AsNoTracking() : appDbContext.Servers)
			.Where(filter)
			.ToListAsync();
	}

	public async Task<Server?> GetByIdAsync(string id, bool readOnly = false)
	{
		return await (readOnly ? appDbContext.Servers.AsNoTracking() : appDbContext.Servers)
			.Where(s => s.Id == id)
			.FirstOrDefaultAsync();
	}

	public async Task<List<Server>> GetAllAsync(bool readOnly = false)
		=> await (readOnly ? appDbContext.Servers.AsNoTracking() : appDbContext.Servers).ToListAsync();
}

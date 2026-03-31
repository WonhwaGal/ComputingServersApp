

using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Enums;
using Quartz;

namespace ComputingServers.Infrastructure.Jobs;

public sealed class ServerReleaseJob(
	IUnitOfWork unitOfWork,
	IServerRepository serverRepository) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		var serverId = context.MergedJobDataMap.GetString("ServerId");
		if (serverId is null)
			return;

		var server = await serverRepository.GetByIdAsync(serverId);

		if (server != null)
		{
			server.LastUsed = DateTime.UtcNow;
			server.Status = ServerStatus.PoweredOff;
			server.IsAvailable = true;
			await unitOfWork.SaveChangesAsync();
		}
	}
}

using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Enums;
using Quartz;

namespace ComputingServers.Infrastructure.Jobs
{
	public sealed class ServerPowerOnJob(
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
				server.Status = ServerStatus.PoweredOn;
				await unitOfWork.SaveChangesAsync();
			}
		}
	}
}

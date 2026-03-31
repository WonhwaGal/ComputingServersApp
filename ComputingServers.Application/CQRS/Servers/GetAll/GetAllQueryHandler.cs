using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Dtos;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.GetAll;

internal class GetAllQueryHandler(
	IServerRepository serverRepository,
	ILoggerManager loggerManager) : IRequestHandler<GetAllQuery, Result<List<ServerFullDto>>>
{
	public async Task<Result<List<ServerFullDto>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var servers = await serverRepository.GetAllAsync(readOnly: true);
			return Result.Success(servers.Select(ServerMappings.ToFullDto).ToList());
		}
		catch (Exception ex)
		{
			loggerManager.LogError($"Failure when getting full server list: {ex.Message}");
			return Result.Failure<List<ServerFullDto>>(
				new Error("FailureOnStatus", $"Failure when getting full server list.", ErrorType.Failure));
		}
	}
}

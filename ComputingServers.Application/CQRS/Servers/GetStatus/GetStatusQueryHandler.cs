using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Additional;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Results;
using ComputingServers.Domain.Results.Errors;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.GetStatus;

internal class GetStatusQueryHandler(
	IServerRepository serverRepository,
	ILoggerManager loggerManager) : IRequestHandler<GetStatusQuery, Result<string>>
{
	public async Task<Result<string>> Handle(GetStatusQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var server = await serverRepository.GetByIdAsync(request.Id, readOnly: true);
			if (server is null)
				return Result.Failure<string>(ServerErrors.NotFound(request.Id));

			return Result.Success(ServerHelper.FormStatusResponse(server.Status));
		}
		catch (Exception ex)
		{
			loggerManager.LogError($"Failure when releasing server: {ex.Message}");
			return Result.Failure<string>(ServerErrors.FailureOnStatus(request.Id));
		}
	}
}

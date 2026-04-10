using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Enums;
using ComputingServers.Domain.Results;
using ComputingServers.Domain.Results.Errors;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.Release;

internal class ReleaseServerCommandHandler(
	IServerRepository serverRepository,
	IUnitOfWork unitOfWork,
	ILoggerManager loggerManager) : IRequestHandler<ReleaseServerCommand, Result<bool>>
{
	public async Task<Result<bool>> Handle(ReleaseServerCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var server = await serverRepository.GetByIdAsync(request.Id);
			if (server is null)
				return Result.Failure<bool>(ServerErrors.NotFound(request.Id));

			server.IsAvailable = true;
			server.LastUsed = DateTime.UtcNow;
			server.Status = ServerStatus.PoweredOff;

			await unitOfWork.SaveChangesAsync();

			loggerManager.LogInfo($"Server with ID {server.Id} was successfully released.");
			return Result.Success(true);
		}
		catch (Exception ex)
		{
			loggerManager.LogError($"Failure when releasing server: {ex.Message}");
			return Result.Failure<bool>(ServerErrors.FailureOnRelease(request.Id));
		}
	}
}

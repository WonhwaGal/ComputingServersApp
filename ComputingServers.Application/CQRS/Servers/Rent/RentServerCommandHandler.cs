using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Additional;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Enums;
using ComputingServers.Domain.Results;
using ComputingServers.Domain.Results.Errors;
using ComputingServers.Infrastructure.Jobs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ComputingServers.Application.CQRS.Servers.Rent;

internal class RentServerCommandHandler(
	IServerRepository serverRepository,
	IUnitOfWork unitOfWork,
	IQuartzService quartzService,
	ILoggerManager loggerManager) : IRequestHandler<RentServerCommand, Result<string>>
{
	public async Task<Result<string>> Handle(RentServerCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var server = await serverRepository.GetByIdAsync(request.Id);
			if (server is null)
				return Result.Failure<string>(ServerErrors.NotFound(request.Id));

			if (!server.IsAvailable)
				return Result.Failure<string>(ServerErrors.NotAvailable(request.Id));

			server.IsAvailable = false;
			server.RentedAt = DateTime.UtcNow;

			if (server.Status == ServerStatus.PoweredOff)
			{
				server.Status = ServerStatus.Booting;
				await quartzService.ScheduleJob<ServerPowerOnJob>("ServerId", server.Id, 5);
				await quartzService.ScheduleJob<ServerReleaseJob>("ServerId", server.Id, 20);
			}

			await unitOfWork.SaveChangesAsync();

			loggerManager.LogInfo($"Server with ID {server.Id} was successfully rented with current status {server.Status}");
			return Result.Success(ServerHelper.FormStatusResponse(server.Status));
		}
		catch (DbUpdateConcurrencyException)
		{
			loggerManager.LogError($"Failure when renting: server is already rented");
			return Result.Failure<string>(ServerErrors.NotAvailable(request.Id));
		}
		catch (Exception ex)
		{
			loggerManager.LogError($"Failure when renting server: {ex.Message}");
			return Result.Failure<string>(ServerErrors.FailureOnRent(request.Id));
		}
	}
}

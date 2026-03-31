using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Dtos;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.Add
{
	internal class AddServerCommandHandler(
		IServerRepository serverRepository,
		IUnitOfWork unitOfWork,
		ILoggerManager loggerManager) : IRequestHandler<AddServerCommand, Result<string>>
	{
		public async Task<Result<string>> Handle(AddServerCommand request, CancellationToken cancellationToken)
		{
			var server = ServerMappings.ToEntity(
				request.OS, 
				request.CpuNumber, 
				request.MemoryCapacity, 
				request.DiskCapacity);

			try
			{
				serverRepository.Add(server);
				await unitOfWork.SaveChangesAsync();

				loggerManager.LogInfo($"A new server was successfully added with ID {server.Id}");
				return Result.Success(server.Id);
			}
			catch (Exception ex)
			{
				loggerManager.LogError($"Failure on saving new server to the database: {ex.Message}");
				return Result.Failure<string>(
					new Error("FailureOnSave", "Failure on saving new server to the database.", ErrorType.Failure));
			}
		}
	}
}

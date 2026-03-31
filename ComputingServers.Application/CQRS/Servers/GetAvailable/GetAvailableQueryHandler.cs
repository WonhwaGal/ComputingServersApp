using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Dtos;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Results;
using ComputingServers.Domain.Results.Errors;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.GetAvailable
{
	internal class GetAvailableQueryHandler(
		IServerRepository serverRepository,
		ILoggerManager loggerManager) : IRequestHandler<GetAvailableQuery, Result<List<FilterServerDto>>>
	{
		public async Task<Result<List<FilterServerDto>>> Handle(GetAvailableQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var filter = ServerMappings.ToFilter(request.QueryParameters);
				var result = await serverRepository.GetAvailableAsync(filter, readOnly: true);

				var dto = result.Select(ServerMappings.ToFilterDto).ToList();
				return Result.Success(dto);
			}
			catch(Exception ex)
			{
				loggerManager.LogError($"Failure when requesting available servers: {ex.Message}");
				return Result.Failure<List<FilterServerDto>>(ServerErrors.FailureOnAvailableList);
			}
		}
	}
}

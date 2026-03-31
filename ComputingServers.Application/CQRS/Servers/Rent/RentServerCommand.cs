using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.Rent
{
	public sealed record RentServerCommand(string Id) : IRequest<Result<string>>;
}

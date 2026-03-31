using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.Add
{
	public sealed record class AddServerCommand(
		string OS,
		int MemoryCapacity,
		int DiskCapacity,
		int CpuNumber) : IRequest<Result<string>>;
}
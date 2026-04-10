using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.Release;

public sealed record ReleaseServerCommand(string Id) : IRequest<Result<bool>>;

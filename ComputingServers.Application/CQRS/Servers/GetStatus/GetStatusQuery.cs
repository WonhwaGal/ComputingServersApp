using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.GetStatus;

public sealed record GetStatusQuery(string Id) : IRequest<Result<string>>;

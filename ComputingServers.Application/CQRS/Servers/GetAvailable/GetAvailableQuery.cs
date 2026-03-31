using ComputingServers.Application.Dtos;
using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.GetAvailable;

public sealed record GetAvailableQuery(ServerQueryParameters QueryParameters) : IRequest<Result<List<FilterServerDto>>>;

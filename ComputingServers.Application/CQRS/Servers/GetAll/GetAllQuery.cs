using ComputingServers.Application.Dtos;
using ComputingServers.Domain.Results;
using MediatR;

namespace ComputingServers.Application.CQRS.Servers.GetAll;

public sealed record GetAllQuery: IRequest<Result<List<ServerFullDto>>>;

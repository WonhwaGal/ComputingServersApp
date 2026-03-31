using ComputingServers.Domain.Enums;

namespace ComputingServers.Application.Dtos;

public sealed record ServerQueryParameters(
	string? OS,
	int? MemoryCapacity,
	int? DiskCapacity,
	int? CpuNumber,
	ServerStatus? Status);

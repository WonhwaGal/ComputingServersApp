
using ComputingServers.Domain.Enums;

namespace ComputingServers.Application.Dtos;

public sealed class FilterServerDto
{
	public string Id { get; set; }
	public string OS { get; set; }
	public int MemoryCapacity { get; set; }
	public int DiskCapacity { get; set; }
	public int CpuNumber { get; set; }
	public ServerStatus Status { get; set; }
	public int Version { get; set; }
}

using ComputingServers.Domain.Enums;

namespace ComputingServers.Application.Dtos;

public sealed class ServerFullDto
{
	public string Id { get; set; }
	public string OS { get; set; }
	public int MemoryCapacity { get; set; }
	public int DiskCapacity { get; set; }
	public int CpuNumber { get; set; }
	public ServerStatus Status { get; set; }
	public bool IsAvailable { get; set; } = true;
	public DateTime? RentedAt { get; set; }
	public DateTime? LastUsed { get; set; }
}

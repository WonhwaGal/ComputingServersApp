
using ComputingServers.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ComputingServers.Domain.Entities
{
	public class Server : DatabaseEntity
	{
		public string OS { get; set; }
		public int MemoryCapacity { get; set; }
		public int DiskCapacity { get; set; }
		public int CpuNumber { get; set; }
		public ServerStatus Status { get; set; }
		public bool IsAvailable { get; set; } = true;
		public DateTime? RentedAt { get; set; }
		public DateTime? LastUsed {  get; set; }

		[Timestamp]
		public byte[] Version { get; set; }
	}
}

using ComputingServers.Domain.Entities;
using ComputingServers.Domain.Extensions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ComputingServers.Application.Dtos
{
	public static class ServerMappings
	{
		public static Server ToEntity(string os, int cpuNumber, int memory, int disk)
		{
			return new Server
			{
				Id = $"s_{Guid.NewGuid()}",
				OS = os,
				CpuNumber = cpuNumber,
				MemoryCapacity = memory,
				DiskCapacity = disk,
			};
		}

		public static ServerFullDto ToFullDto(Server server)
		{
			return new ServerFullDto
			{
				Id = server.Id,
				OS = server.OS,
				CpuNumber = server.CpuNumber,
				MemoryCapacity = server.MemoryCapacity,
				DiskCapacity = server.DiskCapacity,
				IsAvailable = server.IsAvailable,
				RentedAt = server.RentedAt,
				LastUsed = server.LastUsed,
				Status = server.Status,
			};
		}

		public static FilterServerDto ToFilterDto(Server server)
		{
			return new FilterServerDto
			{
				Id = server.Id,
				OS = server.OS,
				CpuNumber = server.CpuNumber,
				MemoryCapacity = server.MemoryCapacity,
				DiskCapacity = server.DiskCapacity,
				Status = server.Status,
			};
		}

		public static Expression<Func<Server, bool>> ToFilter(ServerQueryParameters q)
		{
			Expression<Func<Server, bool>> filter = s => s.IsAvailable;
			
			if (q.Status.HasValue)
				filter = filter.AndAlso(s => s.Status == q.Status.Value);

			if (!string.IsNullOrEmpty(q.OS))
				filter = filter.AndAlso(s => s.OS.ToLower() == q.OS.ToLower());

			if (q.MemoryCapacity.HasValue)
				filter = filter.AndAlso(s => s.MemoryCapacity == q.MemoryCapacity.Value);

			if (q.DiskCapacity.HasValue)
				filter = filter.AndAlso(s => s.DiskCapacity == q.DiskCapacity.Value);

			if (q.CpuNumber.HasValue)
				filter = filter.AndAlso(s => s.CpuNumber == q.CpuNumber.Value);

			return filter;
		}
	}
}

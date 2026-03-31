using ComputingServers.Domain.Entities;
using ComputingServers.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputingServers.Infrastructure.Persistence.Configurations
{
	public class ServerConfiguration : IEntityTypeConfiguration<Server>
	{
		public void Configure(EntityTypeBuilder<Server> builder)
		{
			builder.ToTable("Servers");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.OS).HasMaxLength(50);

			builder.Property(x => x.Status)
				.HasConversion(
					x => x.ToString(),
					str => (ServerStatus)Enum.Parse(typeof(ServerStatus), str));

			builder.Property(x => x.Version)
				.IsRowVersion()
				.IsConcurrencyToken();
		}
	}
}

using Quartz;

namespace ComputingServers.Application.Abstractions
{
	public interface IQuartzService
	{
		Task ScheduleJob<TJob>(string jobName, string serverId, int minuteOffset) where TJob : IJob;
	}
}

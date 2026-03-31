using ComputingServers.Application.Abstractions;
using Quartz;

namespace ComputingServers.Infrastructure.Services;

public class QuartzService(ISchedulerFactory schedulerFactory) : IQuartzService
{
	public async Task ScheduleJob<TJob>(string jobName, string serverId, int minuteOffset) where TJob : IJob
	{
		var scheduler = await schedulerFactory.GetScheduler();

		var jobKey = new JobKey(
			$"{typeof(TJob).Name}-{serverId}",
			"server-jobs"
		);

		var job = JobBuilder.Create<TJob>()
			.WithIdentity(jobKey)
			.UsingJobData(jobName, serverId)
			.Build();

		var trigger = TriggerBuilder.Create()
			.StartAt(DateBuilder.FutureDate(minuteOffset, IntervalUnit.Minute))
			.Build();

		await scheduler.ScheduleJob(job, trigger);
	}
}

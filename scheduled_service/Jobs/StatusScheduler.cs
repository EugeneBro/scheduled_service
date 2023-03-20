using Quartz;
using Quartz.Impl;

namespace scheduled_service.Jobs
{
    public class StatusScheduler
    {
        public static async void Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<StatusChecker>().Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("task_checker_trigger", "triggers")
                .StartNow()
                .WithSimpleSchedule(x => x 
                    .WithIntervalInMinutes(2)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}

using Quartz;
using scheduled_service.Database;
using scheduled_service.Database.Models;

namespace scheduled_service.Jobs
{
    public class StatusChecker : IJob
    {
        private IServiceProvider ServiceProvider { get; }

        public StatusChecker(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await using var dbContext = ServiceProvider.GetRequiredService<PostgreDbContext>();

            var someNewTasks = dbContext.SomeTasks
                .Where(x => x.Status == Status.Created);

            if (someNewTasks.Any())
            {
                foreach (var t in someNewTasks)
                {
                    t.LastUpdate = DateTime.UtcNow;
                    t.Status = Status.Running;
                }

                dbContext.SomeTasks.UpdateRange(someNewTasks);
            }

            var someTasks = dbContext.SomeTasks
                .Where(x => x.Status == Status.Running)
                .Where(x => DateTime.Now - x.LastUpdate > TimeSpan.FromMinutes(2));

            if (someTasks.Any())
            {
                foreach (var t in someTasks)
                {
                    t.LastUpdate = DateTime.UtcNow;
                    t.Status = Status.Finished;
                }

                dbContext.SomeTasks.UpdateRange(someTasks);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}

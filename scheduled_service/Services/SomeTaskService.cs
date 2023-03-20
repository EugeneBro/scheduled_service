using System.Net;
using Microsoft.EntityFrameworkCore;
using scheduled_service.Database;
using scheduled_service.Database.Models;
using System.Web;

namespace scheduled_service.Services
{
    public class SomeTaskService
    {
        private IServiceProvider ServiceProvider { get; }

        public SomeTaskService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async Task<SomeTask> GetAsync(Guid id
            , CancellationToken stoppingToken = default)
        {
            await using var context = GetContext<PostgreDbContext>();

            var someTask = await context.SomeTasks
                .FirstOrDefaultAsync(x => x.Id == id, stoppingToken);

            if (someTask == null)
                throw new BadHttpRequestException("Entity was not found", (int)HttpStatusCode.NotFound);

            return someTask;
        }

        public async Task<string> CreateAsync(CancellationToken stoppingToken = default)
        {
            await using var context = GetContext<PostgreDbContext>();

            var added = await context.SomeTasks.AddAsync(
                new SomeTask()
                {
                    Id = Guid.NewGuid(),
                    LastUpdate = DateTime.UtcNow,
                    Status = Status.Created
                }, stoppingToken);

            await context.SaveChangesAsync(stoppingToken);

            return added.Entity.Status.ToString();
        }

        public async Task DeleteAsync()
        {
            await using var context = GetContext<PostgreDbContext>();
        }

        private TContext GetContext<TContext>()
            where TContext : DbContext
        {
            return ServiceProvider.GetRequiredService<TContext>();
        }
    }
}

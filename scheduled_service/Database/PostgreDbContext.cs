using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;
using Npgsql;
using scheduled_service.Database.Models;

namespace scheduled_service.Database
{
    public class PostgreDbContext : DbContext
    {
        public PostgreDbContext(DbContextOptions<PostgreDbContext> options) : base(options)
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Status>("status_enum");
        }

        #region Tables
        
        public DbSet<SomeTask> SomeTasks { get; init; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

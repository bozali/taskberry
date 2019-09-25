namespace TaskBerry.DataAccess.Domain
{
    using Microsoft.EntityFrameworkCore;

    using TaskBerry.Data.Entities;


    public class TaskBerryDbContext : DbContext
    {
        public TaskBerryDbContext(DbContextOptions<TaskBerryDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfoEntity> UserInfos { get; set; }

        public DbSet<GroupEntity> Groups { get; set; }

        public DbSet<TaskEntity> Tasks { get; set; }

        public DbSet<GroupAssignmentEntity> GroupAssignments { get; set; }
    }
}
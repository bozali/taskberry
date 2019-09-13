namespace TaskBerry.DataAccess.Domain
{
    using Microsoft.EntityFrameworkCore;

    using TaskBerry.Data.Entities;


    public class TaskBerryDbContext : DbContext
    {
        public TaskBerryDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<GroupEntity> Groups { get; set; }

        public DbSet<GroupAssignmentEntity> GroupAssignments { get; set; }
    }
}
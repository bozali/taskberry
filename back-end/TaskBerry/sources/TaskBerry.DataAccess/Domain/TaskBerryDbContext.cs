namespace TaskBerry.DataAccess.Domain
{
    using Microsoft.EntityFrameworkCore;


    public class TaskBerryDbContext : DbContext
    {
        public TaskBerryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
namespace TaskBerry.DataAccess.Domain
{
    using Microsoft.EntityFrameworkCore;


    public class TaskBerryDbContext : DbContext, ITaskBerryDbContext
    {
        public TaskBerryDbContext(string connectionString)
        {
            base.Database.GetDbConnection().ConnectionString = connectionString;
        }
    }
}
namespace TaskBerry.Service.DataAccess
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
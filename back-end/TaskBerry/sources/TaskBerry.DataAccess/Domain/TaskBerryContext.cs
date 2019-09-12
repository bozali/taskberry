namespace TaskBerry.DataAccess.Domain
{
    using Microsoft.EntityFrameworkCore;


    public class TaskBerryContext : DbContext, ITaskBerryContext
    {
        public TaskBerryContext(string connectionString)
        {
            // base.Database.GetDbConnection().ConnectionString = connectionString;
        }
    }
}
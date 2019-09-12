namespace TaskBerry.DataAccess.Domain
{
    public class TaskBerryUnitOfWork : ITaskBerryUnitOfWork
    {
        public TaskBerryUnitOfWork(TaskBerryDbContext context)
        {
            this.Context = context;
        }

        public int Commit()
        {
            return this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public TaskBerryDbContext Context { get; set; }
    }
}
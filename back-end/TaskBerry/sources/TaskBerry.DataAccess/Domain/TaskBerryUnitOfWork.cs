namespace TaskBerry.DataAccess.Domain
{
    using TaskBerry.DataAccess.Repositories;


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

        public IGroupRepository GroupsRepository { get; set; }

        public TaskBerryDbContext Context { get; set; }
    }
}
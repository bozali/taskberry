namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;


    public abstract class RepositoryBase<TEntity>
    {

        protected RepositoryBase(ITaskBerryUnitOfWork taskBerry)
        {
            this.TaskBerry = taskBerry;
        }

        protected ITaskBerryUnitOfWork TaskBerry { get; }
    }
}
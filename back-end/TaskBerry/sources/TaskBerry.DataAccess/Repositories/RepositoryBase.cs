namespace TaskBerry.DataAccess.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using TaskBerry.DataAccess.Domain;


    public abstract class RepositoryBase<TEntity> where TEntity : class
    {

        protected RepositoryBase(ITaskBerryUnitOfWork taskBerry)
        {
            this.TaskBerry = taskBerry;
            this.Entities = this.TaskBerry.Context.Set<TEntity>();
        }

        protected DbSet<TEntity> Entities { get; set; }

        protected ITaskBerryUnitOfWork TaskBerry { get; }
    }
}
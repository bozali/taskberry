namespace TaskBerry.DataAccess.Domain
{
    using TaskBerry.DataAccess.Repositories;

    using System;


    public interface ITaskBerryUnitOfWork : IDisposable
    {
        int Commit();

        // TODO Make this repository access generic with GetRepository<TRepository>()

        IGroupRepository GroupsRepository { get; set; }

        TaskBerryDbContext Context { get; set; }
    }
}
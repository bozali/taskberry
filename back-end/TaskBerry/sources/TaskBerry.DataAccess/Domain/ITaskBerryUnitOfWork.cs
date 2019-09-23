namespace TaskBerry.DataAccess.Domain
{
    using TaskBerry.DataAccess.Repositories;

    using System;


    public interface ITaskBerryUnitOfWork : IDisposable
    {
        int Commit();

        // TODO Make this repository access generic with GetRepository<TRepository>()

        IGroupRepository GroupsRepository { get; set; }

        IUserRepository UsersRepository { get; set; }

        TaskBerryDbContext TaskBerryContext { get; set; }

        MoodleDbContext MoodleContext { get; set; }
    }
}
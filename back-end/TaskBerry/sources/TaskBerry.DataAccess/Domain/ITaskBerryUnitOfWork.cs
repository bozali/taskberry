namespace TaskBerry.DataAccess.Domain
{
    using System;


    public interface ITaskBerryUnitOfWork : IDisposable
    {
        int Commit();

        TaskBerryDbContext Context { get; set; }
    }
}
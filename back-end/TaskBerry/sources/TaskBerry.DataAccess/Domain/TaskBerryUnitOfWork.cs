namespace TaskBerry.DataAccess.Domain
{
    using TaskBerry.DataAccess.Repositories;

    using AutoMapper;


    public class TaskBerryUnitOfWork : ITaskBerryUnitOfWork
    {
        public TaskBerryUnitOfWork(TaskBerryDbContext taskBerryContext, MoodleDbContext moodleContext, IMapper mapper)
        {
            this.TaskBerryContext = taskBerryContext;
            this.MoodleContext = moodleContext;

            this.GroupsRepository = new GroupRepository(this);
            this.UsersRepository = new UserRepository(this, mapper);
        }

        public int Commit()
        {
            return this.TaskBerryContext.SaveChanges();
        }

        public void Dispose()
        {
            this.TaskBerryContext.Dispose();
            this.MoodleContext.Dispose();
        }

        public IGroupRepository GroupsRepository { get; set; }

        public IUserRepository UsersRepository { get; set; }

        public TaskBerryDbContext TaskBerryContext { get; set; }

        public MoodleDbContext MoodleContext { get; set; }
    }
}
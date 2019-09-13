namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;


    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(ITaskBerryUnitOfWork taskBerry) : base(taskBerry)
        {
        }
    }
}
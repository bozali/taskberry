namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;


    public class GroupsRepository : RepositoryBase, IGroupsRepository
    {
        public GroupsRepository(ITaskBerryUnitOfWork taskBerry) : base(taskBerry)
        {
        }
    }
}
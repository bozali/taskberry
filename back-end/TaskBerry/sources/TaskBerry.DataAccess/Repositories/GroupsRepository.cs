namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;


    public class GroupsRepository : IGroupsRepository
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        public GroupsRepository(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }
    }
}
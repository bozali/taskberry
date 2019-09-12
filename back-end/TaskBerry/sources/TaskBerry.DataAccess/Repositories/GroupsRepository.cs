namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;


    public class GroupsRepository : IGroupsRepository
    {
        private readonly ITaskBerryContext _context;


        public GroupsRepository(ITaskBerryContext context)
        {
            this._context = context;
        }
    }
}
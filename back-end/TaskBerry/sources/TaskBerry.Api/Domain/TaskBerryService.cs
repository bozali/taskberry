using TaskBerry.Api.Interfaces;

namespace TaskBerry.Api.Domain
{
    public class TaskBerryService : ITaskBerryService
    {
        private readonly TaskBerryContext _context;

        public TaskBerryService(TaskBerryContext context)
        {
            this._context = context;
        }

        public IGroupService CreateGroupService()
        {
            return new GroupService(this._context);
        }
    }
}
using System.Linq;

namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;

    using System.Collections.Generic;
    using System;


    public class GroupsRepository : RepositoryBase<GroupEntity>, IGroupsRepository
    {
        public GroupsRepository(ITaskBerryUnitOfWork taskBerry) : base(taskBerry)
        {
        }

        public IEnumerable<GroupEntity> GetGroups()
        {
            return this.Entities;
        }

        public IEnumerable<GroupEntity> GetGroupsByName(string name)
        {
            return this.Entities.Where(entity => entity.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<GroupEntity> GetGroupsByUserId(Guid userId)
        {
            return null;
        }

        public GroupEntity GetGroupById(Guid id)
        {
            throw new NotImplementedException();
        }

        public GroupEntity CreateGroup(GroupEntity @group)
        {
            throw new NotImplementedException();
        }
    }
}
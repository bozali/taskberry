namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;

    using System.Collections.Generic;
    using System.Linq;
    using System;


    public class GroupRepository : RepositoryBase, IGroupRepository
    {
        public GroupRepository(ITaskBerryUnitOfWork taskBerry) : base(taskBerry)
        {
        }

        public IEnumerable<GroupEntity> GetGroups()
        {
            return this.TaskBerry.TaskBerryContext.Groups;
        }

        public IEnumerable<GroupEntity> GetGroupsByUserId(int userId)
        {
            IEnumerable<GroupAssignmentEntity> assignments = this.TaskBerry.TaskBerryContext.GroupAssignments.Where(assignment => assignment.UserId == userId);

            return assignments.Select(entity => this.TaskBerry.TaskBerryContext.Groups.FirstOrDefault(group => @group.Id == entity.GroupId));
        }

        public GroupEntity CreateGroup(GroupEntity @group)
        {
            this.TaskBerry.TaskBerryContext.Groups.Add(group);
            this.TaskBerry.Commit();

            return group; // TODO
        }

        public IEnumerable<GroupAssignmentEntity> GetGroupAssignment(Guid groupId)
        {
            return this.TaskBerry.TaskBerryContext.GroupAssignments.Where(a => a.GroupId == groupId);
        }
    }
}
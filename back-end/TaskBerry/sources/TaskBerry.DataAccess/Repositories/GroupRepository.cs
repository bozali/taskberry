using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
            return this.TaskBerry.Context.Groups;
        }

        public IEnumerable<GroupEntity> GetGroupsByName(string name)
        {
            return this.TaskBerry.Context.Groups.Where(entity => entity.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<GroupEntity> GetGroupsByUserId(Guid userId)
        {
            IEnumerable<GroupAssignmentEntity> assignments = this.TaskBerry.Context.GroupAssignments.Where(assignment => assignment.UserId == userId);
            return assignments.Select(entity => this.TaskBerry.Context.Groups.FirstOrDefault(group => @group.Id == entity.GroupId));
        }

        public GroupEntity GetGroupById(Guid id)
        {
            return this.TaskBerry.Context.Groups.FirstOrDefault(group => group.Id == id);
        }

        public GroupEntity CreateGroup(GroupEntity @group)
        {
            this.TaskBerry.Context.Groups.Add(group);
            this.TaskBerry.Commit();

            return group; // TODO
        }
    }
}
namespace TaskBerry.Business.Services
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;
    using System;


    public class GroupsService : IGroupsService
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        public GroupsService(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        public IEnumerable<Group> GetGroups()
        {
            IEnumerable<GroupEntity> groupEntities = this._taskBerry.GroupsRepository.GetGroups();

            List<Group> groups = new List<Group>();

            foreach (GroupEntity groupEntity in groupEntities)
            {
                IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.GroupsRepository.GetGroupAssignment(groupEntity.Id);

                Group group = groupEntity.ToModel();
                group.Members = new List<int>();

                foreach (GroupAssignmentEntity assignmentEntity in assignments)
                {
                    group.Members.Add(assignmentEntity.UserId);
                }

                groups.Add(group);
            }

            return groups;
        }

        public Group CreateGroup(Group @group)
        {
            throw new NotImplementedException();
        }

        public Group AssignUsersToGroup(int[] users, Guid groupId)
        {
            GroupEntity groupEntity = this._taskBerry.GroupsRepository.GetGroups().FirstOrDefault(g => g.Id == groupId);

            if (groupEntity == null)
            {
                return null;
            }

            // TODO Make the member assignment bitiful
            Group group = groupEntity.ToModel();
            group.Members = new List<int>();

            IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.Context.GroupAssignments;

            foreach (int userId in users)
            {
                // Check if user is already assigned to this group
                if (assignments.Any(a => a.GroupId == groupId && a.UserId == userId))
                {
                    continue;
                }

                // TODO Check if userid exists.
                GroupAssignmentEntity assignment = new GroupAssignmentEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    GroupId = groupId
                };

                this._taskBerry.Context.GroupAssignments.Add(assignment);
            }

            foreach (GroupAssignmentEntity assignment in this._taskBerry.Context.GroupAssignments)
            {
                group.Members.Add(assignment.UserId);
            }

            // TODO Check if saved successfully
            this._taskBerry.Context.SaveChanges();

            return group;
        }

        public Group RemoveUsersFromGroup(int[] users, Guid groupId)
        {
            throw new NotImplementedException();
        }

        public Group EditGroup(Group groups)
        {
            throw new NotImplementedException();
        }
    }
}

namespace TaskBerry.Business.Services
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;
    using System;

    using AutoMapper;


    public class GroupsService : IGroupsService
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;
        private readonly IMapper _mapper;


        public GroupsService(ITaskBerryUnitOfWork taskBerry, IMapper mapper)
        {
            this._taskBerry = taskBerry;
            this._mapper = mapper;
        }

        public IEnumerable<Group> GetGroups()
        {
            IEnumerable<GroupEntity> groupEntities = this._taskBerry.GroupsRepository.GetGroups();

            List<Group> groups = new List<Group>();

            foreach (GroupEntity groupEntity in groupEntities)
            {
                IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.GroupsRepository.GetGroupAssignment(groupEntity.Id);

                Group group = this._mapper.Map<Group>(groupEntity);
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
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (string.IsNullOrEmpty(group.Name))
            {
                throw new ArgumentException("GroupsService group.Name parameter is null.");
            }

            group.Id = Guid.NewGuid();
            group.Description = group.Description ?? "";

            GroupEntity entity = this._mapper.Map<GroupEntity>(group);

            this._taskBerry.GroupsRepository.CreateGroup(entity);
            return group;
        }

        public Group AssignUsersToGroup(int[] users, Guid groupId)
        {
            GroupEntity groupEntity = this._taskBerry.GroupsRepository.GetGroups().FirstOrDefault(g => g.Id == groupId);

            if (groupEntity == null)
            {
                return null;
            }

            // TODO Make the member assignment bitiful
            Group group = this._mapper.Map<Group>(groupEntity);
            group.Members = new List<int>();

            IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.TaskBerryContext.GroupAssignments;

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

                this._taskBerry.TaskBerryContext.GroupAssignments.Add(assignment);
            }

            foreach (GroupAssignmentEntity assignment in this._taskBerry.TaskBerryContext.GroupAssignments)
            {
                group.Members.Add(assignment.UserId);
            }

            // TODO Check if saved successfully
            this._taskBerry.TaskBerryContext.SaveChanges();

            return group;
        }
    }
}

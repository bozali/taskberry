namespace TaskBerry.Business.Services
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;


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
    }
}

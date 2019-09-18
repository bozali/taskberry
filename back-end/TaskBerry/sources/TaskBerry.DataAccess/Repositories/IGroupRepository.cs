namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.Data.Entities;

    using System.Collections.Generic;
    using System;


    public interface IGroupRepository
    {
        IEnumerable<GroupEntity> GetGroups();

        IEnumerable<GroupEntity> GetGroupsByUserId(int userId);

        IEnumerable<GroupAssignmentEntity> GetGroupAssignment(Guid groupId);

        GroupEntity CreateGroup(GroupEntity group);
    }
}
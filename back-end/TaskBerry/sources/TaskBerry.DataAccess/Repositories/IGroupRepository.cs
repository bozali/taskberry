namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.Data.Entities;

    using System.Collections.Generic;
    using System;


    public interface IGroupRepository
    {
        IEnumerable<GroupEntity> GetGroups();

        IEnumerable<GroupEntity> GetGroupsByName(string name);

        IEnumerable<GroupEntity> GetGroupsByUserId(Guid userId);

        GroupEntity GetGroupById(Guid id);

        GroupEntity CreateGroup(GroupEntity group);
    }
}
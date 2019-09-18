using System;

namespace TaskBerry.Business.Services
{
    using System.Collections.Generic;

    using TaskBerry.Data.Models;


    public interface IGroupsService
    {
        IEnumerable<Group> GetGroups();

        Group CreateGroup(Group group);

        Group AssignUsersToGroup(int[] users, Guid groupId);

        Group RemoveUsersFromGroup(int[] users, Guid groupId);

        Group EditGroup(Group groups);
    }
}

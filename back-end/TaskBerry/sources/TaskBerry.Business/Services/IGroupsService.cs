namespace TaskBerry.Business.Services
{
    using System.Collections.Generic;
    using System;

    using TaskBerry.Data.Models;


    public interface IGroupsService
    {
        IEnumerable<Group> GetGroups();

        Group CreateGroup(Group group);

        Group AssignUsersToGroup(int[] users, Guid groupId);

    }
}

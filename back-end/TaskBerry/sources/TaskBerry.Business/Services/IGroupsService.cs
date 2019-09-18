namespace TaskBerry.Business.Services
{
    using System.Collections.Generic;

    using TaskBerry.Data.Models;


    public interface IGroupsService
    {
        IEnumerable<Group> GetGroups();
    }
}

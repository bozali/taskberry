using System.Text.RegularExpressions;

namespace TaskBerry.Api.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TaskBerry.Data.Models;


    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetGroupsAsync();
    }
}
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using TaskBerry.Api.Interfaces;

namespace TaskBerry.Api.Domain
{
    using System.Collections.Generic;

    using System.Threading.Tasks;

    using TaskBerry.Data.Models;


    public class GroupService : IGroupService
    {
        private readonly TaskBerryContext _context;

        public GroupService(TaskBerryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            using (HttpResponseMessage message = await this._context.Client.GetAsync(""))
            {
                message.EnsureSuccessStatusCode();

                return null; // TODO Deserialize http response
            }
        }
    }
}
namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using TaskBerry.Service.DataAccess;
    using TaskBerry.Shared.Entities;

    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private ITaskBerryDbContext _database;

        public GroupsController(ITaskBerryDbContext database)
        {
            this._database = database;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets groups.", Description = "Gets a null.")]
        public ActionResult<IEnumerable<GroupEntity>> Get()
        {
            return null;
        }
    }
}
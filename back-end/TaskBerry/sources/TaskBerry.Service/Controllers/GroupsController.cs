namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private ITaskBerryContext _database;

        public GroupsController(ITaskBerryContext database)
        {
            this._database = database;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets groups.", Description = "Gets a null.")]
        public ActionResult<IEnumerable<Group>> Get()
        {
            return this.Ok();
        }
    }
}
namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using TaskBerry.Shared.Entities;

    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        public GroupsController()
        {
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets groups.", Description = "Gets a null.")]
        public ActionResult<IEnumerable<GroupEntity>> Get()
        {
            return null;
        }
    }
}
namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;

    using System.Collections.Generic;
    using System;

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
            return new GroupEntity[]
            {
                new GroupEntity { Description = "Test description 1", Name = "Test name", Id = Guid.NewGuid() },
                new GroupEntity { Description = "Test description 2", Name = "Test name", Id = Guid.NewGuid() }
            };
        }
    }
}
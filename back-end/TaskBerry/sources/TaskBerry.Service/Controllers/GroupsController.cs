namespace TaskBerry.Service.Controllers
{
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System;

    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        // private ITaskBerryContext _database;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="database"></param>
        public GroupsController(/*ITaskBerryContext database*/)
        {
            // this._database = database;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            return this.Ok();
        }

        [HttpGet("{name}")]
        public ActionResult<IEnumerable<Group>> GetGroupsByName(string name)
        {
            return this.Ok();
        }

        [HttpGet("getById/{userId}")]
        public ActionResult<IEnumerable<Group>> GetGroupsByUserId(Guid userId)
        {
            return this.Ok();
        }

        [HttpGet("id/{id}")]
        public ActionResult<Group> GetGroupById(Guid id)
        {
            return this.Ok();
        }

        [HttpPost]
        public ActionResult<Group> CreateGroup([FromBody] Group group)
        {
            return this.Ok();
        }
    }
}
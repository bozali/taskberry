namespace TaskBerry.Service.Controllers
{
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System;

    using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [Authorize]
        public ActionResult<IEnumerable<Group>> GetGroupsByName(string name)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getById/{userId}")]
        [Authorize]
        public ActionResult<IEnumerable<Group>> GetGroupsByUserId(Guid userId)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        [Authorize]
        public ActionResult<Group> GetGroupById(Guid id)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult<Group> CreateGroup([FromBody] Group group)
        {
            return this.Ok();
        }
    }
}
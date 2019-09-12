using System;

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

        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetGroupsByName(string name)
        {
            return this.Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetGroupsByUserId(Guid userId)
        {
            return this.Ok();
        }

        [HttpGet]
        public ActionResult<Group> GetGroupById(Guid id)
        {
            return this.Ok();
        }
    }
}
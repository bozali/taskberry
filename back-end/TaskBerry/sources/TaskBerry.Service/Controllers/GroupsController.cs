using System.Collections;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;
using TaskBerry.Data.Entities;

namespace TaskBerry.Service.Controllers
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GroupsController(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // [Authorize]
        [SwaggerResponse(200, "Returned all groups successfully.")]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            IEnumerable<GroupEntity> entities = this._taskBerry.GroupsRepository.GetGroups();
            
            return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        // [Authorize]
        public ActionResult<IEnumerable<Group>> GetGroupsByName(string name)
        {
            IEnumerable<GroupEntity> entities = this._taskBerry.GroupsRepository.GetGroupsByName(name);

            return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("/userid/{userId}")]
        // [Authorize]
        public ActionResult<IEnumerable<Group>> GetGroupsByUserId(Guid userId)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/id/{id}")]
        // [Authorize]
        public ActionResult<Group> GetGroupById(Guid id)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        // [Authorize]
        public ActionResult<Group> CreateGroup([FromBody] Group group)
        {
            return this.Ok();
        }
    }
}
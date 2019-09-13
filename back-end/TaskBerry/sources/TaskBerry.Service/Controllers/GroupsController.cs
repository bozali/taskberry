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
        [Produces("application/json")]
        [SwaggerResponse(200, "Returned all groups successfully.")]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            return this.Ok(new Group[]
            {
                new Group { Name = "Test 1" },
                new Group { Name = "Test 2" },
                new Group { Name = "Test 3" },
                new Group { Name = "Test 4" }
            });
            // IEnumerable<GroupEntity> entities = this._taskBerry.GroupsRepository.GetGroups();
            // 
            // return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [Produces("application/json")]
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
        [Produces("application/json")]
        public ActionResult<IEnumerable<Group>> GetGroupsByUserId(Guid userId)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/id/{id}")]
        [Produces("application/json")]
        public ActionResult<Group> GetGroupById(Guid id)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Produces("application/json")]
        //public ActionResult<Group> CreateGroup([FromBody] Group group)
        //{
        //    return this.Ok();
        //}
    }
}
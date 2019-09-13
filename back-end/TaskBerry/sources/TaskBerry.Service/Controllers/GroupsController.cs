namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;
    using System;

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
            IEnumerable<GroupEntity> entities = this._taskBerry.GroupsRepository.GetGroups();
            
            return this.Ok(entities.Select(entity => entity.ToModel()));
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
        [HttpGet("/byUser/{userId:guid}")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Group>> GetGroupsByUserId(Guid userId)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/{id:guid}")]
        [HttpGet("/byGroup/{id:guid}")]
        [Produces("application/json")]
        public ActionResult<Group> GetGroupById(Guid id)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost("/new")]
        [Produces("application/json")]
        public ActionResult<Group> CreateGroup([FromBody] Group group)
        {
            return this.Ok();
        }
    }
}
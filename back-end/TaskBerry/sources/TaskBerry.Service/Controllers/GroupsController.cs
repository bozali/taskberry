using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
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
        [HttpGet("/byUser/{userId:int}")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Group>> GetGroupsByUserId(int userId)
        {
            IEnumerable<GroupEntity> entities = this._taskBerry.GroupsRepository.GetGroupsByUserId(userId);

            return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/byGroup/{id:guid}")]
        [Produces("application/json")]
        public ActionResult<Group> GetGroupById(Guid id)
        {
            GroupEntity entity = this._taskBerry.GroupsRepository.GetGroupById(id);

            if (entity == null)
            {
                return this.NotFound(id);
            }

            return this.Ok(entity.ToModel());
        }

        /// <summary>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost("/new")]
        [Produces("application/json")]
        public ActionResult<Group> CreateGroup([FromBody] Group group)
        {
            GroupEntity entity = new GroupEntity
            {
                Id = Guid.NewGuid(),
                Name = group.Name,
                Description = group.Description,
            };

            this._taskBerry.GroupsRepository.CreateGroup(entity);

            return this.Ok(entity.ToModel()); // TODO CreateResult?
        }
    }
}